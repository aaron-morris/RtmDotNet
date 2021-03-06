﻿// -----------------------------------------------------------------------
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
using RtmDotNet.Http;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Auth;
using RtmDotNet.Users;

namespace RtmDotNet.Auth
{
    public class AuthFactory : IAuthFactory
    {
        private readonly string _apiKey;
        private readonly string _sharedSecret;

        public AuthFactory(string apiKey, string sharedSecret)
        {
            _apiKey = apiKey;
            _sharedSecret = sharedSecret;
        }

        public IDesktopAuthenticator CreateDesktopAuthenticator()
        {
            var dataHasher = new Md5DataHasher();
            var signatureGenerator = new ApiSignatureGenerator(dataHasher, _sharedSecret);
            var urlBuilderFactory = new AuthUrlBuilderFactory(_apiKey, signatureGenerator);
            var permissionLevelConverter = new PermissionLevelConverter();
            var urlFactory = new AuthUrlFactory(urlBuilderFactory, permissionLevelConverter);
            var httpClient = new RtmHttpClient();
            var apiClient = new ApiClient(httpClient);
            var userFactory = new UserFactory();

            return new DesktopAuthenticator(urlFactory, apiClient, userFactory);
        }

        public ITokenVerifier CreateTokenVerifier()
        {
            var dataHasher = new Md5DataHasher();
            var signatureGenerator = new ApiSignatureGenerator(dataHasher, _sharedSecret);
            var urlBuilderFactory = new AuthUrlBuilderFactory(_apiKey, signatureGenerator);
            var permissionLevelConverter = new PermissionLevelConverter();
            var urlFactory = new AuthUrlFactory(urlBuilderFactory, permissionLevelConverter);
            var httpClient = new RtmHttpClient();
            var apiClient = new ApiClient(httpClient);

            return new TokenVerifier(urlFactory, apiClient);
        }
    }
}