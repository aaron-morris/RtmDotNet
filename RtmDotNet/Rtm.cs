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
using System;
using RtmDotNet.Auth;
using RtmDotNet.Http;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Lists;
using RtmDotNet.Http.Api.Locations;
using RtmDotNet.Http.Api.Tasks;
using RtmDotNet.Lists;
using RtmDotNet.Locations;
using RtmDotNet.Tasks;
using RtmDotNet.Users;

namespace RtmDotNet
{
    public static class Rtm
    {
        private static readonly IApiClient ApiClient = new ApiClient(new RtmHttpClient());

        private static IApiSignatureGenerator _signatureGenerator;

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

            _signatureGenerator = new ApiSignatureGenerator(new Md5DataHasher(), SharedSecret);
        }

        public static IAuthFactory GetAuthFactory()
        {
            EnforceInitialization();
            return new AuthFactory(ApiKey, SharedSecret);
        }

        public static IListRepository GetListRepository(AuthenticationToken authToken)
        {
            if (authToken == null)
            {
                throw new ArgumentNullException(nameof(authToken));
            }

            EnforceInitialization();

            var urlBuilderFactory = new ListsUrlBuilderFactory(ApiKey, _signatureGenerator);
            var urlFactory = new ListsUrlFactory(urlBuilderFactory);
            var taskRepository = GetTaskRepository(authToken);
            var responseParser = new Http.Api.Lists.ResponseParser(taskRepository);

            return new ListRepository(urlFactory, ApiClient, responseParser, authToken);
        }

        public static ILocationRepository GetLocationRepository(AuthenticationToken authToken)
        {
            if (authToken == null)
            {
                throw new ArgumentNullException(nameof(authToken));
            }

            EnforceInitialization();

            var urlBuilderFactory = new LocationsUrlBuilderFactory(ApiKey, _signatureGenerator);
            var urlFactory = new LocationsUrlFactory(urlBuilderFactory);
            var responseParser = new Http.Api.Locations.ResponseParser();
            var locationApiClient = new LocationApiClient(urlFactory, ApiClient, authToken, responseParser);
            var locationCache = new InMemoryLocationCache();

            return new LocationRepository(locationApiClient, locationCache);
        }

        public static ITaskRepository GetTaskRepository(AuthenticationToken authToken)
        {
            if (authToken == null)
            {
                throw new ArgumentNullException(nameof(authToken));
            }

            EnforceInitialization();

            var urlBuilderFactory = new TasksUrlBuilderFactory(ApiKey, _signatureGenerator);
            var urlFactory = new TasksUrlFactory(urlBuilderFactory);
            var taskApiClient = new TaskApiClient(urlFactory, ApiClient, authToken);
            var responseParser = new Http.Api.Tasks.ResponseParser(GetLocationRepository(authToken));
            var taskTreeBuilder = new TaskTreeBuilder();
            var taskCache = new InMemoryTaskCache(taskTreeBuilder);
            var syncTracker = new InMemorySyncTracker();
            return new TaskRepository(taskApiClient, responseParser, taskCache, syncTracker);
        }

        public static IUserFactory GetUserFactory()
        {
            return new UserFactory();
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