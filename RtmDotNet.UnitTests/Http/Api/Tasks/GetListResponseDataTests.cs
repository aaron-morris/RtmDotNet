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
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Tasks;

namespace RtmDotNet.UnitTests.Http.Api.Tasks
{
    [TestFixture]
    public class GetListResponseDataTests
    {
        private const string CurrentListWithNoUpdatedTasksJson =
            "{\r\n          \"id\": \"123456\",\r\n          \"current\": \"2018-08-16T00:00:00Z\"\r\n        }";

        private const string ListWithUpdatedTasksJson =
            "{\r\n  \"id\": \"My List with Updated Tasks\",\r\n  \"current\": \"2018-08-16T00:00:00Z\",\r\n  \"taskseries\": [\r\n    {\r\n      \"id\": \"Task Series ID 1\",\r\n      \"created\": \"2018-08-16T09:30:04Z\",\r\n      \"modified\": \"2018-08-16T09:30:04Z\",\r\n      \"name\": \"My Task\",\r\n      \"source\": \"recurrence\",\r\n      \"url\": \"\",\r\n      \"location_id\": \"\",\r\n      \"parent_task_id\": \"624070987\",\r\n      \"tags\": [],\r\n      \"participants\": [],\r\n      \"notes\": [],\r\n      \"task\": [\r\n        {\r\n          \"id\": \"Task Series ID 1 - Task 1\",\r\n          \"due\": \"\",\r\n          \"has_due_time\": \"0\",\r\n          \"added\": \"2018-08-09T09:30:04Z\",\r\n          \"completed\": \"\",\r\n          \"deleted\": \"\",\r\n          \"priority\": \"N\",\r\n          \"postponed\": \"0\",\r\n          \"estimate\": \"\",\r\n          \"start\": \"\",\r\n          \"has_start_time\": \"0\"\r\n        },\r\n        {\r\n          \"id\": \"Task Series ID 1 - Task 2\",\r\n          \"due\": \"\",\r\n          \"has_due_time\": \"0\",\r\n          \"added\": \"2018-08-16T09:30:04Z\",\r\n          \"completed\": \"\",\r\n          \"deleted\": \"\",\r\n          \"priority\": \"N\",\r\n          \"postponed\": \"0\",\r\n          \"estimate\": \"\",\r\n          \"start\": \"\",\r\n          \"has_start_time\": \"0\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"Task Series ID 2\",\r\n      \"created\": \"2018-08-16T09:30:04Z\",\r\n      \"modified\": \"2018-08-16T09:30:04Z\",\r\n      \"name\": \"My Task\",\r\n      \"source\": \"recurrence\",\r\n      \"url\": \"\",\r\n      \"location_id\": \"\",\r\n      \"parent_task_id\": \"624070987\",\r\n      \"tags\": [],\r\n      \"participants\": [],\r\n      \"notes\": [],\r\n      \"task\": [\r\n        {\r\n          \"id\": \"Task Series ID 2 - Task 1\",\r\n          \"due\": \"\",\r\n          \"has_due_time\": \"0\",\r\n          \"added\": \"2018-08-09T09:30:04Z\",\r\n          \"completed\": \"\",\r\n          \"deleted\": \"\",\r\n          \"priority\": \"N\",\r\n          \"postponed\": \"0\",\r\n          \"estimate\": \"\",\r\n          \"start\": \"\",\r\n          \"has_start_time\": \"0\"\r\n        },\r\n        {\r\n          \"id\": \"Task Series ID 2 - Task 2\",\r\n          \"due\": \"\",\r\n          \"has_due_time\": \"0\",\r\n          \"added\": \"2018-08-16T09:30:04Z\",\r\n          \"completed\": \"\",\r\n          \"deleted\": \"\",\r\n          \"priority\": \"N\",\r\n          \"postponed\": \"0\",\r\n          \"estimate\": \"\",\r\n          \"start\": \"\",\r\n          \"has_start_time\": \"0\"\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}";

