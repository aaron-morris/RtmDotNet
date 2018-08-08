// -----------------------------------------------------------------------
// <copyright file="RtmUserFactoryTests.cs" author="Aaron Morris">
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
using NUnit.Framework;
using RtmDotNet.Auth;
using RtmDotNet.Http.Api.Auth;
using RtmDotNet.Users;

namespace RtmDotNet.UnitTests.Users
{
    [TestFixture]
    public class RtmUserFactoryTests
    {
        private const string ExpectedFullName = "Jane Doe";
        private const string ExpectedUserName = "jdoe";
        private const string ExpectedUserId = "123456";
        private const string ExpectedPermissionName = "delete";
        private const PermissionLevel ExpectedPermissionLevel = PermissionLevel.Delete;
        private const string ExpectedToken = "My Fake Token";

        [Test]
        public void CreateNewUser_ValidAuthToken_InitsUserFromAuthTokenData()
        {
            // Setup
            var fakeAuthToken = new GetTokenResponseData.AuthorizationTokenData
            {
                User = new GetTokenResponseData.AuthorizationTokenData.UserInfo
                {
                    FullName = ExpectedFullName,
                    UserId = ExpectedUserId,
                    UserName = ExpectedUserName
                },
                Permissions = ExpectedPermissionLevel,
                Token = ExpectedToken
            };

            // Execute
            var factory = GetItemUnderTest();
            var actual = factory.CreateNewUser(fakeAuthToken);

            // Verify
            Assert.AreEqual(ExpectedFullName, actual.FullName);
            Assert.AreEqual(ExpectedUserName, actual.UserName);
            Assert.AreEqual(ExpectedUserId, actual.UserId);
            Assert.AreEqual(ExpectedPermissionLevel, actual.Token.Permissions);
            Assert.AreEqual(ExpectedToken, actual.Token.Id);
        }

        [Test]
        public void CreateNewUser_NullAuthToken_ThrowsArgumentNullException()
        {
            var factory = GetItemUnderTest();
            Assert.Throws<ArgumentNullException>(() => factory.CreateNewUser(null));
        }

        [Test]
        public void LoadFromJson_ValidJson_InitsUserFromJson()
        {
            // Setup
            var json = 
$@"{{
    ""user_id"": ""{ExpectedUserId}"",
    ""user_name"": ""{ExpectedUserName}"",
    ""full_name"": ""{ExpectedFullName}"",
    ""token"": {{
        ""id"": ""{ExpectedToken}"",
        ""permissions"": ""{ExpectedPermissionName}""
    }}
}}";

            // Execute
            var factory = GetItemUnderTest();
            var actual = factory.LoadFromJson(json);

            // Verify
            Assert.AreEqual(ExpectedFullName, actual.FullName);
            Assert.AreEqual(ExpectedUserName, actual.UserName);
            Assert.AreEqual(ExpectedUserId, actual.UserId);
            Assert.AreEqual(ExpectedPermissionLevel, actual.Token.Permissions);
            Assert.AreEqual(ExpectedToken, actual.Token.Id);
        }

        [TestCase(null)]
        [TestCase("")]
        public void LoadFromJson_NullJson_ThrowsArgumentNullException(string json)
        {
            var factory = GetItemUnderTest();
            Assert.Throws<ArgumentNullException>(() => factory.LoadFromJson(json));
        }

        private RtmUserFactory GetItemUnderTest()
        {
            return new RtmUserFactory();
        }
    }
}