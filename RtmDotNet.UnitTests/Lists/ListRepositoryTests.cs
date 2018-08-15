// -----------------------------------------------------------------------
// <copyright file="ListRepositoryTests.cs" author="Aaron Morris">
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
            var expectedLists = new List<RtmList> {new RtmList{ Name = "My Fake List"}};

            var fakeAuthToken = new AuthenticationToken{ Id = "My Fake Token ID", Permissions = permissionLevel};
            const string fakeListsUrl = "My Fake URL";

            var fakeUrlFactory = Substitute.For<IListsUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthToken.Id).Returns(fakeListsUrl);

            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeListsUrl).Returns(Task.FromResult(new GetListResponseData{ Lists = new GetListResponseData.ListOfLists{Lists = expectedLists}}));

            // Execute
            var listRepository = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeAuthToken);
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
            return GetItemUnderTest(Substitute.For<IListsUrlFactory>(), Substitute.For<IRtmApiClient>(), authToken);
        }

        private ListRepository GetItemUnderTest(IListsUrlFactory urlFactory, IRtmApiClient apiClient, AuthenticationToken authToken)
        {
            return new ListRepository(urlFactory, apiClient, authToken);
        }
    }
}