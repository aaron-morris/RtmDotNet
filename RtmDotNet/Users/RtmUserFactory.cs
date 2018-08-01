// -----------------------------------------------------------------------
// <copyright file="RtmUserFactory.cs" author="Aaron Morris">
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
using Newtonsoft.Json;
using RtmDotNet.Auth;

namespace RtmDotNet.Users
{
    public class RtmUserFactory : IRtmUserFactory
    {
        public IRtmUser CreateNewUser(AuthorizationToken authToken)
        {
            if (authToken == null)
            {
                throw new ArgumentNullException(nameof(authToken));
            }

            return new RtmUser
            {
                UserId = authToken.User.UserId,
                UserName = authToken.User.UserName,
                FullName = authToken.User.FullName,
                Token = authToken.Token,
                Permissions = authToken.Permissions
            };
        }

        public IRtmUser LoadFromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            return JsonConvert.DeserializeObject<RtmUser>(json);
        }
    }
}