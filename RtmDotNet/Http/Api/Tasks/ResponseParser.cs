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
using RtmDotNet.Locations;
using RtmDotNet.Tasks;

namespace RtmDotNet.Http.Api.Tasks
{
    public class ResponseParser : IResponseParser
    {
        private readonly ILocationRepository _locationRepository;

        public ResponseParser(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public IList<IRtmTask> GetTasks(GetListResponseData responseData)
        {
            var tasks = new List<IRtmTask>();

            if (responseData?.Lists?.Lists == null)
            {
                return new List<IRtmTask>();
            }

            foreach (var list in responseData.Lists.Lists)
            {
                foreach (var taskSeries in list.TaskSeries)
                {
                    tasks.AddRange(ConvertResponseDataToTasks(list.ListId, taskSeries));
                }
            }

            return tasks;
        }

        public IList<IRtmTask> GetDeletedTasks(GetListResponseData responseData)
        {
            var deletedItems = new List<IRtmTask>();

            if (responseData?.Lists?.Lists == null)
            {
                return new List<IRtmTask>();
            }

            foreach (var list in responseData.Lists.Lists)
            {
                foreach (var deletedItem in list.DeletedItems)
                {
                    deletedItems.AddRange(ConvertResponseDataToTasks(list.ListId, deletedItem.TaskSeries));
                }
            }

            return deletedItems;
        }

        private IList<IRtmTask> ConvertResponseDataToTasks(string listId, GetListResponseData.TaskSeriesData taskSeriesData)
        {
            var tasks = new List<IRtmTask>();

            foreach (var taskInstance in taskSeriesData.TaskInstances)
            {
                var task = new RtmTask(taskInstance.Id)
                {
                    Added = taskInstance.Added,
                    Completed = taskInstance.Completed,
                    Created = taskSeriesData.Created,
                    Deleted = taskInstance.Deleted,
                    Due = taskInstance.Due,
                    Estimate = taskInstance.Estimate,
                    HasDueTime = taskInstance.HasDueTime,
                    HasStartTime = taskInstance.HasStartTime,
                    Location = GetLocation(taskSeriesData.LocationId),
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

        private IRtmLocation GetLocation(string locationId)
        {
            if (string.IsNullOrEmpty(locationId))
            {
                return null;
            }

            var getLocationTask = _locationRepository.GetLocationByIdAsync(locationId);
            var location = getLocationTask.Result;
            return location;
        }
    }
}