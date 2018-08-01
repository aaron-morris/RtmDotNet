// -----------------------------------------------------------------------
// <copyright file="DesktopAuthorizerTests.cs" author="Aaron Morris">
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
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Auth;
using RtmDotNet.Http;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Models;
using RtmDotNet.Users;

namespace RtmDotNet.UnitTests.Auth
{
    [TestFixture]
    public class DesktopAuthorizerTests
    {
        [Test]
        public async Task GetAuthorizationUrlAsync_GetsAuthorizationUrl()
        {
            // Setup
            const string expectedAuthUrl = "My_Fake_Url";
            const string fakeFrobUrl = "My_Fake_Frob_Url";
            const string fakeFrob = "My_Fake_Frob";
            var fakePermissionLevel = (PermissionLevel)int.MaxValue;
            
            var fakeUrlFactory = Substitute.For<IUrlFactory>();
            fakeUrlFactory.CreateGetFrobUrl().Returns(fakeFrobUrl);
            fakeUrlFactory.CreateAuthorizationUrl(fakePermissionLevel, fakeFrob).Returns(expectedAuthUrl);

            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetFrobResponseData>(fakeFrobUrl).Returns(Task.FromResult(new GetFrobResponseData { Frob = fakeFrob}));

            // Execute
            var authorizer = GetItemUnderTest(fakeUrlFactory, fakeApiClient);
            var actual = await authorizer.GetAuthorizationUrlAsync(fakePermissionLevel);

            // Verify
            Assert.AreEqual(expectedAuthUrl, actual);
        }

        [Test]
        public async Task GetAuthorizedUserAsync_AuthUrlIsInvokedFirst_GetsUser()
        {
            // Setup
            var expectedUser = Substitute.For<IRtmUser>();

            const string fakeFrobUrl = "My_Fake_Frob_Url";
            const string fakeFrob = "My Fake Frob";
            const string fakeTokenUrl = "My_Fake_Token_Url";
            var fakeAuthToken = new AuthorizationToken {Token = "My Fake Token"};
            var fakePermissionLevel = (PermissionLevel)int.MaxValue;

            var fakeUrlFactory = Substitute.For<IUrlFactory>();
            fakeUrlFactory.CreateGetFrobUrl().Returns(fakeFrobUrl);
            fakeUrlFactory.CreateGetTokenUrl(fakeFrob).Returns(fakeTokenUrl);

            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetFrobResponseData>(fakeFrobUrl).Returns(Task.FromResult(new GetFrobResponseData { Frob = fakeFrob }));
            fakeApiClient.GetAsync<GetTokenResponseData>(fakeTokenUrl).Returns(Task.FromResult(new GetTokenResponseData { AuthorizationToken = fakeAuthToken }));

            var fakeUserFactory = Substitute.For<IRtmUserFactory>();
            fakeUserFactory.CreateNewUser(fakeAuthToken).ReturnsForAnyArgs(expectedUser);

            // Execute
            var authorizer = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeUserFactory);
            await authorizer.GetAuthorizationUrlAsync(fakePermissionLevel);
            var actual = await authorizer.GetAuthorizedUserAsync();

            // Verify
            Assert.AreSame(expectedUser, actual);
        }

        [Test]
        public void GetAuthorizedUserAsync_AuthUrlIsNotInvokedFirst_ThrowsInvalidOperationException()
        {
            // Setup
            var expectedUser = Substitute.For<IRtmUser>();

            const string fakeFrobUrl = "My_Fake_Frob_Url";
            const string fakeFrob = "My Fake Frob";
            const string fakeTokenUrl = "My_Fake_Token_Url";
            var fakeAuthToken = new AuthorizationToken { Token = "My Fake Token" };
            
            var fakeUrlFactory = Substitute.For<IUrlFactory>();
            fakeUrlFactory.CreateGetFrobUrl().Returns(fakeFrobUrl);
            fakeUrlFactory.CreateGetTokenUrl(fakeFrob).Returns(fakeTokenUrl);

            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetFrobResponseData>(fakeFrobUrl).Returns(Task.FromResult(new GetFrobResponseData { Frob = fakeFrob }));
            fakeApiClient.GetAsync<GetTokenResponseData>(fakeTokenUrl).Returns(Task.FromResult(new GetTokenResponseData { AuthorizationToken = fakeAuthToken }));

            var fakeUserFactory = Substitute.For<IRtmUserFactory>();
            fakeUserFactory.CreateNewUser(fakeAuthToken).ReturnsForAnyArgs(expectedUser);

            // Execute
            var authorizer = GetItemUnderTest(fakeUrlFactory, fakeApiClient, fakeUserFactory);
            Assert.ThrowsAsync<InvalidOperationException>(() => authorizer.GetAuthorizedUserAsync());
        }

        private DesktopAuthorizer GetItemUnderTest(IUrlFactory urlFactory, IRtmApiClient apiClient)
        {
            return GetItemUnderTest(urlFactory, apiClient, Substitute.For<IRtmUserFactory>());
        }

        private DesktopAuthorizer GetItemUnderTest(IUrlFactory urlFactory, IRtmApiClient apiClient, IRtmUserFactory userFactory)
        {
            return new DesktopAuthorizer(urlFactory, apiClient, userFactory);
        }
    }
}