        private const string ListWithUpdatedAndDeletedTasksJson =
            "{\r\n  \"id\": \"My List with Updated and Deleted Tasks\",\r\n  \"current\": \"2018-08-16T00:00:00Z\",\r\n  \"taskseries\":   [\r\n    {\r\n      \"id\": \"Task Series ID 1\",\r\n      \"created\": \"2018-08-16T09:30:04Z\",\r\n      \"modified\": \"2018-08-16T09:30:04Z\",\r\n      \"name\": \"My Task\",\r\n      \"source\": \"recurrence\",\r\n      \"url\": \"\",\r\n      \"location_id\": \"\",\r\n      \"parent_task_id\": \"624070987\",\r\n      \"tags\": [],\r\n      \"participants\": [],\r\n      \"notes\": [],\r\n      \"task\": [\r\n        {\r\n          \"id\": \"Task Series ID 1 - Task 1\",\r\n          \"due\": \"\",\r\n          \"has_due_time\": \"0\",\r\n          \"added\": \"2018-08-09T09:30:04Z\",\r\n          \"completed\": \"\",\r\n          \"deleted\": \"\",\r\n          \"priority\": \"N\",\r\n          \"postponed\": \"0\",\r\n          \"estimate\": \"\",\r\n          \"start\": \"\",\r\n          \"has_start_time\": \"0\"\r\n        },\r\n        {\r\n          \"id\": \"Task Series ID 1 - Task 2\",\r\n          \"due\": \"\",\r\n          \"has_due_time\": \"0\",\r\n          \"added\": \"2018-08-16T09:30:04Z\",\r\n          \"completed\": \"\",\r\n          \"deleted\": \"\",\r\n          \"priority\": \"N\",\r\n          \"postponed\": \"0\",\r\n          \"estimate\": \"\",\r\n          \"start\": \"\",\r\n          \"has_start_time\": \"0\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"id\": \"Task Series ID 2\",\r\n      \"created\": \"2018-08-16T09:30:04Z\",\r\n      \"modified\": \"2018-08-16T09:30:04Z\",\r\n      \"name\": \"My Task\",\r\n      \"source\": \"recurrence\",\r\n      \"url\": \"\",\r\n      \"location_id\": \"\",\r\n      \"parent_task_id\": \"624070987\",\r\n      \"tags\": [],\r\n      \"participants\": [],\r\n      \"notes\": [],\r\n      \"task\": [\r\n        {\r\n          \"id\": \"Task Series ID 2 - Task 1\",\r\n          \"due\": \"\",\r\n          \"has_due_time\": \"0\",\r\n          \"added\": \"2018-08-09T09:30:04Z\",\r\n          \"completed\": \"\",\r\n          \"deleted\": \"\",\r\n          \"priority\": \"N\",\r\n          \"postponed\": \"0\",\r\n          \"estimate\": \"\",\r\n          \"start\": \"\",\r\n          \"has_start_time\": \"0\"\r\n        },\r\n        {\r\n          \"id\": \"Task Series ID 2 - Task 2\",\r\n          \"due\": \"\",\r\n          \"has_due_time\": \"0\",\r\n          \"added\": \"2018-08-16T09:30:04Z\",\r\n          \"completed\": \"\",\r\n          \"deleted\": \"\",\r\n          \"priority\": \"N\",\r\n          \"postponed\": \"0\",\r\n          \"estimate\": \"\",\r\n          \"start\": \"\",\r\n          \"has_start_time\": \"0\"\r\n        }\r\n      ]\r\n    }\r\n  ],\r\n  \"deleted\": [\r\n    {\r\n      \"taskseries\": {\r\n        \"id\": \"Deleted Series 1\",\r\n        \"task\": [\r\n          {\r\n            \"id\": \"Deleted Series 1 - Task 1\",\r\n            \"deleted\": \"\"\r\n          },\r\n          {\r\n            \"id\": \"Deleted Series 1 - Task 2\",\r\n            \"deleted\": \"\"\r\n          }\r\n        ]\r\n      }\r\n    },\r\n    {\r\n      \"taskseries\": {\r\n        \"id\": \"Deleted Series 2\",\r\n        \"task\": [\r\n          {\r\n            \"id\": \"Deleted Series 2 - Task 1\",\r\n            \"deleted\": \"\"\r\n          },\r\n          {\r\n            \"id\": \"Deleted Series 2 - Task 2\",\r\n            \"deleted\": \"\"\r\n          }\r\n        ]\r\n      }\r\n    }\r\n  ]\r\n}";

        private static readonly string FullResponseJson =
            $"{{\r\n  \"rsp\": {{\r\n    \"stat\": \"ok\",\r\n    \"tasks\": {{\r\n      \"rev\": \"9900ml6wpqdsr18z9re8oq07xwwwiwp\",\r\n      \"list\": [{CurrentListWithNoUpdatedTasksJson},\r\n\t\t\t{ListWithUpdatedTasksJson},\r\n\t\t\t{ListWithUpdatedAndDeletedTasksJson},\r\n\t\t\t\r\n      ]\r\n    }}\r\n  }}\r\n}}";

        [Test]
        public void CurrentListWithNoTasks_ParsesFromJson()
        {
            // Execute
            var actual = JsonConvert.DeserializeObject<GetListResponseData.TaskListData>(CurrentListWithNoUpdatedTasksJson);

            // Verify
            Assert.AreEqual("123456", actual.ListId);
            Assert.AreEqual(DateTime.Parse("2018-08-16"), actual.Current);
            Assert.IsFalse(actual.TaskSeries.Any());
        }

        [Test]
        public void ListWithUpdatedTasks_ParsesFromJson()
        {
            // Execute
            var actual = JsonConvert.DeserializeObject<GetListResponseData.TaskListData>(ListWithUpdatedTasksJson);

            // Verify
            Assert.AreEqual("My List with Updated Tasks", actual.ListId);
            Assert.AreEqual(DateTime.Parse("2018-08-16"), actual.Current);
            
            Assert.AreEqual(2, actual.TaskSeries.Count);
        }

        [Test]
        public void ListWithUpdatedAndDeletedTasks_ParsesFromJson()
        {
            // Execute
            var actual = JsonConvert.DeserializeObject<GetListResponseData.TaskListData>(ListWithUpdatedAndDeletedTasksJson);

            // Verify
            Assert.AreEqual("My List with Updated and Deleted Tasks", actual.ListId);
            Assert.AreEqual(DateTime.Parse("2018-08-16"), actual.Current);

            Assert.AreEqual(2, actual.TaskSeries.Count);
            Assert.AreEqual(2, actual.DeletedItems.Count);
        }

        [Test]
        public void MultipleLists_ParsesFromJson()
        {
            // Execute
            var actual = JsonConvert.DeserializeObject<ApiResponse<GetListResponseData>>(FullResponseJson);

            // Verify
            Assert.AreEqual(3, actual.Content.Lists.Lists.Count);

            Assert.AreEqual("123456", actual.Content.Lists.Lists[0].ListId);
            Assert.AreEqual("My List with Updated Tasks", actual.Content.Lists.Lists[1].ListId);
            Assert.AreEqual("My List with Updated and Deleted Tasks", actual.Content.Lists.Lists[2].ListId);
        }
    }
}