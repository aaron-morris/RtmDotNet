// -----------------------------------------------------------------------
// <copyright file="UrlBuilderFactoryTests.cs" author="Aaron Morris">
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

using NUnit.Framework;
using RtmDotNet.Http;
using RtmDotNet.Http.Api.Auth;
using RtmDotNet.Http.Auth;

namespace RtmDotNet.UnitTests.Http
{
    [TestFixture]
    public class UrlBuilderFactoryTests
    {
        [Test]
        public void CreateCheckTokenUrlBuilder_ReturnsInitializedGetFrobUrlBuilder()
        {
            // Execute
            var factory = GetItemUnderTest();
            var actual = factory.CreateCheckTokenUrlBuilder(string.Empty);

            // Verify
            Assert.IsInstanceOf<CheckTokenUrlBuilder>(actual);
        }

        [Test]
        public void CreateGetFrobUrlBuilder_ReturnsInitializedGetFrobUrlBuilder()
        {
            // Execute
            var factory = GetItemUnderTest();
            var actual = factory.CreateGetFrobUrlBuilder();

            // Verify
            Assert.IsInstanceOf<GetFrobUrlBuilder>(actual);
        }

        [Test]
        public void CreateGetTokenUrlBuilder_ReturnsInitializedGetTokenUrlBuilder()
        {
            // Execute
            var factory = GetItemUnderTest();
            var actual = factory.CreateGetTokenUrlBuilder(string.Empty);

            // Verify
            Assert.IsInstanceOf<GetTokenUrlBuilder>(actual);
        }

        [Test]
        public void CreateAuthUrlBuilder_WithoutFrobParam_ReturnsInitializedAuthBuilder()
        {
            // Setup
            const string expectedPermLevel = "My_Fake_Perm_Level";

            // Execute
            var factory = GetItemUnderTest();
            var actual = factory.CreateAuthUrlBuilder(expectedPermLevel);

            // Verify
            Assert.IsInstanceOf<AuthUrlBuilder>(actual);
            Assert.AreEqual(expectedPermLevel, actual.Parameters["perms"]);
            Assert.IsFalse(actual.Parameters.ContainsKey("frob"));
        }

        [Test]
        public void CreateAuthUrlBuilder_WithFrobParam_ReturnsInitializedAuthBuilder()
        {
            // Setup
            const string expectedPermLevel = "My_Fake_Perm_Level";
            const string expectedFrob = "My_Fake_Frob";

            // Execute
            var factory = GetItemUnderTest();
            var actual = factory.CreateAuthUrlBuilder(expectedPermLevel, expectedFrob);

            // Verify
            Assert.IsInstanceOf<AuthUrlBuilder>(actual);
            Assert.AreEqual(expectedPermLevel, actual.Parameters["perms"]);
            Assert.AreEqual(expectedFrob, actual.Parameters["frob"]);
        }

        private UrlBuilderFactory GetItemUnderTest()
        {
            return new UrlBuilderFactory(null, null);
        }
    }
}