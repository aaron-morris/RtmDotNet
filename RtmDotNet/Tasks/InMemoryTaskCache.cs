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
using System.Linq;
using System.Threading.Tasks;

namespace RtmDotNet.Tasks
{
    public class InMemoryTaskCache : ITaskCache
    {
        private readonly ITaskTreeBuilder _taskTreeBuilder;

        private readonly Dictionary<string, List<string>> _tasksIdsPerList;

        private readonly HashSet<IRtmTask> _cachedTasks;

        private readonly object _lock;

        public InMemoryTaskCache(ITaskTreeBuilder taskTreeBuilder)
        {
            _taskTreeBuilder = taskTreeBuilder;

            _tasksIdsPerList = new Dictionary<string, List<string>>();
            _cachedTasks = new HashSet<IRtmTask>();
            _lock = new object();
        }

        public Task AddOrReplaceAsync(IEnumerable<IRtmTask> tasks)
        {
            return Task.Run(() => DoAddOrReplaceTasks(tasks));
        }

        public Task AddOrReplaceAsync(string listId, IList<IRtmTask> tasks, bool overwriteListDefinition = false)
        {
            return Task.Run(() => DoAddOrReplaceTasks(listId, tasks, overwriteListDefinition));
        }

        public Task<IList<IRtmTask>> GetAllAsync()
        {
            return Task.Run(() => DoGetTasksAsync());
        }

        public Task<IList<IRtmTask>> GetAllAsync(string listId)
        {
            return Task.Run(() => DoGetTasksAsync(listId));
        }

        public Task RemoveAsync(IList<IRtmTask> tasks)
        {
            return Task.Run(() => DoRemoveTask(tasks));
        }

        public Task ClearAsync()
        {
            return Task.Run(() => DoClearTasks());
        }

        private void DoAddOrReplaceTasks(IEnumerable<IRtmTask> tasks)
        {
            lock (_lock)
            {
                foreach (var task in tasks)
                {
                    AddOrReplaceTask(task);
                }
            }
        }

        private void DoAddOrReplaceTasks(string listId, IList<IRtmTask> tasks, bool overwriteListDefinition)
        {
            lock (_lock)
            {
                var taskIds = tasks.Select(x => x.Id);

                if (_tasksIdsPerList.ContainsKey(listId) && !overwriteListDefinition)
                {
                    // Update the list of task IDs associated with the list ID.
                    foreach (var taskId in taskIds)
                    {
                        if (!_tasksIdsPerList[listId].Contains(taskId))
                        {
                            _tasksIdsPerList[listId].Add(taskId);
                        }
                    }
                }
                else
                {
                    _tasksIdsPerList[listId] = taskIds.ToList();
                }

                foreach (var task in tasks)
                {
                    AddOrReplaceTask(task);
                }
            }
        }

        private void AddOrReplaceTask(IRtmTask task)
        {
            if (_cachedTasks.Contains(task))
            {
                _cachedTasks.Remove(task);
            }

            _cachedTasks.Add(task);
        }

        private IList<IRtmTask> DoGetTasksAsync()
        {
            lock (_lock)
            {
                return _taskTreeBuilder.AssembleTaskTrees(_cachedTasks);
            }
        }

        private IList<IRtmTask> DoGetTasksAsync(string listId)
        {
            lock (_lock)
            {
                if (!_tasksIdsPerList.ContainsKey(listId))
                {
                    throw new InvalidOperationException($"Error retrieving task list.  The specified list ID was not initialized in the internal task cache.  List ID = {listId}");
                }

                var taskIds = _tasksIdsPerList[listId];
                var rootTasks = _cachedTasks.Where(x => taskIds.Contains(x.Id));
                return _taskTreeBuilder.AssembleTaskTrees(rootTasks, _cachedTasks);
            }
        }

        private void DoRemoveTask(IList<IRtmTask> tasks)
        {
            lock (_lock)
            {
                foreach (var task in tasks)
                {
                    _cachedTasks.Remove(task);

                    foreach (var taskIdList in _tasksIdsPerList.Values)
                    {
                        if (taskIdList.Contains(task.Id))
                        {
                            taskIdList.Remove(task.Id);
                        }
                    }
                }
            }
        }

        private void DoClearTasks()
        {
            lock (_lock)
            {
                _tasksIdsPerList.Clear();
                _cachedTasks.Clear();
            }
        }
    }
}