// -----------------------------------------------------------------------
// <copyright file="RtmTaskConverterTests.cs" author="Aaron Morris">
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
using System.Linq;
using NUnit.Framework;
using RtmDotNet.Http.Api.Tasks;

namespace RtmDotNet.UnitTests.Http.Api.Tasks
{
    [TestFixture]
    public class RtmTaskConverterTests
    {
        private const string ExpectedEstimate = "Fake Estimate";
        private const bool ExpectedHasDueTime = true;
        private const bool ExpectedHasStartTime = true;
        private const string ExpectedId = "Fake Task Id";
        private const string ExpectedLocationId = "Fake Location ID";
        private const string ExpectedName = "Fake Task Name";
        private const string ExpectedParentTaskId = "Fake Parent ID";
        private const string ExpectedSeriesId = "Fake Series ID";
        private const string ExpectedListId = "Fake List ID";
        private const int ExpectedPostponed = 5;
        private const string ExpectedPriority = "Fake Priority";
        private const string ExpectedSource = "Fake Source";
        private const string ExpectedUrl = "Fake URL";
        private const string ExpectedNoteId = "Fake Note ID";
        private const string ExpectedNoteText = "Fake Note Text";
        private const string ExpectedNoteTitle = "Fake Note Title";

        private readonly DateTime _expectedAdded = DateTime.Parse("2018-01-02T01:23:24");
        private readonly DateTime _expectedCompleted = DateTime.Parse("2018-03-03T02:34:56");
        private readonly DateTime _expectedCreated = DateTime.Parse("2017-01-02T01:23:24");
        private readonly DateTime _expectedDeleted = DateTime.Parse("2018-10-02T01:23:24");
        private readonly DateTime _expectedDue = DateTime.Parse("2019-01-02T01:23:24");
        private readonly DateTime _expectedModified = DateTime.Parse("2018-01-02T11:23:24");
        private readonly DateTime _expectedStart = DateTime.Parse("2018-11-01T01:23:24");
        private readonly IList<string> _expectedTags = new List<string> { "tag1", "tag2" };
        private readonly DateTime _expectedNoteCreated = DateTime.Parse("2015-11-01T01:23:24");
        private readonly DateTime _expectedNoteModified = DateTime.Parse("2018-07-01T08:30:24");

        [Test]
        public void ConvertToTasks_TaskData_InitsFromTaskData()
        {
            var fakeResponseData = GetFakeResponsesData();
            var converter = GetItemUnderTest();
            var actual = converter.ConvertToTasks(fakeResponseData);


            // Verify
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(ExpectedEstimate, actual[0].Estimate);
            Assert.AreEqual(ExpectedHasDueTime, actual[0].HasDueTime);
            Assert.AreEqual(ExpectedHasStartTime, actual[0].HasStartTime);
            Assert.AreEqual(ExpectedId, actual[0].Id);
            Assert.AreEqual(ExpectedLocationId, actual[0].LocationId);
            Assert.AreEqual(ExpectedName, actual[0].Name);
            Assert.AreEqual(ExpectedParentTaskId, actual[0].ParentTaskId);
            Assert.AreEqual(ExpectedListId, actual[0].ListId);
            Assert.AreEqual(ExpectedSeriesId, actual[0].SeriesId);
            Assert.AreEqual(ExpectedPostponed, actual[0].Postponed);
            Assert.AreEqual(ExpectedPriority, actual[0].Priority);
            Assert.AreEqual(ExpectedSource, actual[0].Source);
            Assert.AreEqual(ExpectedUrl, actual[0].Url);
            Assert.AreEqual(_expectedAdded, actual[0].Added);
            Assert.AreEqual(_expectedCompleted, actual[0].Completed);
            Assert.AreEqual(_expectedCreated, actual[0].Created);
            Assert.AreEqual(_expectedDeleted, actual[0].Deleted);
            Assert.AreEqual(_expectedDue, actual[0].Due);
            Assert.AreEqual(_expectedModified, actual[0].Modified);
            Assert.AreEqual(_expectedStart, actual[0].Start);
            Assert.AreEqual(_expectedTags, actual[0].Tags);

            Assert.AreEqual(1, actual[0].Notes.Count);
            Assert.AreEqual(ExpectedNoteId, actual[0].Notes[0].Id);
            Assert.AreEqual(ExpectedNoteText, actual[0].Notes[0].Text);
            Assert.AreEqual(ExpectedNoteTitle, actual[0].Notes[0].Title);
            Assert.AreEqual(_expectedNoteCreated, actual[0].Notes[0].Created);
            Assert.AreEqual(_expectedNoteModified, actual[0].Notes[0].Modified);
        }

