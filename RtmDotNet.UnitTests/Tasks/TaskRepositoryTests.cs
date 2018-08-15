// -----------------------------------------------------------------------
// <copyright file="TaskRepositoryTests.cs" author="Aaron Morris">
//      This file is part of RtmDotNet.
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
//  </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Auth;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Tasks;
using RtmDotNet.Tasks;

namespace RtmDotNet.UnitTests.Tasks
{
    [TestFixture]
    public class TaskRepositoryTests
    {
        [Test]
        public async Task GetAllListsAsync_DefaultOperation_IncludesIncompleteFilter()
        {
            // Setup
            var expectedTasks = new List<IRtmTask> { new RtmTask { Name = "My Fake Task" } };

            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeListsUrl = "My Fake URL";

            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, filter: "status:incomplete").Returns(fakeListsUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeListsUrl).Returns(Task.FromResult(fakeResponseData));

            var fakeTaskConverter = Substitute.For<IRtmTaskConverter>();
            fakeTaskConverter.ConvertToTasks(fakeResponseData).Returns(expectedTasks);

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken, fakeTaskConverter);
            var actual = await taskRepository.GetAllTasksAsync().ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedTasks, actual);
        }

        [Test]
        public async Task GetAllListsAsync_ExplicitExcludeOption_IncludesIncompleteFilter()
        {
            // Setup
            var expectedTasks = new List<IRtmTask> { new RtmTask { Name = "My Fake Task" } };

            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeListsUrl = "My Fake URL";

            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, filter: "status:incomplete").Returns(fakeListsUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeListsUrl).Returns(Task.FromResult(fakeResponseData));

            var fakeTaskConverter = Substitute.For<IRtmTaskConverter>();
            fakeTaskConverter.ConvertToTasks(fakeResponseData).Returns(expectedTasks);

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken, fakeTaskConverter);
            var actual = await taskRepository.GetAllTasksAsync(false).ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedTasks, actual);
        }

        [Test]
        public async Task GetAllListsAsync_IncludeCompletedTasks_ExcludesIncompleteFilter()
        {
            // Setup
            var expectedTasks = new List<IRtmTask> { new RtmTask { Name = "My Fake Task" } };

            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeListsUrl = "My Fake URL";

            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id).Returns(fakeListsUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeListsUrl).Returns(Task.FromResult(fakeResponseData));

            var fakeTaskConverter = Substitute.For<IRtmTaskConverter>();
            fakeTaskConverter.ConvertToTasks(fakeResponseData).Returns(expectedTasks);

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken, fakeTaskConverter);
            var actual = await taskRepository.GetAllTasksAsync(true).ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedTasks, actual);
        }

        [TestCase(PermissionLevel.Read)]
        [TestCase(PermissionLevel.Write)]
        [TestCase(PermissionLevel.Delete)]
        public async Task GetAllListsAsync_SufficientPermissions_NoPermissionException(PermissionLevel permissionLevel)
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = permissionLevel };
            
            // Execute
            var taskRepository = GetItemUnderTest(fakeAuthToken);
            var actual = await taskRepository.GetAllTasksAsync().ConfigureAwait(false);

            // Verify
            Assert.Pass("No permission exception was thrown.");
        }

        [Test]
        public void GetAllListsAsync_InsufficientPermissions_ThrowsInvalidOperationError()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Undefined };

            // Execute
            var taskRepository = GetItemUnderTest(fakeAuthToken);
            Assert.ThrowsAsync<InvalidOperationException>(() => taskRepository.GetAllTasksAsync());
        }

        private TaskRepository GetItemUnderTest(AuthenticationToken authToken)
        {
            return GetItemUnderTest(Substitute.For<ITasksUrlFactory>(), Substitute.For<IRtmApiClient>(), authToken, Substitute.For<IRtmTaskConverter>());
        }

        private TaskRepository GetItemUnderTest(ITasksUrlFactory urlFactory, IRtmApiClient apiClient, AuthenticationToken authToken, IRtmTaskConverter taskConverter)
        {
            return new TaskRepository(urlFactory, apiClient, authToken, taskConverter);
        }
    }
}