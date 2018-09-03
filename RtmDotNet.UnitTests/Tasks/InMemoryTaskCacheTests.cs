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
using RtmDotNet.Tasks;

namespace RtmDotNet.UnitTests.Tasks
{
    using NUnit.Framework;

    [TestFixture]
    public class InMemoryTaskCacheTests
    {
        [Test]
        public async Task Roundtrip_EmptyCache_AddsTasksNew()
        {
            // Setup
            var fakeTask1 = GetFakeTask("Task 1");
            var fakeTask2 = GetFakeTask("Task 2");
            var fakeTask3 = GetFakeTask("Task 3");
            var tasks = new List<IRtmTask> {fakeTask1, fakeTask2, fakeTask3};

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(tasks).ConfigureAwait(false);
            var actual = await taskCache.GetAllAsync().ConfigureAwait(false);

            // Verify
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains(fakeTask1));
            Assert.IsTrue(actual.Contains(fakeTask2));
            Assert.IsTrue(actual.Contains(fakeTask3));
        }

        [Test]
        public async Task Roundtrip_ExistingEntries_UpdatesExisting()
        {
            // Setup
            var fakeTask1 = GetFakeTask("Task 1", "My Task 1");
            var fakeTask2 = GetFakeTask("Task 2", "My Task 2");
            var fakeTask3 = GetFakeTask("Task 3", "My Task 3");
            var taskBatch1 = new List<IRtmTask> { fakeTask1, fakeTask2 };
            var taskBatch2 = new List<IRtmTask> {fakeTask3, fakeTask2};

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(taskBatch1).ConfigureAwait(false);
            fakeTask2.Name = "My Task 2 - Updated";
            await taskCache.AddOrReplaceAsync(taskBatch2).ConfigureAwait(false);
            var actual = await taskCache.GetAllAsync().ConfigureAwait(false);

            // Verify
            Assert.AreEqual(3, actual.Count);

            var actual1 = actual.First(x => x.Id.Equals("Task 1"));
            Assert.AreEqual("My Task 1", actual1.Name);

            var actual2 = actual.First(x => x.Id.Equals("Task 2"));
            Assert.AreEqual("My Task 2 - Updated", actual2.Name);

            var actual3 = actual.First(x => x.Id.Equals("Task 3"));
            Assert.AreEqual("My Task 3", actual3.Name);
        }

