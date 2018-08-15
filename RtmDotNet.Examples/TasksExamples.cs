// -----------------------------------------------------------------------
// <copyright file="TasksExamples.cs" author="Aaron Morris">
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RtmDotNet.Tasks;

namespace RtmDotNet.Examples
{
    public static class TasksExamples
    {
        public static async Task Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Task Examples - Select an option:");
                Console.WriteLine();
                Console.WriteLine("   1) List Tasks");
                Console.WriteLine("   2) List Orphaned Subtasks");
                Console.WriteLine();
                Console.WriteLine("   0) Back to Main Menu");
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine();
                Console.Write("--> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await ListAllTasks().ConfigureAwait(false);
                        break;

                    case "2":
                        await ListOrphanedSubTasks().ConfigureAwait(false);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid Entry");
                        break;
                }
            }
        }

        private static async Task ListAllTasks()
        {
            var tasks = await GetAllTasks().ConfigureAwait(false);

            foreach (var task in tasks)
            {
                WriteTask(task, 0);
            }

            Console.WriteLine($"Total Tasks: {tasks.Count}");
        }

        private static async Task ListOrphanedSubTasks()
        {
            var tasks = await GetAllTasks().ConfigureAwait(false);
            var orphanedSubtasks = tasks.Where(x => !string.IsNullOrEmpty(x.ParentTaskId)).ToList();
            foreach (var subtask in orphanedSubtasks)
            {
                WriteTask(subtask, 0);
            }

            Console.WriteLine($"Total Orphaned Subtasks: {orphanedSubtasks.Count}");
        }

        private static async Task<IList<IRtmTask>> GetAllTasks()
        {
            // Load a user from JSON
            var userJson = File.ReadAllText("myRtmUser.json");
            var user = Rtm.GetUserFactory().LoadFromJson(userJson);

            var taskRepository = Rtm.GetTaskRepository(user.Token);
            return await taskRepository.GetAllTasksAsync().ConfigureAwait(false);
        }

        private static void WriteTask(IRtmTask task, int indentLevel)
        {
            if (task.HasDueTime)
            {
                Console.WriteLine($"{new string('\t', indentLevel)} {task.Name} Due: {task.Due}");
            }
            else
            {
                Console.WriteLine($"{new string('\t', indentLevel)} {task.Name}");
            }

            foreach (var subtask in task.Subtasks)
            {
                WriteTask(subtask, indentLevel + 1);
            }
        }
    }
}