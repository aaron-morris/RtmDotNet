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
using System.Collections.Generic;
using System.Threading.Tasks;
using RtmDotNet.Auth;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Lists;

namespace RtmDotNet.Lists
{
    public class ListRepository : IListRepository
    {
        private readonly IListsUrlFactory _urlFactory;

        private readonly IApiClient _apiClient;

        private readonly AuthenticationToken _authToken;

        private readonly IResponseParser _responseParser;

        public ListRepository(IListsUrlFactory urlFactory, IApiClient apiClient, IResponseParser responseParser, AuthenticationToken authToken)
        {
            _urlFactory = urlFactory;
            _apiClient = apiClient;
            _authToken = authToken;
            _responseParser = responseParser;
        }

        public async Task<IList<IRtmList>> GetAllListsAsync()
        {
            if (_authToken.Permissions < PermissionLevel.Read)
            {
                throw new InvalidOperationException("This operation requires READ permissions of the RTM API.");
            }

            var url = _urlFactory.CreateGetListsUrl(_authToken.Id);
            var response = await _apiClient.GetAsync<GetListResponseData>(url).ConfigureAwait(false);
            return _responseParser.GetLists(response);
        }
    }
}