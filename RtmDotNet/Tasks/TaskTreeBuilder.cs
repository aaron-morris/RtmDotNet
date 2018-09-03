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

using System.Collections.Generic;
using System.Linq;

namespace RtmDotNet.Tasks
{
    public class TaskTreeBuilder : ITaskTreeBuilder
    {
        public IList<IRtmTask> AssembleTaskTrees(ICollection<IRtmTask> tasks)
        {
            var taskTrees = new List<IRtmTask>();

            foreach (var task in tasks)
            {
                if (string.IsNullOrEmpty(task.ParentTaskId))
                {
                    // This is a root task.
                    taskTrees.Add(task);
                }
                else
                {
                    var parentTask = tasks.FirstOrDefault(x => x.Id.Equals(task.ParentTaskId));
                    if (parentTask != null)
                    {
                        // Add the subtask to its parent.
                        parentTask.Subtasks.Add(task);
                    }
                    else
                    {
                        // This task's parent is not available
                        taskTrees.Add(task);
                    }
                }
            }

            return taskTrees;
        }

        public IList<IRtmTask> AssembleTaskTrees(IEnumerable<IRtmTask> rootTasks, ICollection<IRtmTask> allTasks)
        {
            var taskTrees = new List<IRtmTask>();

            foreach (var rootTask in rootTasks)
            {
                rootTask.Subtasks = GetSubtasks(rootTask, allTasks);
                taskTrees.Add(rootTask);
            }

            return taskTrees;
        }

        private IList<IRtmTask> GetSubtasks(IRtmTask parentTask, ICollection<IRtmTask> allTasks)
        {
            var subtasks = allTasks.Where(subtask => string.Equals(parentTask.Id, subtask.ParentTaskId)).ToList();

            foreach (var subtask in subtasks)
            {
                subtask.Subtasks = GetSubtasks(subtask, allTasks);
            }

            return subtasks;
        }
    }
}