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
using NUnit.Framework;
using RtmDotNet.Auth;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Lists;
using RtmDotNet.Lists;

namespace RtmDotNet.UnitTests.Lists
{
    [TestFixture]
    public class ListRepositoryTests
    {
        [TestCase(PermissionLevel.Read)]
        [TestCase(PermissionLevel.Write)]
        [TestCase(PermissionLevel.Delete)]
        public async Task GetAllListsAsync_SufficientPermissions_ReturnsRtmLists(PermissionLevel permissionLevel)
        {
            // Setup
            var expectedLists = new List<IRtmList> { Substitute.For<IRtmList>()};

            var fakeAuthToken = new AuthenticationToken{ Id = "My Fake Token ID", Permissions = permissionLevel};
            const string fakeListsUrl = "My Fake URL";

            var fakeUrlFactory = Substitute.For<IListsUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id).Returns(fakeListsUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeListsUrl).Returns(Task.FromResult(fakeResponseData));

            var fakeListConverter = Substitute.For<IListConverter>();
            fakeListConverter.ConvertToLists(fakeResponseData).Returns(expectedLists);

            // Execute
            var listRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeListConverter, fakeAuthToken);
            var actual = await listRepository.GetAllListsAsync().ConfigureAwait(false);

            // Verify
            Assert.AreSame(expectedLists, actual);
        }

        [Test]
        public void GetAllListsAsync_InsufficientPermissions_ThrowsInvalidOperationError()
        {
            // Setup
            var fakeAuthToken = new AuthenticationToken { Id = "My Fake Token ID", Permissions = PermissionLevel.Undefined };

            // Execute
            var listRepository = GetItemUnderTest(fakeAuthToken);
            Assert.ThrowsAsync<InvalidOperationException>(() => listRepository.GetAllListsAsync());
        }

        private ListRepository GetItemUnderTest(AuthenticationToken authToken)
        {
            return GetItemUnderTest(Substitute.For<IListsUrlFactory>(), Substitute.For<IApiClient>(), Substitute.For<IListConverter>(), authToken);
        }

        private ListRepository GetItemUnderTest(IListsUrlFactory urlFactory, IApiClient apiClient, IListConverter listConverter, AuthenticationToken authToken)
        {
            return new ListRepository(urlFactory, apiClient, listConverter, authToken);
        }
    }
}