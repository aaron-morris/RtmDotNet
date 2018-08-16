// -----------------------------------------------------------------------
// <copyright file="RtmListTests.cs" author="Aaron Morris">
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
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Lists;
using RtmDotNet.Tasks;

namespace RtmDotNet.UnitTests.Lists
{
    [TestFixture]
    public class RtmListTests
    {
        [Test]
        public async Task GetTasksAsync_GetsTasksFromTaskRepo()
        {
            // Setup
            IList<IRtmTask> expectedTasks = new List<IRtmTask> {Substitute.For<IRtmTask>()};
            const string fakeListId = "My Fake List ID";

            var fakeTaskRepo = Substitute.For<ITaskRepository>();
            fakeTaskRepo.GetTasksByListIdAsync(fakeListId).Returns(Task.FromResult(expectedTasks));

            // Execute
            var list = GetItemUnderTest(fakeTaskRepo);
            list.Id = fakeListId;
            var actual = await list.GetTasksAsync().ConfigureAwait(false);

            // Verify
            CollectionAssert.AreEqual(expectedTasks, actual);
        }

        private RtmList GetItemUnderTest(ITaskRepository taskRepository)
        {
            return new RtmList(taskRepository);
        }
    }
}