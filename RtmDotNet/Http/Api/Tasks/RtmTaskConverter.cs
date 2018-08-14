// -----------------------------------------------------------------------
// <copyright file="RtmTaskConverter.cs" author="Aaron Morris">
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

using System.Collections.Generic;
using System.Linq;
using RtmDotNet.Tasks;

namespace RtmDotNet.Http.Api.Tasks
{
    public class RtmTaskConverter : IRtmTaskConverter
    {
        public IList<IRtmTask> ConvertToTasks(GetListResponseData responseData)
        {
            var individualTasks = new List<IRtmTask>();

            foreach (var list in responseData.Lists.Lists)
            {
                foreach (var taskSeries in list.TaskSeries)
                {
                    individualTasks.AddRange(ConvertResponseDataToTasks(list.ListId, taskSeries));
                }
            }

            return AssembleTaskTree(individualTasks);
        }

        private IList<IRtmTask> ConvertResponseDataToTasks(string listId, GetListResponseData.TaskSeriesData taskSeriesData)
        {
            var tasks = new List<IRtmTask>();

            foreach (var taskInstance in taskSeriesData.TaskInstances)
            {
                var task = new RtmTask
                {
                    Added = taskInstance.Added,
                    Completed = taskInstance.Completed,
                    Created = taskSeriesData.Created,
                    Deleted = taskInstance.Deleted,
                    Due = taskInstance.Due,
                    Estimate = taskInstance.Estimate,
                    HasDueTime = taskInstance.HasDueTime,
                    HasStartTime = taskInstance.HasStartTime,
                    Id = taskInstance.Id,
                    LocationId = taskSeriesData.LocationId,
                    Name = taskSeriesData.Name,
                    Modified = taskSeriesData.Modified,
                    ParentTaskId = taskSeriesData.ParentTaskId,
                    SeriesId = taskSeriesData.Id,
                    ListId = listId,
                    Postponed = taskInstance.Postponed,
                    Priority = taskInstance.Priority,
                    Source = taskSeriesData.Source,
                    Start = taskInstance.Start,
                    Tags = taskSeriesData.Tags,
                    Url = taskSeriesData.Url
                };

                foreach (var noteData in taskSeriesData.Notes)
                {
                    var note = new RtmTask.Note
                    {
                        Created = noteData.Created,
                        Id = noteData.Id,
                        Modified = noteData.Modified,
                        Text = noteData.Text,
                        Title = noteData.Title
                    };

                    task.Notes.Add(note);
                }

                tasks.Add(task);
            }

            return tasks;
        }

        private IList<IRtmTask> AssembleTaskTree(IList<IRtmTask> individualTasks)
        {
            var taskTree = new List<IRtmTask>();

            foreach (var task in individualTasks)
            {
                if (string.IsNullOrEmpty(task.ParentTaskId))
                {
                    // This is a high-level task.
                    taskTree.Add(task);
                }
                else
                {
                    var parentTask = individualTasks.FirstOrDefault(x => x.Id.Equals(task.ParentTaskId));
                    if (parentTask != null)
                    {
                        // Add the subtask to its parent.
                        parentTask.Subtasks.Add(task);
                    }
                    else
                    {
                        // This task's parent is not available
                        taskTree.Add(task);
                    }
                }
            }

            return taskTree;
        }
    }
}