        [Test]
        public void ConvertToTasks_TasksWithSubtasks_AssemblesIntoTree()
        {
            // Setup
            var fakeResponseData = GetFakeResponsesData();

            var fakeParentTask = GetFakeTaskSeriesData();
            fakeParentTask.TaskInstances[0].Id = "Task With Children";
            fakeParentTask.ParentTaskId = string.Empty;
            fakeResponseData.Lists.Lists[0].TaskSeries.Add(fakeParentTask);

            var fakeChildWithSubChildren = GetFakeTaskSeriesData();
            fakeChildWithSubChildren.TaskInstances[0].Id = "Subtask With Children";
            fakeChildWithSubChildren.ParentTaskId = "Task With Children";
            fakeResponseData.Lists.Lists[0].TaskSeries.Add(fakeChildWithSubChildren);

            var fakeChildWithoutSubChildren = GetFakeTaskSeriesData();
            fakeChildWithoutSubChildren.TaskInstances[0].Id = "Subtask Without Children";
            fakeChildWithoutSubChildren.ParentTaskId = "Task With Children";
            fakeResponseData.Lists.Lists[0].TaskSeries.Add(fakeChildWithoutSubChildren);

            var fakeGrandChild = GetFakeTaskSeriesData();
            fakeGrandChild.TaskInstances[0].Id = "Subtask of Subtask";
            fakeGrandChild.ParentTaskId = "Subtask With Children";
            fakeResponseData.Lists.Lists[0].TaskSeries.Add(fakeGrandChild);

            // Execute
            var converter = GetItemUnderTest();
            var actual = converter.ConvertToTasks(fakeResponseData);

            // Verify
            Assert.AreEqual(2, actual.Count); // The list should have the default task created by the GetFakeResponseData method, plus our parent task above.

            var defaultTask = actual.Single(task => task.Id.Equals(ExpectedId));
            Assert.IsFalse(defaultTask.Subtasks.Any()); // The default test task should not have any subtasks.

            var parentTask = actual.Single(task => task.Id.Equals("Task With Children"));
            Assert.AreEqual(2, parentTask.Subtasks.Count);

            var subTaskWithChildren = parentTask.Subtasks.Single(task => task.Id.Equals("Subtask With Children"));
            Assert.AreEqual(1, subTaskWithChildren.Subtasks.Count);

            var grandchild = subTaskWithChildren.Subtasks.Single(task => task.Id.Equals("Subtask of Subtask"));
            Assert.IsFalse(grandchild.Subtasks.Any());

            var subTaskWithoutChildren = parentTask.Subtasks.Single(task => task.Id.Equals("Subtask Without Children"));
            Assert.IsFalse(subTaskWithoutChildren.Subtasks.Any());
        }

        private IRtmTaskConverter GetItemUnderTest()
        {
            return new RtmTaskConverter();
        }

        private GetListResponseData GetFakeResponsesData()
        {
            return new GetListResponseData
            {
                Lists = new GetListResponseData.ListOfLists
                {
                    Lists = new List<GetListResponseData.TaskListData>
                    {
                        new GetListResponseData.TaskListData
                        {
                            ListId = ExpectedListId,
                            TaskSeries = new List<GetListResponseData.TaskSeriesData>
                            {
                                GetFakeTaskSeriesData()
                            }
                        }
                    }
                }
            };
        }

        private GetListResponseData.TaskSeriesData GetFakeTaskSeriesData()
        {
            return new GetListResponseData.TaskSeriesData
            {
                Created = _expectedCreated,
                Id = ExpectedSeriesId,
                LocationId = ExpectedLocationId,
                Modified = _expectedModified,
                Name = ExpectedName,
                Notes = new List<GetListResponseData.Note>
                {
                    new GetListResponseData.Note
                    {
                        Id = ExpectedNoteId,
                        Created = _expectedNoteCreated,
                        Modified = _expectedNoteModified,
                        Text = ExpectedNoteText,
                        Title = ExpectedNoteTitle
                    }
                },
                ParentTaskId = ExpectedParentTaskId,
                Source = ExpectedSource,
                Tags = _expectedTags,
                Url = ExpectedUrl,
                TaskInstances = new List<GetListResponseData.TaskData>
                {
                    new GetListResponseData.TaskData
                    {
                        Added = _expectedAdded,
                        Completed = _expectedCompleted,
                        Deleted = _expectedDeleted,
                        Due = _expectedDue,
                        Estimate = ExpectedEstimate,
                        HasDueTime = ExpectedHasDueTime,
                        HasStartTime = ExpectedHasStartTime,
                        Id = ExpectedId,
                        Start = _expectedStart,
                        Priority = ExpectedPriority,
                        Postponed = ExpectedPostponed
                    }
                }
            };
        }
    }

    
}