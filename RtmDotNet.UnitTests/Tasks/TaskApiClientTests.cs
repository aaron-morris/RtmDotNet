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
    public class TaskApiClientTests
    {
        [Test]
        public async Task GetAllTasksAsync_DefaultOperation_IncludesIncompleteFilter()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeTasksUrl = "My Fake URL";

            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, filter: "status:incomplete").Returns(fakeTasksUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeTasksUrl).Returns(Task.FromResult(fakeResponseData));

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
            var actual = await taskRepository.GetAllTasksAsync().ConfigureAwait(false);

            // Verify
            Assert.AreSame(fakeResponseData, actual);
        }

        [Test]
        public async Task GetAllTasksAsync_ExplicitExcludeOption_IncludesIncompleteFilter()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeTasksUrl = "My Fake URL";

            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, filter: "status:incomplete").Returns(fakeTasksUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeTasksUrl).Returns(Task.FromResult(fakeResponseData));

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
            // ReSharper disable once RedundantArgumentDefaultValue
            var actual = await taskRepository.GetAllTasksAsync(false).ConfigureAwait(false);

            // Verify
            Assert.AreSame(fakeResponseData, actual);
        }

        [Test]
        public async Task GetAllTasksAsync_IncludeCompletedTasks_ExcludesIncompleteFilter()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeTasksUrl = "My Fake URL";

            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id).Returns(fakeTasksUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeTasksUrl).Returns(Task.FromResult(fakeResponseData));

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
            var actual = await taskRepository.GetAllTasksAsync(true).ConfigureAwait(false);

            // Verify
            Assert.AreSame(fakeResponseData, actual);
        }

        [TestCase(PermissionLevel.Read)]
        [TestCase(PermissionLevel.Write)]
        [TestCase(PermissionLevel.Delete)]
        public async Task GetAllTasksAsync_SufficientPermissions_NoPermissionException(PermissionLevel permissionLevel)
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = permissionLevel };
            
            // Execute
            var taskRepository = GetItemUnderTest(fakeAuthToken);
            await taskRepository.GetAllTasksAsync().ConfigureAwait(false);

            // Verify
            Assert.Pass("No permission exception was thrown.");
        }

        [Test]
        public void GetAllTasksAsync_InsufficientPermissions_ThrowsInvalidOperationError()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Undefined };

            // Execute
            var taskRepository = GetItemUnderTest(fakeAuthToken);
            Assert.ThrowsAsync<InvalidOperationException>(() => taskRepository.GetAllTasksAsync());
        }

        [Test]
        public async Task GetAllTasksAsync_DefaultOperationWithLastSync_IncludesIncompleteFilter()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeTasksUrl = "My Fake URL";

            var lastSync = DateTime.Parse("2018-01-02");
            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, lastSync, filter: "status:incomplete").Returns(fakeTasksUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeTasksUrl).Returns(Task.FromResult(fakeResponseData));

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
            var actual = await taskRepository.GetAllTasksAsync(lastSync).ConfigureAwait(false);

            // Verify
            Assert.AreSame(fakeResponseData, actual);
        }

        [Test]
        public async Task GetAllTasksAsync_ExplicitExcludeOptionWithLastSync_IncludesIncompleteFilter()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeTasksUrl = "My Fake URL";

            var lastSync = DateTime.Parse("2018-01-02");
            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, lastSync, filter: "status:incomplete").Returns(fakeTasksUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeTasksUrl).Returns(Task.FromResult(fakeResponseData));

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
            // ReSharper disable once RedundantArgumentDefaultValue
            var actual = await taskRepository.GetAllTasksAsync(lastSync, false).ConfigureAwait(false);

            // Verify
            Assert.AreSame(fakeResponseData, actual);
        }

        [Test]
        public async Task GetAllTasksAsync_IncludeCompletedTasksWithLastSync_ExcludesIncompleteFilter()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeTasksUrl = "My Fake URL";

            var lastSync = DateTime.Parse("2018-01-02");
            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, lastSync).Returns(fakeTasksUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeTasksUrl).Returns(Task.FromResult(fakeResponseData));

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
            var actual = await taskRepository.GetAllTasksAsync(lastSync, true).ConfigureAwait(false);

            // Verify
            Assert.AreSame(fakeResponseData, actual);
        }

        [TestCase(PermissionLevel.Read)]
        [TestCase(PermissionLevel.Write)]
        [TestCase(PermissionLevel.Delete)]
        public async Task GetAllTasksAsync_WithLastSyncAndSufficientPermissions_NoPermissionException(PermissionLevel permissionLevel)
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = permissionLevel };
            var lastSync = DateTime.Parse("2018-01-02");

            // Execute
            var taskRepository = GetItemUnderTest(fakeAuthToken);
            await taskRepository.GetAllTasksAsync(lastSync).ConfigureAwait(false);

            // Verify
            Assert.Pass("No permission exception was thrown.");
        }

        [Test]
        public void GetAllTasksAsync_WithLastSyncAndInsufficientPermissions_ThrowsInvalidOperationError()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Undefined };
            var lastSync = DateTime.Parse("2018-01-02");

            // Execute
            var taskRepository = GetItemUnderTest(fakeAuthToken);
            Assert.ThrowsAsync<InvalidOperationException>(() => taskRepository.GetAllTasksAsync(lastSync));
        }

        [Test]
        public async Task GetTasksByListIdAsync_DefaultOperation_IncludesIncompleteFilter()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeTasksUrl = "My Fake URL";

            var fakeListId = "My Fake List ID";
            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, listId:fakeListId, filter: "status:incomplete").Returns(fakeTasksUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeTasksUrl).Returns(Task.FromResult(fakeResponseData));

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
            var actual = await taskRepository.GetTasksByListIdAsync(fakeListId).ConfigureAwait(false);

            // Verify
            Assert.AreSame(fakeResponseData, actual);
        }

        [Test]
        public async Task GetTasksByListIdAsync_ExplicitExcludeOption_IncludesIncompleteFilter()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeTasksUrl = "My Fake URL";

            var fakeListId = "My Fake List ID";
            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, listId: fakeListId, filter: "status:incomplete").Returns(fakeTasksUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeTasksUrl).Returns(Task.FromResult(fakeResponseData));

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
            // ReSharper disable once RedundantArgumentDefaultValue
            var actual = await taskRepository.GetTasksByListIdAsync(fakeListId, includeCompletedTasks: false).ConfigureAwait(false);

            // Verify
            Assert.AreSame(fakeResponseData, actual);
        }

        [Test]
        public async Task GetTasksByListIdAsync_IncludeCompletedTasks_ExcludesIncompleteFilter()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Read };
            const string fakeTasksUrl = "My Fake URL";

            var fakeListId = "My Fake List ID";
            var fakeUrlFactory = Substitute.For<ITasksUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id, listId: fakeListId).Returns(fakeTasksUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeTasksUrl).Returns(Task.FromResult(fakeResponseData));

            // Execute
            var taskRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
            var actual = await taskRepository.GetTasksByListIdAsync(fakeListId, includeCompletedTasks: true).ConfigureAwait(false);

            // Verify
            Assert.AreSame(fakeResponseData, actual);
        }

        [TestCase(PermissionLevel.Read)]
        [TestCase(PermissionLevel.Write)]
        [TestCase(PermissionLevel.Delete)]
        public async Task GetTasksByListIdAsync_SufficientPermissions_NoPermissionException(PermissionLevel permissionLevel)
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = permissionLevel };

            // Execute
            var taskRepository = GetItemUnderTest(fakeAuthToken);
            await taskRepository.GetTasksByListIdAsync("Fake List ID").ConfigureAwait(false);

            // Verify
            Assert.Pass("No permission exception was thrown.");
        }

        [Test]
        public void GetTasksByListIdAsync_InsufficientPermissions_ThrowsInvalidOperationError()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Undefined };

            // Execute
            var taskRepository = GetItemUnderTest(fakeAuthToken);
            Assert.ThrowsAsync<InvalidOperationException>(() => taskRepository.GetTasksByListIdAsync("Fake List ID"));
        }

        private TaskApiClient GetItemUnderTest(AuthenticationToken authToken)
        {
            return GetItemUnderTest(Substitute.For<ITasksUrlFactory>(), Substitute.For<IApiClient>(), authToken);
        }

        private TaskApiClient GetItemUnderTest(ITasksUrlFactory urlFactory, IApiClient apiClient, AuthenticationToken authToken)
        {
            return new TaskApiClient(urlFactory, apiClient, authToken);
        }
    }
}