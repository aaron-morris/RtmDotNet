// -----------------------------------------------------------------------
// <copyright file="AuthUrlFactoryTests.cs" author="Aaron Morris">
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

using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Auth;
using RtmDotNet.Http;
using RtmDotNet.Http.Api.Auth;

namespace RtmDotNet.UnitTests.Http.Api.Auth
{
    [TestFixture]
    public class AuthUrlFactoryTests
    {
        [Test]
        public void CreateCheckTokenUrl_ReturnsCheckTokenUrl()
        {
            // Setup
            const string expectedUrl = "My_Fake_Url";
            const string fakeToken = "My Fake Token";

            var fakeUrlBuilder = Substitute.For<IUrlBuilder>();
            fakeUrlBuilder.BuildUrl().Returns(expectedUrl);

            var fakeUrlBuilderFactory = Substitute.For<IAuthUrlBuilderFactory>();
            fakeUrlBuilderFactory.CreateCheckTokenUrlBuilder(fakeToken).Returns(fakeUrlBuilder);

            // Execute
            var factory = GetItemUnderTest(fakeUrlBuilderFactory, null);
            var actual = factory.CreateCheckTokenUrl(fakeToken);

            // Verify
            Assert.AreEqual(expectedUrl, actual);
        }

        [Test]
        public void CreateGetFrobUrl_ReturnsGetFrobUrl()
        {
            // Setup
            const string expectedUrl = "My_Fake_Url";

            var fakeUrlBuilder = Substitute.For<IUrlBuilder>();
            fakeUrlBuilder.BuildUrl().Returns(expectedUrl);

            var fakeUrlBuilderFactory = Substitute.For<IAuthUrlBuilderFactory>();
            fakeUrlBuilderFactory.CreateGetFrobUrlBuilder().Returns(fakeUrlBuilder);

            // Execute
            var factory = GetItemUnderTest(fakeUrlBuilderFactory, null);
            var actual = factory.CreateGetFrobUrl();

            // Verify
            Assert.AreEqual(expectedUrl, actual);
        }

        [Test]
        public void CreateGetTokenUrl_ReturnsGetTokenUrl()
        {
            // Setup
            const string expectedUrl = "My_Fake_Url";
            const string fakeFrob = "My Fake Frob";

            var fakeUrlBuilder = Substitute.For<IUrlBuilder>();
            fakeUrlBuilder.BuildUrl().Returns(expectedUrl);

            var fakeUrlBuilderFactory = Substitute.For<IAuthUrlBuilderFactory>();
            fakeUrlBuilderFactory.CreateGetTokenUrlBuilder(fakeFrob).Returns(fakeUrlBuilder);

            // Execute
            var factory = GetItemUnderTest(fakeUrlBuilderFactory, null);
            var actual = factory.CreateGetTokenUrl(fakeFrob);

            // Verify
            Assert.AreEqual(expectedUrl, actual);
        }

        [Test]
        public void CreateAuthenticationUrl_WithoutFrobParam_ReturnsAuthenticationUrl()
        {
            // Setup
            const string expectedUrl = "My_Fake_Url";
            const string fakePermLevelName = "My_Fake_Perm_Level";

            var fakePermissionConverter = Substitute.For<IPermissionLevelConverter>();
            var fakePermissionLevel = (PermissionLevel) int.MaxValue;
            fakePermissionConverter.ToString(fakePermissionLevel).Returns(fakePermLevelName);

            var fakeUrlBuilder = Substitute.For<IUrlBuilder>();
            fakeUrlBuilder.BuildUrl().Returns(expectedUrl);

            var fakeUrlBuilderFactory = Substitute.For<IAuthUrlBuilderFactory>();
            fakeUrlBuilderFactory.CreateAuthUrlBuilder(fakePermLevelName).Returns(fakeUrlBuilder);

            // Execute
            var factory = GetItemUnderTest(fakeUrlBuilderFactory, fakePermissionConverter);
            var actual = factory.CreateAuthenticationUrl(fakePermissionLevel);

            // Verify
            Assert.AreEqual(expectedUrl, actual);
        }

        [Test]
        public void CreateAuthenticationUrl_WithFrobParam_ReturnsAuthenticationUrl()
        {
            // Setup
            const string expectedUrl = "My_Fake_Url";
            const string fakePermLevelName = "My_Fake_Perm_Level";
            const string fakeFrob = "My_Fake_Frob";

            var fakePermissionConverter = Substitute.For<IPermissionLevelConverter>();
            var fakePermissionLevel = (PermissionLevel)int.MaxValue;
            fakePermissionConverter.ToString(fakePermissionLevel).Returns(fakePermLevelName);

            var fakeUrlBuilder = Substitute.For<IUrlBuilder>();
            fakeUrlBuilder.BuildUrl().Returns(expectedUrl);

            var fakeUrlBuilderFactory = Substitute.For<IAuthUrlBuilderFactory>();
            fakeUrlBuilderFactory.CreateAuthUrlBuilder(fakePermLevelName, fakeFrob).Returns(fakeUrlBuilder);

            // Execute
            var factory = GetItemUnderTest(fakeUrlBuilderFactory, fakePermissionConverter);
            var actual = factory.CreateAuthenticationUrl(fakePermissionLevel, fakeFrob);

            // Verify
            Assert.AreEqual(expectedUrl, actual);
        }

        private AuthUrlFactory GetItemUnderTest(IAuthUrlBuilderFactory urlBuilderFactory, IPermissionLevelConverter permissionLevelConverter)
        {
            return new AuthUrlFactory(urlBuilderFactory, permissionLevelConverter);
        }
    }
}