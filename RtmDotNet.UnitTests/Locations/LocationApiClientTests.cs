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
using RtmDotNet.Auth;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Locations;
using RtmDotNet.Locations;

namespace RtmDotNet.UnitTests.Locations
{
    using NUnit.Framework;

    [TestFixture]
    public class LocationApiClientTests
    {
        [Test]
        public async Task GetAllLocationsAsync_SufficientPermissions_UpdatesCacheAndReturnsList()
        {
            // Setup
            var expected = new List<IRtmLocation> { Substitute.For<IRtmLocation>() };

            const string fakeAuthTokenId = "My Fake Auth Token";
            var fakeAuthToken = new AuthenticationToken {Id = fakeAuthTokenId, Permissions = PermissionLevel.Read};

            const string fakeUrl = "My Fake Url";
            var fakeUrlFactory = Substitute.For<ILocationsUrlFactory>();
            fakeUrlFactory.CreateGetListsUrl(fakeAuthTokenId).Returns(fakeUrl);

            var fakeResponseData = new GetListResponseData();
            var fakeApiClient = Substitute.For<IApiClient>();
            fakeApiClient.GetAsync<GetListResponseData>(fakeUrl).Returns(fakeResponseData);
            
            var fakeResponseParser = Substitute.For<IResponseParser>();
            fakeResponseParser.GetLocations(fakeResponseData).Returns(expected);

            // Execute
            var apiClient = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeResponseParser, fakeAuthToken);
            var actual = await apiClient.GetAllLocationsAsync().ConfigureAwait(false);

            // Verify
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void GetAllLocationsAsync_InsufficientPermissions_ThrowsInvalidOperationExcetpion()
        {
            const string fakeAuthTokenId = "My Fake Auth Token";
            var fakeAuthToken = new AuthenticationToken { Id = fakeAuthTokenId, Permissions = PermissionLevel.Undefined };

            var apiClient = GetItemUnderTest(fakeAuthToken);
            Assert.ThrowsAsync<InvalidOperationException>(() => apiClient.GetAllLocationsAsync());
        }

        private LocationApiClient GetItemUnderTest(AuthenticationToken authToken)
        {
            return GetItemUnderTest(Substitute.For<ILocationsUrlFactory>(), Substitute.For<IApiClient>(), Substitute.For<IResponseParser>(), authToken);
        }

        private LocationApiClient GetItemUnderTest(ILocationsUrlFactory urlFactory, IApiClient apiClient, IResponseParser responseParser, AuthenticationToken authToken)
        {
            return new LocationApiClient(urlFactory, apiClient, authToken, responseParser);
        }
    }
}