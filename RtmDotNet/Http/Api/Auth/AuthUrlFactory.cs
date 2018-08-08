// -----------------------------------------------------------------------
// <copyright file="AuthUrlFactory.cs" author="Aaron Morris">
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

using RtmDotNet.Auth;

namespace RtmDotNet.Http.Api.Auth
{
    public class AuthUrlFactory : IAuthUrlFactory
    {
        private readonly IAuthUrlBuilderFactory _urlBuilderFactory;

        private readonly IPermissionLevelConverter _permissionLevelConverter;

        public AuthUrlFactory(IAuthUrlBuilderFactory urlBuilderFactory, IPermissionLevelConverter permissionLevelConverter)
        {
            _urlBuilderFactory = urlBuilderFactory;
            _permissionLevelConverter = permissionLevelConverter;
        }

        public string CreateCheckTokenUrl(string authToken)
        {
            var checkTokenUrlBulider = _urlBuilderFactory.CreateCheckTokenUrlBuilder(authToken);
            return checkTokenUrlBulider.BuildUrl();
        }

        public string CreateGetFrobUrl()
        {
            var getFrobUrlBuilder = _urlBuilderFactory.CreateGetFrobUrlBuilder();
            return getFrobUrlBuilder.BuildUrl();
        }

        public string CreateGetTokenUrl(string frob)
        {
            var getTokenUrlBuilder = _urlBuilderFactory.CreateGetTokenUrlBuilder(frob);
            return getTokenUrlBuilder.BuildUrl();
        }

        public string CreateAuthorizationUrl(PermissionLevel permissionLevel)
        {
            var permissionName = _permissionLevelConverter.ToString(permissionLevel);
            var authUrlBuilder = _urlBuilderFactory.CreateAuthUrlBuilder(permissionName);
            return authUrlBuilder.BuildUrl();
        }

        public string CreateAuthorizationUrl(PermissionLevel permissionLevel, string frob)
        {
            var permissionName = _permissionLevelConverter.ToString(permissionLevel);
            var authUrlBuilder = _urlBuilderFactory.CreateAuthUrlBuilder(permissionName, frob);
            return authUrlBuilder.BuildUrl();
        }
    }
}