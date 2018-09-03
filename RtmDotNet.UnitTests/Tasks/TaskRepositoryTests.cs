// -----------------------------------------------------------------------
//     This file is part of RtmDotNet.
//     Copyright (c) 2018 Aaron Morris
//     https://github.com/aaron-morris/RtmDotNet
// 
//     RtmDotNet is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     RtmDotNet is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with RtmDotNet.  If not, see <https://www.gnu.org/licenses/>.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using RtmDotNet.Http.Api.Tasks;
using RtmDotNet.Tasks;

namespace RtmDotNet.UnitTests.Tasks
{
    using NUnit.Framework;

    [TestFixture]
    public class TaskRepositoryTests
    {
        [Test]
        public async Task GetAllTasksAsync_DefaultOperation_ExcludesCompleted()
        {
            // Setup
            var fakeResponseData = new GetListResponseData();
            var fakeTaskClient = Substitute.For<ITaskApiClient>();
            fakeTaskClient.GetAllTasksAsync().Returns(Task.FromResult(fakeResponseData));

            var fakeTaskList = new List<IRtmTask>();
            var fakeResponseParser = Substitute.For<IResponseParser>();
            fakeResponseParser.GetTasks(fakeResponseData).Returns(fakeTaskList);

            IList<IRtmTask> expectedTaskList = new List<IRtmTask>();
            var mockTaskCache = Substitute.For<ITaskCache>();
            mockTaskCache.GetAllAsync().Returns(Task.FromResult(expectedTaskList));

            // Execute
            var taskRepository = GetItemUnderTest(fakeTaskClient, fakeResponseParser, mockTaskCache);
            var actual = await taskRepository.GetAllTasksAsync().ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedTaskList, actual);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            mockTaskCache.Received().ClearAsync();
            mockTaskCache.Received().AddOrReplaceAsync(fakeTaskList);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Test]
        public async Task GetAllTasksAsync_ExplicitExcludeOption_ExcludesCompleted()
        {
            // Setup
            var fakeResponseData = new GetListResponseData();
            var fakeTaskClient = Substitute.For<ITaskApiClient>();
            fakeTaskClient.GetAllTasksAsync().Returns(Task.FromResult(fakeResponseData));

            var fakeTaskList = new List<IRtmTask>();
            var fakeResponseParser = Substitute.For<IResponseParser>();
            fakeResponseParser.GetTasks(fakeResponseData).Returns(fakeTaskList);

            IList<IRtmTask> expectedTaskList = new List<IRtmTask>();
            var mockTaskCache = Substitute.For<ITaskCache>();
            mockTaskCache.GetAllAsync().Returns(Task.FromResult(expectedTaskList));

            // Execute
            var taskRepository = GetItemUnderTest(fakeTaskClient, fakeResponseParser, mockTaskCache);
            // ReSharper disable once RedundantArgumentDefaultValue
            var actual = await taskRepository.GetAllTasksAsync(false).ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedTaskList, actual);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            mockTaskCache.Received().ClearAsync();
            mockTaskCache.Received().AddOrReplaceAsync(fakeTaskList);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Test]
        public async Task GetAllTasksAsync_ExplicitIncludeOption_IncludesCompleted()
        {
            // Setup
            var fakeResponseData = new GetListResponseData();
            var fakeTaskClient = Substitute.For<ITaskApiClient>();
            fakeTaskClient.GetAllTasksAsync(true).Returns(Task.FromResult(fakeResponseData));

            var fakeTaskList = new List<IRtmTask>();
            var fakeResponseParser = Substitute.For<IResponseParser>();
            fakeResponseParser.GetTasks(fakeResponseData).Returns(fakeTaskList);

            IList<IRtmTask> expectedTaskList = new List<IRtmTask>();
            var mockTaskCache = Substitute.For<ITaskCache>();
            mockTaskCache.GetAllAsync().Returns(Task.FromResult(expectedTaskList));

            // Execute
            var taskRepository = GetItemUnderTest(fakeTaskClient, fakeResponseParser, mockTaskCache);
            var actual = await taskRepository.GetAllTasksAsync(true).ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedTaskList, actual);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            mockTaskCache.Received().ClearAsync();
            mockTaskCache.Received().AddOrReplaceAsync(fakeTaskList);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        [Test]
        public async Task GetTasksByListIdAsync_ListNotSyncedYet_GetsAllTasksForList()
        {
            // Setup
            const string listId = "My Fake List ID";

            var fakeSyncTracker = Substitute.For<ISyncTracker>();
            fakeSyncTracker.GetLastSync(listId).Returns((DateTime?)null);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<ITaskApiClient>();
            fakeApiClient.GetTasksByListIdAsync(listId).Returns(Task.FromResult(fakeResponseData));

            var fakeTaskList = new List<IRtmTask>();
            var fakeDeletedTaskList = new List<IRtmTask>();
            var fakeResponseParser = Substitute.For<IResponseParser>();
            fakeResponseParser.GetTasks(fakeResponseData).Returns(fakeTaskList);
            fakeResponseParser.GetDeletedTasks(fakeResponseData).Returns(fakeDeletedTaskList);

            IList<IRtmTask> expectedTaskList = new List<IRtmTask>();
            var mockTaskCache = Substitute.For<ITaskCache>();
            mockTaskCache.GetAllAsync(listId).Returns(Task.FromResult(expectedTaskList));
            
            // Execute
            var taskRepository = GetItemUnderTest(fakeApiClient, fakeResponseParser, mockTaskCache, fakeSyncTracker);
            var actual = await taskRepository.GetTasksByListIdAsync(listId).ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedTaskList, actual);
