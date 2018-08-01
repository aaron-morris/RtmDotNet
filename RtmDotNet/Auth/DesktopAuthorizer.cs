// -----------------------------------------------------------------------
// <copyright file="DesktopAuthorizer.cs" author="Aaron Morris">
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
using RtmDotNet.Http;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Models;
using RtmDotNet.Users;

namespace RtmDotNet.Auth
{
    public class DesktopAuthorizer : IDesktopAuthorizer
    {
        private readonly IUrlFactory _urlFactory;
        private readonly IRtmApiClient _apiClient;
        private readonly IRtmUserFactory _userFactory;

        private string _frob;

        public DesktopAuthorizer(IUrlFactory urlFactory, IRtmApiClient apiClient, IRtmUserFactory userFactory)
        {
            _urlFactory = urlFactory;
            _apiClient = apiClient;
            _userFactory = userFactory;
        }

        public async Task<string> GetAuthorizationUrlAsync(PermissionLevel permissionLevel)
        {
            _frob = await GetNewFrobAsync();
            return _urlFactory.CreateAuthorizationUrl(permissionLevel, _frob);
        }

        public async Task<IRtmUser> GetAuthorizedUserAsync()
        {
            if (string.IsNullOrEmpty(_frob))
            {
                throw new InvalidOperationException("You must generate an authorization URL before requesting a token.");
            }

            var getTokenUrl = _urlFactory.CreateGetTokenUrl(_frob);
            var response = await _apiClient.GetAsync<GetTokenResponseData>(getTokenUrl);
            var token = response.AuthorizationToken;
            return _userFactory.CreateNewUser(token);
        }

        private async Task<string> GetNewFrobAsync()
        {
            var getFrobUrl = _urlFactory.CreateGetFrobUrl();
            var response = await _apiClient.GetAsync<GetFrobResponseData>(getFrobUrl);
            return response.Frob;
        }
    }
}