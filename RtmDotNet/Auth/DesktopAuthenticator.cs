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
using System;
using System.Threading.Tasks;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Auth;
using RtmDotNet.Users;

namespace RtmDotNet.Auth
{
    public class DesktopAuthenticator : IDesktopAuthenticator
    {
        private readonly IAuthUrlFactory _urlFactory;
        private readonly IRtmApiClient _apiClient;
        private readonly IRtmUserFactory _userFactory;

        private string _frob;

        public DesktopAuthenticator(IAuthUrlFactory urlFactory, IRtmApiClient apiClient, IRtmUserFactory userFactory)
        {
            _urlFactory = urlFactory;
            _apiClient = apiClient;
            _userFactory = userFactory;
        }

        public async Task<string> GetAuthenticationUrlAsync(PermissionLevel permissionLevel)
        {
            _frob = await GetNewFrobAsync().ConfigureAwait(false);
            return _urlFactory.CreateAuthenticationUrl(permissionLevel, _frob);
        }

        public async Task<IRtmUser> GetAutheticatedUserAsync()
        {
            if (string.IsNullOrEmpty(_frob))
            {
                throw new InvalidOperationException("You must generate an authentication URL before requesting a token.");
            }

            var getTokenUrl = _urlFactory.CreateGetTokenUrl(_frob);
            var response = await _apiClient.GetAsync<GetTokenResponseData>(getTokenUrl).ConfigureAwait(false);
            var token = response.AuthenticationToken;
            return _userFactory.CreateNewUser(token);
        }

        private async Task<string> GetNewFrobAsync()
        {
            var getFrobUrl = _urlFactory.CreateGetFrobUrl();
            var response = await _apiClient.GetAsync<GetFrobResponseData>(getFrobUrl).ConfigureAwait(false);
            return response.Frob;
        }
    }
}