#pragma warning disable 4014
            mockTaskCache.DidNotReceive().ClearAsync();
            mockTaskCache.Received(1).AddOrReplaceAsync(listId, fakeTaskList, true);
            mockTaskCache.DidNotReceive().RemoveAsync(fakeDeletedTaskList);
#pragma warning restore 4014
        }

        [Test]
        public async Task GetTasksByListIdAsync_ListAlreadySynced_IgnoresLastSyncDate()
        {
            // Setup
            const string listId = "My Fake List ID";
            var lastSync = DateTime.Parse("2018-01-23 12:34:56");

            var fakeSyncTracker = Substitute.For<ISyncTracker>();
            fakeSyncTracker.GetLastSync(listId).Returns(lastSync);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<ITaskApiClient>();
            fakeApiClient.GetTasksByListIdAsync(listId).Returns(Task.FromResult(fakeResponseData));

            var fakeTaskList = new List<IRtmTask>();
            var fakeDeletedTaskList = new List<IRtmTask>();
            var fakeResponseParser = Substitute.For<IResponseParser>();
            fakeResponseParser.GetTasks(fakeResponseData).Returns(fakeTaskList);
            fakeResponseParser.GetDeletedTasks(fakeResponseData).Returns(fakeDeletedTaskList);

            IList<IRtmTask> expectedTaskList = new List<IRtmTask>();
            var mockTaskCache = Substitute.For<ITaskCache>();
            mockTaskCache.GetAllAsync(listId).Returns(Task.FromResult(expectedTaskList));

            // Execute
            var taskRepository = GetItemUnderTest(fakeApiClient, fakeResponseParser, mockTaskCache, fakeSyncTracker);
            var actual = await taskRepository.GetTasksByListIdAsync(listId).ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedTaskList, actual);
#pragma warning disable 4014
            mockTaskCache.DidNotReceive().ClearAsync();
            mockTaskCache.Received(1).AddOrReplaceAsync(listId, fakeTaskList, true);
            mockTaskCache.DidNotReceive().RemoveAsync(fakeDeletedTaskList);