        [Test]
        public async Task RoundtripWithListId_EmptyCache_AddsTasksNew()
        {
            // Setup
            const string fakeListId = "List 1";
            var fakeTask1 = GetFakeTask("Task 1");
            var fakeTask2 = GetFakeTask("Task 2");
            var fakeTask3 = GetFakeTask("Task 3");
            var tasks = new List<IRtmTask> { fakeTask1, fakeTask2, fakeTask3 };

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(fakeListId, tasks).ConfigureAwait(false);
            var actual = await taskCache.GetAllAsync(fakeListId).ConfigureAwait(false);

            // Verify
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains(fakeTask1));
            Assert.IsTrue(actual.Contains(fakeTask2));
            Assert.IsTrue(actual.Contains(fakeTask3));
        }

        [Test]
        public async Task RoundtripWithListId_ExistingEntries_UpdatesExisting()
        {
            // Setup
            const string fakeListId = "List 1";
            var fakeTask1 = GetFakeTask("Task 1", "My Task 1");
            var fakeTask2 = GetFakeTask("Task 2", "My Task 2");
            var fakeTask3 = GetFakeTask("Task 3", "My Task 3");
            var taskBatch1 = new List<IRtmTask> { fakeTask1, fakeTask2 };
            var taskBatch2 = new List<IRtmTask> { fakeTask3, fakeTask2 };

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(fakeListId, taskBatch1).ConfigureAwait(false);
            fakeTask2.Name = "My Task 2 - Updated";
            await taskCache.AddOrReplaceAsync(fakeListId, taskBatch2).ConfigureAwait(false);
            var actual = await taskCache.GetAllAsync(fakeListId).ConfigureAwait(false);

            // Verify
            Assert.AreEqual(3, actual.Count);

            var actual1 = actual.First(x => x.Id.Equals("Task 1"));
            Assert.AreEqual("My Task 1", actual1.Name);

            var actual2 = actual.First(x => x.Id.Equals("Task 2"));
            Assert.AreEqual("My Task 2 - Updated", actual2.Name);

            var actual3 = actual.First(x => x.Id.Equals("Task 3"));
            Assert.AreEqual("My Task 3", actual3.Name);
        }

        [Test]
        public async Task RoundtripWithListId_ExistingEntriesWithOverwriteList_ClearsExisitngListDefinition()
        {
            // Setup
            const string fakeListId = "List 1";
            var fakeTask1 = GetFakeTask("Task 1", "My Task 1");
            var fakeTask2 = GetFakeTask("Task 2", "My Task 2");
            var fakeTask3 = GetFakeTask("Task 3", "My Task 3");
            var taskBatch1 = new List<IRtmTask> { fakeTask1, fakeTask2 };
            var taskBatch2 = new List<IRtmTask> { fakeTask3, fakeTask2 };

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(fakeListId, taskBatch1, true).ConfigureAwait(false);
            fakeTask2.Name = "My Task 2 - Updated";
            await taskCache.AddOrReplaceAsync(fakeListId, taskBatch2, true).ConfigureAwait(false);
            var actual = await taskCache.GetAllAsync(fakeListId).ConfigureAwait(false);

            // Verify
            Assert.AreEqual(2, actual.Count);

            var actual2 = actual.First(x => x.Id.Equals("Task 2"));
            Assert.AreEqual("My Task 2 - Updated", actual2.Name);

            var actual3 = actual.First(x => x.Id.Equals("Task 3"));
            Assert.AreEqual("My Task 3", actual3.Name);
        }

        [Test]
        public async Task RoundtripWithListId_TwoLists_SegregatesListsWhileMaintainingSharedTaskCache()
        {
            // Setup
            const string fakeListId1 = "List 1";
            const string fakeListId2 = "List 2";
            var fakeTask1 = GetFakeTask("Task 1", "My Task 1");
            var fakeTask2 = GetFakeTask("Task 2", "My Task 2");
            var fakeTask3 = GetFakeTask("Task 3", "My Task 3");
            var taskBatch1 = new List<IRtmTask> { fakeTask1, fakeTask2 };
            var taskBatch2 = new List<IRtmTask> { fakeTask3, fakeTask2 };

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(fakeListId1, taskBatch1).ConfigureAwait(false);
            fakeTask2.Name = "My Task 2 - Updated";
            await taskCache.AddOrReplaceAsync(fakeListId2, taskBatch2).ConfigureAwait(false);
            var actualList1 = await taskCache.GetAllAsync(fakeListId1).ConfigureAwait(false);
            var actualList2 = await taskCache.GetAllAsync(fakeListId2).ConfigureAwait(false);

            // Verify
            Assert.AreEqual(2, actualList1.Count);
            Assert.AreEqual(2, actualList2.Count);

            var actualTask1 = actualList1.First(x => x.Id.Equals("Task 1"));
            Assert.AreEqual("My Task 1", actualTask1.Name);

            var actualTask2A = actualList1.First(x => x.Id.Equals("Task 2"));
            Assert.AreEqual("My Task 2 - Updated", actualTask2A.Name);

            var actualTask2B = actualList2.First(x => x.Id.Equals("Task 2"));
            Assert.AreEqual("My Task 2 - Updated", actualTask2B.Name);

            var actualTask3 = actualList2.First(x => x.Id.Equals("Task 3"));
            Assert.AreEqual("My Task 3", actualTask3.Name);
        }

        [Test]
        public async Task RoundtripWithListId_TaskRemoved_RemovesFromCacheAndAllLists()
        {
            // Setup
            const string fakeListId1 = "List 1";
            const string fakeListId2 = "List 2";
            var fakeTask1 = GetFakeTask("Task 1", "My Task 1");
            var fakeTask2 = GetFakeTask("Task 2", "My Task 2");
            var fakeTask3 = GetFakeTask("Task 3", "My Task 3");
            var taskBatch1 = new List<IRtmTask> { fakeTask1, fakeTask2 };
            var taskBatch2 = new List<IRtmTask> { fakeTask3, fakeTask2 };

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(fakeListId1, taskBatch1).ConfigureAwait(false);
            fakeTask2.Name = "My Task 2 - Updated";
            await taskCache.AddOrReplaceAsync(fakeListId2, taskBatch2).ConfigureAwait(false);
            await taskCache.RemoveAsync(new List<IRtmTask> {fakeTask2}).ConfigureAwait(false);
            var actualList1 = await taskCache.GetAllAsync(fakeListId1).ConfigureAwait(false);
            var actualList2 = await taskCache.GetAllAsync(fakeListId2).ConfigureAwait(false);

            // Verify
            Assert.AreEqual(1, actualList1.Count);
            Assert.AreEqual(1, actualList2.Count);

            var actualTask1 = actualList1.First(x => x.Id.Equals("Task 1"));
            Assert.AreEqual("My Task 1", actualTask1.Name);

            var actualTask3 = actualList2.First(x => x.Id.Equals("Task 3"));
            Assert.AreEqual("My Task 3", actualTask3.Name);
        }

        [Test]
        public async Task RoundtripWithListId_TaskRemovedAndReaddedToCache_RemainsRemovedFromLists()
        {
            // Setup
            const string fakeListId1 = "List 1";
            const string fakeListId2 = "List 2";
            var fakeTask1 = GetFakeTask("Task 1", "My Task 1");
            var fakeTask2 = GetFakeTask("Task 2", "My Task 2");
            var fakeTask3 = GetFakeTask("Task 3", "My Task 3");
            var taskBatch1 = new List<IRtmTask> { fakeTask1, fakeTask2 };
            var taskBatch2 = new List<IRtmTask> { fakeTask3, fakeTask2 };
            var taskBatch3 = new List<IRtmTask> {fakeTask2};

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(fakeListId1, taskBatch1).ConfigureAwait(false);
            fakeTask2.Name = "My Task 2 - Updated";
            await taskCache.AddOrReplaceAsync(fakeListId2, taskBatch2).ConfigureAwait(false);
            await taskCache.RemoveAsync(new List<IRtmTask> { fakeTask2 }).ConfigureAwait(false);
            await taskCache.AddOrReplaceAsync(taskBatch3).ConfigureAwait(false);
            var actualList1 = await taskCache.GetAllAsync(fakeListId1).ConfigureAwait(false);
            var actualList2 = await taskCache.GetAllAsync(fakeListId2).ConfigureAwait(false);

            // Verify
            Assert.AreEqual(1, actualList1.Count);
            Assert.AreEqual(1, actualList2.Count);

            var actualTask1 = actualList1.First(x => x.Id.Equals("Task 1"));
            Assert.AreEqual("My Task 1", actualTask1.Name);

            var actualTask3 = actualList2.First(x => x.Id.Equals("Task 3"));
            Assert.AreEqual("My Task 3", actualTask3.Name);
        }

        [Test]
        public async Task GetTasksAsync_ListIdNotFound_ThrowsInvalidOperationException()
        {
            // Setup
            const string fakeListId = "List 1";
            var fakeTask1 = GetFakeTask("Task 1");
            var fakeTask2 = GetFakeTask("Task 2");
            var fakeTask3 = GetFakeTask("Task 3");
            var tasks = new List<IRtmTask> { fakeTask1, fakeTask2, fakeTask3 };

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(fakeListId, tasks).ConfigureAwait(false);

            Assert.ThrowsAsync<InvalidOperationException>(() => taskCache.GetAllAsync("List 2"));
        }

        [Test]
        public async Task ClearTaskAsync_CacheWithLists_ClearsListsAndTasks()
        {
            // Setup
            const string fakeListId1 = "List 1";
            const string fakeListId2 = "List 2";
            var fakeTask1 = GetFakeTask("Task 1", "My Task 1");
            var fakeTask2 = GetFakeTask("Task 2", "My Task 2");
            var fakeTask3 = GetFakeTask("Task 3", "My Task 3");
            var taskBatch1 = new List<IRtmTask> { fakeTask1, fakeTask2 };
            var taskBatch2 = new List<IRtmTask> { fakeTask3, fakeTask2 };

            // Execute
            var taskCache = GetItemUnderTest();
            await taskCache.AddOrReplaceAsync(fakeListId1, taskBatch1).ConfigureAwait(false);
            fakeTask2.Name = "My Task 2 - Updated";
            await taskCache.AddOrReplaceAsync(fakeListId2, taskBatch2).ConfigureAwait(false);
            await taskCache.ClearAsync().ConfigureAwait(false);

            // Verify
            var tasks = await taskCache.GetAllAsync().ConfigureAwait(false);
            Assert.AreEqual(0, tasks.Count);
            Assert.ThrowsAsync<InvalidOperationException>(() => taskCache.GetAllAsync(fakeListId1));
            Assert.ThrowsAsync<InvalidOperationException>(() => taskCache.GetAllAsync(fakeListId2));
        }

        private InMemoryTaskCache GetItemUnderTest()
        {
            return GetItemUnderTest(new FakeTreeBuilder());
        }

        private InMemoryTaskCache GetItemUnderTest(ITaskTreeBuilder taskTreeBuilder)
        {
            return new InMemoryTaskCache(taskTreeBuilder);
        }

        private IRtmTask GetFakeTask(string taskId)
        {
            return new RtmTask(taskId);
        }

        private IRtmTask GetFakeTask(string taskId, string name)
        {
            return new RtmTask(taskId) {Name = name};
        }

        private class FakeTreeBuilder : ITaskTreeBuilder
        {
            public IList<IRtmTask> AssembleTaskTrees(ICollection<IRtmTask> tasks)
            {
                return tasks.ToList();
            }

            public IList<IRtmTask> AssembleTaskTrees(IEnumerable<IRtmTask> rootTasks, ICollection<IRtmTask> allTasks)
            {
                return rootTasks.ToList();
            }
        }
    }
}
