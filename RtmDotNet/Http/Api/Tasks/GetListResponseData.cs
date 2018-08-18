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
using Newtonsoft.Json;

namespace RtmDotNet.Http.Api.Tasks
{
    public class GetListResponseData : ApiResponseData
    {
        [JsonProperty("tasks")]
        public ListOfLists Lists { get; set; }

        public class ListOfLists
        {
            [JsonProperty("list")]
            public IList<TaskListData> Lists { get; set; }
        }

        public class TaskListData
        {
            public TaskListData()
            {
                TaskSeries = new List<TaskSeriesData>();
            }

            [JsonProperty("id")]
            public string ListId { get; set; }

            [JsonProperty("taskseries")]
            public IList<TaskSeriesData> TaskSeries { get; set; }
        }

        public class TaskSeriesData
        {
            public TaskSeriesData()
            {
                Tags = new List<string>();
                TaskInstances = new List<TaskData>();
            }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("created")]
            public DateTime Created { get; set; }

            [JsonProperty("modified")]
            public DateTime Modified { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("source")]
            public string Source { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("location_id")]
            public string LocationId { get; set; }

            [JsonProperty("parent_task_id")]
            public string ParentTaskId { get; set; }

            [JsonProperty("tags")]
            [JsonConverter(typeof(RtmArrayJsonConverter<string>))]
            public IList<string> Tags { get; set; }

            [JsonProperty("notes")]
            [JsonConverter(typeof(RtmArrayJsonConverter<Note>))]
            public IList<Note> Notes { get; set; }

            [JsonProperty("task")]
            public IList<TaskData> TaskInstances { get; set; }
        }

        public class Note
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("created")]
            public DateTime Created { get; set; }

            [JsonProperty("modified")]
            public DateTime Modified { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("$t")]
            public string Text { get; set; }
        }

        public class TaskData
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("due")]
            public DateTime? Due { get; set; }

            [JsonProperty("has_due_time")]
            [JsonConverter(typeof(RtmBooleanJsonConverter))]
            public bool HasDueTime { get; set; }

            [JsonProperty("added")]
            public DateTime Added { get; set; }

            [JsonProperty("completed")]
            public DateTime? Completed { get; set; }

            [JsonProperty("deleted")]
            public DateTime? Deleted { get; set; }

            [JsonProperty("priority")]
            public string Priority { get; set; }

            [JsonProperty("postponed")]
            public int Postponed { get; set; }

            [JsonProperty("estimate")]
            public string Estimate { get; set; }

            [JsonProperty("start")]
            public DateTime? Start { get; set; }

            [JsonProperty("has_start_time")]
            [JsonConverter(typeof(RtmBooleanJsonConverter))]
            public bool HasStartTime { get; set; }
        }
    }
}