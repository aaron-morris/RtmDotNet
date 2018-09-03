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
using System.Threading.Tasks;
using RtmDotNet.Http.Api.Tasks;

namespace RtmDotNet.Tasks
{
    public interface ITaskApiClient
    {
        Task<GetListResponseData> GetAllTasksAsync(bool includeCompletedTasks = false);

        Task<GetListResponseData> GetAllTasksAsync(DateTime lastSync, bool includeCompletedTasks = false);

        Task<GetListResponseData> GetTasksByListIdAsync(string listId, DateTime? lastSync = null, bool includeCompletedTasks = false);
    }
}