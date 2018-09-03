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
using System.Collections.Generic;
using System.Threading.Tasks;
using RtmDotNet.Http.Api.Tasks;

namespace RtmDotNet.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ITaskApiClient _taskApiClient;

        private readonly IResponseParser _responseParser;

        private readonly ITaskCache _taskCache;

        private readonly ISyncTracker _syncTracker;

        public TaskRepository(ITaskApiClient taskApiClient, IResponseParser responseParser, ITaskCache taskCache, ISyncTracker syncTracker)
        {
            _taskApiClient = taskApiClient;
            _responseParser = responseParser;
            _taskCache = taskCache;
            _syncTracker = syncTracker;
        }

        public async Task<IList<IRtmTask>> GetAllTasksAsync(bool includeCompletedTasks = false)
        {
            var response = await _taskApiClient.GetAllTasksAsync(includeCompletedTasks).ConfigureAwait(false);
            var tasks = _responseParser.GetTasks(response);

            // Because we just retrieved a full, up-to-date list of tasks, clear the internal cache before processing the new list.
            await _taskCache.ClearAsync().ConfigureAwait(false);

            await _taskCache.AddOrReplaceAsync(tasks).ConfigureAwait(false);
            return await _taskCache.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<IList<IRtmTask>> GetTasksByListIdAsync(string listId, bool includeCompletedTasks = false)
        {
            var currentSync = DateTime.Now;
            await UpdateTaskCacheForListId(listId, null, currentSync, includeCompletedTasks).ConfigureAwait(false);
            return await _taskCache.GetAllAsync(listId).ConfigureAwait(false);
        }

        private async Task UpdateTaskCacheForListId(string listId, DateTime? lastSync, DateTime currentSync, bool includeCompletedTasks)
        {
            var response = await _taskApiClient.GetTasksByListIdAsync(listId, lastSync, includeCompletedTasks).ConfigureAwait(false);

            var tasks = _responseParser.GetTasks(response);
            
            if (lastSync != null)
            {
                await _taskCache.AddOrReplaceAsync(listId, tasks).ConfigureAwait(false);

                var deletedTasks = _responseParser.GetDeletedTasks(response);
                await _taskCache.RemoveAsync(deletedTasks).ConfigureAwait(false); 
            }
            else
            {
                await _taskCache.AddOrReplaceAsync(listId, tasks, true).ConfigureAwait(false);
            }

            _syncTracker.SetLastSync(listId, currentSync);

            foreach (var task in tasks)
            {
                if (!task.ListId.Equals(listId))
                {
                    // The current list is not this task's master list.  We'll need to ensure that the master list is also synchronized so that we have
                    // all subtask information.
                    var masterListLastSync = _syncTracker.GetLastSync(task.ListId);
                    if (masterListLastSync == null || masterListLastSync.Value < currentSync)
                    {
                        await UpdateTaskCacheForListId(task.ListId, masterListLastSync, currentSync, includeCompletedTasks).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}