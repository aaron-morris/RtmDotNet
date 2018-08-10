// -----------------------------------------------------------------------
// <copyright file="GetTokenResponseData.cs" author="Aaron Morris">
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

using Newtonsoft.Json;
using RtmDotNet.Auth;

namespace RtmDotNet.Http.Api.Auth
{
    public class GetTokenResponseData : RtmApiResponseData
    {
        [JsonProperty("auth")]
        public AuthenticationTokenData AuthenticationToken { get; set; }

        public class AuthenticationTokenData
        {
            [JsonProperty("token")]
            public string Token { get; set; }

            [JsonProperty("perms")]
            [JsonConverter(typeof(PermissionsJsonConverter))]
            public PermissionLevel Permissions { get; set; }

            [JsonProperty("user")]
            public UserInfo User { get; set; }

            public class UserInfo
            {
                [JsonProperty("id")]
                public string UserId { get; set; }

                [JsonProperty("username")]
                public string UserName { get; set; }

                [JsonProperty("fullname")]
                public string FullName { get; set; }
            }
        }
    }
}