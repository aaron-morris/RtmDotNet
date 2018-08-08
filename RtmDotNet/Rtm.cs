// -----------------------------------------------------------------------
// <copyright file="Rtm.cs" author="Aaron Morris">
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
using RtmDotNet.Auth;
using RtmDotNet.Http;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Lists;
using RtmDotNet.Lists;
using RtmDotNet.Users;

namespace RtmDotNet
{
    public static class Rtm
    {
        public static string ApiKey { get; set; }

        public static string SharedSecret { get; set; }

        public static void Init(string apiKey, string sharedSecret)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            if (string.IsNullOrEmpty(sharedSecret))
            {
                throw new ArgumentNullException(nameof(sharedSecret));
            }

            ApiKey = apiKey;
            SharedSecret = sharedSecret;
        }

        public static IAuthFactory GetAuthFactory()
        {
            EnforceInitialization();
            return new AuthFactory(ApiKey, SharedSecret);
        }

        public static IListRepository GetListRepository(AuthorizationToken authToken)
        {
            if (authToken == null)
            {
                throw new ArgumentNullException(nameof(authToken));
            }

            EnforceInitialization();

            var dataHasher = new Md5DataHasher();
            var signatureGenerator = new ApiSignatureGenerator(dataHasher, SharedSecret);
            var urlBuilderFactory = new ListsUrlBuilderFactory(ApiKey, signatureGenerator);
            var urlFactory = new ListsUrlFactory(urlBuilderFactory);
            var httpClient = new RtmHttpClient();
            var apiClient = new RtmApiClient(httpClient);

            return new ListRepository(urlFactory, apiClient, authToken);
        }

        public static IRtmUserFactory GetUserFactory()
        {
            return new RtmUserFactory();
        }

        private static void EnforceInitialization()
        {
            if (string.IsNullOrEmpty(ApiKey) || string.IsNullOrEmpty(SharedSecret))
            {
                throw new InvalidOperationException("You must first initialize the API with an API Key and Shared Secret before accessing its properties.");
            }
        }
    }
}