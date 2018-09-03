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
using RtmDotNet.Auth;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Tasks;

namespace RtmDotNet.Tasks
{
    public class TaskApiClient : ITaskApiClient
    {
        private readonly ITasksUrlFactory _urlFactory;

        private readonly IApiClient _apiClient;

        private readonly AuthenticationToken _authToken;

        public TaskApiClient(ITasksUrlFactory urlFactory, IApiClient apiClient, AuthenticationToken authToken)
        {
            _urlFactory = urlFactory;
            _apiClient = apiClient;
            _authToken = authToken;
        }

        public async Task<GetListResponseData> GetAllTasksAsync(bool includeCompletedTasks = false)
        {
            if (_authToken.Permissions < PermissionLevel.Read)
            {
                throw new InvalidOperationException("This operation requires READ permissions of the RTM API.");
            }

            var taskStatusFilter = includeCompletedTasks ? string.Empty : "status:incomplete";
            var url = _urlFactory.CreateGetListsUrl(_authToken.Id, filter: taskStatusFilter);
            return await _apiClient.GetAsync<GetListResponseData>(url).ConfigureAwait(false);
        }

        public async Task<GetListResponseData> GetAllTasksAsync(DateTime lastSync, bool includeCompletedTasks = false)
        {
            if (_authToken.Permissions < PermissionLevel.Read)
            {
                throw new InvalidOperationException("This operation requires READ permissions of the RTM API.");
            }

            var taskStatusFilter = includeCompletedTasks ? string.Empty : "status:incomplete";
            var url = _urlFactory.CreateGetListsUrl(_authToken.Id, lastSync, filter: taskStatusFilter);
            return await _apiClient.GetAsync<GetListResponseData>(url).ConfigureAwait(false);
        }

        public async Task<GetListResponseData> GetTasksByListIdAsync(string listId, DateTime? lastSync = null, bool includeCompletedTasks = false)
        {
            if (_authToken.Permissions < PermissionLevel.Read)
            {
                throw new InvalidOperationException("This operation requires READ permissions of the RTM API.");
            }

            var taskStatusFilter = includeCompletedTasks ? string.Empty : "status:incomplete";
            var url = _urlFactory.CreateGetListsUrl(_authToken.Id, lastSync, listId, taskStatusFilter);
            return await _apiClient.GetAsync<GetListResponseData>(url).ConfigureAwait(false);
        }
    }
}