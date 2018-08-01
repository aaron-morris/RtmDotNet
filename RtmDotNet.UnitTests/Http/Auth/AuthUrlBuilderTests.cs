// -----------------------------------------------------------------------
// <copyright file="AuthUrlBuilderTests.cs" author="Aaron Morris">
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

using System.Collections.Generic;
using NUnit.Framework;
using RtmDotNet.Http;
using RtmDotNet.Http.Auth;

namespace RtmDotNet.UnitTests.Http.Auth
{
    [TestFixture]
    public class AuthUrlBuilderTests : SignedUrlBuilderTests
    {
        private const string FakePermissionLevel = "My_Fake_Perm_Level";

        protected override string ExpectedUrl => $"{AuthUrlBuilder.AuthUrl}api_key={FakeApiKey}&perms={FakePermissionLevel}&api_sig={FakeApiSig}";

        protected override IDictionary<string, string> ExpectedParams { get; } = new Dictionary<string, string>
        {
            { "api_key", FakeApiKey },
            { "perms", FakePermissionLevel }
        };

        [Test]
        public void Ctor_WithFrobParam_InitializesFrobParam()
        {
            const string expectedFrob = "My_Fake_Frob";

            // Execute
            var authUrlBuilder = new AuthUrlBuilder(FakeApiKey, new MockSignatureGenerator(), FakePermissionLevel, expectedFrob);

            // Verify
            Assert.IsTrue(authUrlBuilder.Parameters.ContainsKey("frob"));
            Assert.AreEqual(expectedFrob, authUrlBuilder.Parameters["frob"]);
        }

        protected override IUrlBuilder GetItemUnderTest(IApiSignatureGenerator signatureGenerator)
        {
            return new AuthUrlBuilder(FakeApiKey, signatureGenerator, FakePermissionLevel);
        }
    }
}