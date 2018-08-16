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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RtmDotNet.Lists;

namespace RtmDotNet.Examples
{
    public static class ListExamples
    {
        public static async Task Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("List Examples - Select an option:");
                Console.WriteLine();
                Console.WriteLine("   1) Display All Lists");
                Console.WriteLine("   2) Display All Tasks from a List");
                Console.WriteLine();
                Console.WriteLine("   0) Exit");
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine();
                Console.Write("--> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await DisplayAllLists().ConfigureAwait(false);
                        break;

                    case "2":
                        await DisplayTasksFromList().ConfigureAwait(false);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid Entry");
                        break;
                }
            }
        }

        private static async Task<IList<IRtmList>> DisplayAllLists()
        {
            // Load a user from JSON
            var userJson = File.ReadAllText("myRtmUser.json");
            var user = Rtm.GetUserFactory().LoadFromJson(userJson);

            // Download a list of all the user's lists in RTM.
            var listRepository = Rtm.GetListRepository(user.Token);
            var lists = await listRepository.GetAllListsAsync().ConfigureAwait(false);

            WriteListsToConsole(lists);

            return lists;
        }

        private static async Task DisplayTasksFromList()
        {
            var lists = await DisplayAllLists().ConfigureAwait(false);

            Console.WriteLine();
            Console.Write("Enter the Name or List ID of the list for which you would like to print tasks --> ");
            var input = Console.ReadLine();

            var list = lists.SingleOrDefault(x => x.Id.Equals(input) || x.Name.Equals(input));

            if (list == null)
            {
                Console.WriteLine("Invalid Selection");
                return;
            }

            var tasks = await list.GetTasksAsync().ConfigureAwait(false);
            
            // Display the task names, sorted.
            var sortedTasks = tasks.ToList().OrderBy(x => x.Name).ToList();

            Console.WriteLine();
            Console.WriteLine($"Tasks in List '{list.Name}'");
            Console.WriteLine("----------------------");
            foreach (var task in sortedTasks)
            {
                Console.WriteLine(task.Name);
            }
        }

        private static void WriteListsToConsole(IList<IRtmList> lists)
        {
            // Display the list names, organized and sorted.
            var sortedLists = lists.ToList().OrderBy(x => x.Position).ThenBy(x => x.Name).ToList();

            Console.WriteLine();
            Console.WriteLine("System Lists");
            Console.WriteLine("----------------------");
            foreach (var systemList in sortedLists.Where(x => x.IsLocked))
            {
                Console.WriteLine($"[ID: {systemList.Id}] {systemList.Name}");
            }

            Console.WriteLine();
            Console.WriteLine("User Lists");
            Console.WriteLine("----------------------");
            foreach (var userList in sortedLists.Where(x => !x.IsLocked && !x.IsArchived && !x.IsSmart))
            {
                Console.WriteLine($"[ID: {userList.Id}] {userList.Name}");
            }

            Console.WriteLine();
            Console.WriteLine("Smart Lists");
            Console.WriteLine("----------------------");
            foreach (var smartList in sortedLists.Where(x => x.IsSmart))
            {
                Console.WriteLine($"[ID: {smartList.Id}] {smartList.Name}");
            }

            Console.WriteLine();
            Console.WriteLine("Archived Lists");
            Console.WriteLine("----------------------");
            foreach (var archivedList in sortedLists.Where(x => x.IsArchived))
            {
                Console.WriteLine($"[ID: {archivedList.Id}] {archivedList.Name}");
            }
        }
    }
}