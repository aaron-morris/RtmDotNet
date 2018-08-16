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
using NUnit.Framework;
using RtmDotNet.Auth;
using RtmDotNet.Users;

namespace RtmDotNet.UnitTests.Users
{
    [TestFixture]
    public class RtmUserTests
    {
        private const string ExpectedFullName = "Jane Doe";
        private const string ExpectedUserName = "jdoe";
        private const string ExpectedUserId = "123456";
        private const string ExpectedPermissionName = "delete";
        private const PermissionLevel ExpectedPermissionLevel = PermissionLevel.Delete;
        private const string ExpectedToken = "My Fake Token";

        private readonly string _expectedJson =
$@"{{
  ""user_id"": ""{ExpectedUserId}"",
  ""user_name"": ""{ExpectedUserName}"",
  ""full_name"": ""{ExpectedFullName}"",
  ""token"": {{
    ""id"": ""{ExpectedToken}"",
    ""permissions"": ""{ExpectedPermissionName}""
  }}
}}";

        [Test]
        public void ToJson_RtmUser_ConvertsToJson()
        {
            var user = GetItemUnderTest();
            var actual = user.ToJson();
            Assert.AreEqual(_expectedJson, actual);
        }

        private RtmUser GetItemUnderTest()
        {
            return new RtmUser
            {
                FullName = ExpectedFullName,
                UserId = ExpectedUserId,
                UserName = ExpectedUserName,
                Token = new AuthenticationToken
                {
                    Id = ExpectedToken,
                    Permissions = ExpectedPermissionLevel
                }
            };
        }
    }
}