#pragma warning restore 4014
        }

        [Test]
        public async Task GetTasksByListIdAsync_HasTasksFromOtherLists_GetsOtherListsTooUsingLastSync()
        {
            // Setup
            const string masterListId = "My Master List ID";
            const string firstForeignListId = "First Foreign List ID";
            const string secondForeignListId = "Second Foreign List ID";

            var masterLastSync = DateTime.Parse("2018-01-23 12:34:56");
            var firstForeignLastSync = masterLastSync.Subtract(TimeSpan.FromSeconds(1));
            var secondForeignLastSync = masterLastSync.Add(TimeSpan.FromSeconds(1));

            var testSyncTracker = new InMemorySyncTracker();
            testSyncTracker.SetLastSync(masterListId, masterLastSync);
            testSyncTracker.SetLastSync(firstForeignListId, firstForeignLastSync);
            testSyncTracker.SetLastSync(secondForeignListId, secondForeignLastSync);

            var masterResponseData = new GetListResponseData();
            var firstForeignResponseData = new GetListResponseData();
            var secondForeignResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<ITaskApiClient>();
            fakeApiClient.GetTasksByListIdAsync(masterListId).Returns(Task.FromResult(masterResponseData));
            fakeApiClient.GetTasksByListIdAsync(firstForeignListId, firstForeignLastSync).Returns(Task.FromResult(firstForeignResponseData));
            fakeApiClient.GetTasksByListIdAsync(secondForeignListId, secondForeignLastSync).Returns(Task.FromResult(secondForeignResponseData));

            var firstTask = Substitute.For<IRtmTask>();
            firstTask.ListId.Returns(firstForeignListId);
            var secondTask = Substitute.For<IRtmTask>();
            secondTask.ListId.Returns(secondForeignListId);

            var masterTaskList = new List<IRtmTask> { firstTask, secondTask};
            var firstForeignTaskList = new List<IRtmTask> { firstTask, secondTask };
            var secondForeignTaskList = new List<IRtmTask> { firstTask, secondTask };
            var masterDeletedTaskList = new List<IRtmTask>();
            var firstForeignDeletedTaskList = new List<IRtmTask>();
            var secondForeignDeletedTaskList = new List<IRtmTask>();
            var fakeResponseParser = Substitute.For<IResponseParser>();
            fakeResponseParser.GetTasks(masterResponseData).Returns(masterTaskList);
            fakeResponseParser.GetDeletedTasks(masterResponseData).Returns(masterDeletedTaskList);
            fakeResponseParser.GetTasks(firstForeignResponseData).Returns(firstForeignTaskList);
            fakeResponseParser.GetDeletedTasks(firstForeignResponseData).Returns(firstForeignDeletedTaskList);
            fakeResponseParser.GetTasks(secondForeignResponseData).Returns(secondForeignTaskList);
            fakeResponseParser.GetDeletedTasks(secondForeignResponseData).Returns(secondForeignDeletedTaskList);

            IList<IRtmTask> expectedTaskList = new List<IRtmTask>();
            var mockTaskCache = Substitute.For<ITaskCache>();
            mockTaskCache.GetAllAsync(masterListId).Returns(Task.FromResult(expectedTaskList));

            // Execute
            var taskRepository = GetItemUnderTest(fakeApiClient, fakeResponseParser, mockTaskCache, testSyncTracker);
            var actual = await taskRepository.GetTasksByListIdAsync(masterListId).ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedTaskList, actual);
#pragma warning disable 4014
            mockTaskCache.DidNotReceive().ClearAsync();
            mockTaskCache.Received(1).AddOrReplaceAsync(masterListId, masterTaskList, true);
            mockTaskCache.DidNotReceive().RemoveAsync(masterDeletedTaskList);
            mockTaskCache.Received(1).AddOrReplaceAsync(firstForeignListId, firstForeignTaskList);
            mockTaskCache.Received(1).RemoveAsync(firstForeignDeletedTaskList);
            mockTaskCache.Received(1).AddOrReplaceAsync(secondForeignListId, secondForeignTaskList);
            mockTaskCache.Received(1).RemoveAsync(secondForeignDeletedTaskList);
#pragma warning restore 4014
        }

        private TaskRepository GetItemUnderTest(ITaskApiClient taskApiClient, IResponseParser responseParser, ITaskCache taskCache)
        {
            return GetItemUnderTest(taskApiClient, responseParser, taskCache, Substitute.For<ISyncTracker>());
        }

        private TaskRepository GetItemUnderTest(ITaskApiClient taskApiClient, IResponseParser responseParser, ITaskCache taskCache, ISyncTracker syncTracker)
        {
            return new TaskRepository(taskApiClient, responseParser, taskCache, syncTracker);
        }
    }
}