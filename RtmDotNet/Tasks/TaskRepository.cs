// -----------------------------------------------------------------------
// <copyright file="TaskRepository.cs" author="Aaron Morris">
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
using System.Collections.Generic;
using System.Threading.Tasks;
using RtmDotNet.Auth;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Tasks;

namespace RtmDotNet.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ITasksUrlFactory _urlFactory;

        private readonly IRtmApiClient _apiClient;

        private readonly AuthenticationToken _authToken;

        private readonly IRtmTaskConverter _taskConverter;

        public TaskRepository(ITasksUrlFactory urlFactory, IRtmApiClient apiClient, AuthenticationToken authToken, IRtmTaskConverter taskConverter)
        {
            _urlFactory = urlFactory;
            _apiClient = apiClient;
            _authToken = authToken;
            _taskConverter = taskConverter;
        }

        public async Task<IList<IRtmTask>> GetAllTasksAsync(bool includeCompletedTasks = false)
        {
            if (_authToken.Permissions < PermissionLevel.Read)
            {
                throw new InvalidOperationException("This operation requires READ permissions of the RTM API.");
            }

            var taskStatusFilter = includeCompletedTasks ? string.Empty : "status:incomplete";
            var url = _urlFactory.CreateGetListsUrl(_authToken.Id, filter:taskStatusFilter);
            var response = await _apiClient.GetAsync<GetListResponseData>(url);
            return _taskConverter.ConvertToTasks(response);
        }
    }
}