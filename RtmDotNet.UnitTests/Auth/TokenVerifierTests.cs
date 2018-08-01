// -----------------------------------------------------------------------
// <copyright file="TokenVerifierTests.cs" author="Aaron Morris">
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
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using RtmDotNet.Auth;
using RtmDotNet.Http;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Models;

namespace RtmDotNet.UnitTests.Auth
{
    [TestFixture]
    public class TokenVerifierTests
    {
        [Test]
        public async Task VerifyAsync_ApiVerifies_ReturnsTrue()
        {
            // Setup
            const string fakeTokenUrl = "My_Fake_Token_Url";
            const string fakeToken = "My Fake Token";
            var expectedAuthToken = new AuthorizationToken();

            var fakeUrlFactory = Substitute.For<IUrlFactory>();
            fakeUrlFactory.CreateCheckTokenUrl(fakeToken).Returns(fakeTokenUrl);

            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetTokenResponseData>(fakeTokenUrl).Returns(Task.FromResult(new GetTokenResponseData { AuthorizationToken = expectedAuthToken }));

            // Execute
            var tokenVerifier = GetItemUnderTest(fakeUrlFactory, fakeApiClient);
            var actual = await tokenVerifier.VerifyAsync(fakeToken);

            Assert.IsTrue(actual);
        }

        [Test]
        public async Task VerifyAsync_ApiThrowsInvalidAuthTokenError_ReturnsFalse()
        {
            // Setup
            const string fakeTokenUrl = "My_Fake_Token_Url";
            const string fakeToken = "My Fake Token";
            
            var fakeUrlFactory = Substitute.For<IUrlFactory>();
            fakeUrlFactory.CreateCheckTokenUrl(fakeToken).Returns(fakeTokenUrl);

            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetTokenResponseData>(fakeTokenUrl).Throws(new RtmException(RtmErrorCodes.InvalidAuthToken, String.Empty));

            // Execute
            var tokenVerifier = GetItemUnderTest(fakeUrlFactory, fakeApiClient);
            var actual = await tokenVerifier.VerifyAsync(fakeToken);

            Assert.IsFalse(actual);
        }

        [Test]
        public void VerifyAsync_ApiThrowsOtherError_RethrowsError()
        {
            // Setup
            var expectedErrorCode = RtmErrorCodes.InvalidApiKey;
            const string expectedMessage = "Invalid API Key";

            const string fakeTokenUrl = "My_Fake_Token_Url";
            const string fakeToken = "My Fake Token";

            var fakeUrlFactory = Substitute.For<IUrlFactory>();
            fakeUrlFactory.CreateCheckTokenUrl(fakeToken).Returns(fakeTokenUrl);

            var fakeApiClient = Substitute.For<IRtmApiClient>();
            fakeApiClient.GetAsync<GetTokenResponseData>(fakeTokenUrl).Throws(new RtmException(expectedErrorCode, expectedMessage));

            // Execute
            var tokenVerifier = GetItemUnderTest(fakeUrlFactory, fakeApiClient);
            var actual = Assert.ThrowsAsync<RtmException>(() => tokenVerifier.VerifyAsync(fakeToken));

            Assert.AreEqual(expectedErrorCode, actual.ErrorCode);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        private TokenVerifier GetItemUnderTest(IUrlFactory urlFactory, IRtmApiClient apiClient)
        {
            return new TokenVerifier(urlFactory, apiClient);
        }
    }
}