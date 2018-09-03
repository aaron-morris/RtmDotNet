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
using NUnit.Framework;
using RtmDotNet.Tasks;

namespace RtmDotNet.UnitTests.Tasks
{
    [TestFixture]
    public class TaskTreeBuilderTests
    {
        [Test]
        public void AssembleTaskTrees_TasksOnly_AssemblesIntoTreesAddingOrphansAsRoots()
        {
            // Setup
            var fakeTasks = new List<IRtmTask>();

            var fakeParentTask = GetFakeTask("Task With Children");
            fakeParentTask.ParentTaskId = string.Empty;
            fakeTasks.Add(fakeParentTask);

            var fakeChildWithSubChildren = GetFakeTask("Subtask With Children");
            fakeChildWithSubChildren.ParentTaskId = "Task With Children";
            fakeTasks.Add(fakeChildWithSubChildren);

            var fakeChildWithoutSubChildren = GetFakeTask("Subtask Without Children");
            fakeChildWithoutSubChildren.ParentTaskId = "Task With Children";
            fakeTasks.Add(fakeChildWithoutSubChildren);

            var fakeGrandChild = GetFakeTask("Subtask of Subtask");
            fakeGrandChild.ParentTaskId = "Subtask With Children";
            fakeTasks.Add(fakeGrandChild);

            var fakeOrphan = GetFakeTask("Task with Missing Parent");
            fakeOrphan.ParentTaskId = "Non-existent Parent ID";
            fakeTasks.Add(fakeOrphan);

            // Execute
            var treeBuilder = GetItemUnderTest();
            var actual = treeBuilder.AssembleTaskTrees(fakeTasks);

            // Verify
            Assert.AreEqual(2, actual.Count);

            var parentTask = actual.Single(task => task.Id.Equals("Task With Children"));
            Assert.AreEqual(2, parentTask.Subtasks.Count);

            var subTaskWithChildren = parentTask.Subtasks.Single(task => task.Id.Equals("Subtask With Children"));
            Assert.AreEqual(1, subTaskWithChildren.Subtasks.Count);

            var grandchild = subTaskWithChildren.Subtasks.Single(task => task.Id.Equals("Subtask of Subtask"));
            Assert.IsFalse(grandchild.Subtasks.Any());

            var subTaskWithoutChildren = parentTask.Subtasks.Single(task => task.Id.Equals("Subtask Without Children"));
            Assert.IsFalse(subTaskWithoutChildren.Subtasks.Any());

            var orphan = actual.Single(task => task.Id.Equals("Task with Missing Parent"));
            Assert.AreEqual(0, orphan.Subtasks.Count);
        }

        [Test]
        public void AssembleTaskTrees_TasksWithCollection_AssemblesIntoTreesSearchingForSubtasksInCollection()
        {
            // Setup
            var fakeTasks = new List<IRtmTask>();
            var taskCollection = new List<IRtmTask>();

            var fakeParentTask = GetFakeTask("Task With Children");
            fakeParentTask.ParentTaskId = string.Empty;
            fakeTasks.Add(fakeParentTask);
            taskCollection.Add(fakeParentTask);

            var fakeChildWithSubChildren = GetFakeTask("Subtask With Children");
            fakeChildWithSubChildren.ParentTaskId = "Task With Children";
            taskCollection.Add(fakeChildWithSubChildren);

            var fakeChildWithoutSubChildren = GetFakeTask("Subtask Without Children");
            fakeChildWithoutSubChildren.ParentTaskId = "Task With Children";
            taskCollection.Add(fakeChildWithoutSubChildren);

            var fakeGrandChild = GetFakeTask("Subtask of Subtask");
            fakeGrandChild.ParentTaskId = "Subtask With Children";
            taskCollection.Add(fakeGrandChild);

            var fakeOrphan = GetFakeTask("Task with Missing Parent");
            fakeOrphan.ParentTaskId = "Non-existent Parent ID";
            fakeTasks.Add(fakeOrphan);
            taskCollection.Add(fakeOrphan);

            // Execute
            var treeBuilder = GetItemUnderTest();
            var actual = treeBuilder.AssembleTaskTrees(fakeTasks, taskCollection);

            // Verify
            Assert.AreEqual(2, actual.Count);

            var parentTask = actual.Single(task => task.Id.Equals("Task With Children"));
            Assert.AreEqual(2, parentTask.Subtasks.Count);

            var subTaskWithChildren = parentTask.Subtasks.Single(task => task.Id.Equals("Subtask With Children"));
            Assert.AreEqual(1, subTaskWithChildren.Subtasks.Count);

            var grandchild = subTaskWithChildren.Subtasks.Single(task => task.Id.Equals("Subtask of Subtask"));
            Assert.IsFalse(grandchild.Subtasks.Any());

            var subTaskWithoutChildren = parentTask.Subtasks.Single(task => task.Id.Equals("Subtask Without Children"));
            Assert.IsFalse(subTaskWithoutChildren.Subtasks.Any());

            var orphan = actual.Single(task => task.Id.Equals("Task with Missing Parent"));
            Assert.AreEqual(0, orphan.Subtasks.Count);
        }

        private TaskTreeBuilder GetItemUnderTest()
        {
            return new TaskTreeBuilder();
        }

        private IRtmTask GetFakeTask(string id)
        {
            return new RtmTask(id);
        }
    }
}