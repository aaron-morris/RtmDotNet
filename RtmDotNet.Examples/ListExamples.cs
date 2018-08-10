// -----------------------------------------------------------------------
// <copyright file="ListExamples.cs" author="Aaron Morris">
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                Console.WriteLine("List Examples - Enter number option or enter 'x' to return to main menu:");
                Console.WriteLine();
                Console.WriteLine("   1) Display All Lists");
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine();
                Console.Write("--> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await DisplayAllLists();
                        break;

                    case "x":
                    case "X":
                        return;

                    default:
                        Console.WriteLine("Invalid Entry");
                        break;
                }
            }
        }

        private static async Task DisplayAllLists()
        {
            // Load a user from JSON
            var userJson = File.ReadAllText("myRtmUser.json");
            var user = Rtm.GetUserFactory().LoadFromJson(userJson);

            // Download a list of all the user's lists in RTM.
            var listRepository = Rtm.GetListRepository(user.Token);
            var lists = await listRepository.GetAllListsAsync();

            // Display the list names, organized and sorted.
            var sortedLists = lists.ToList().OrderBy(x => x.Position).ThenBy(x => x.Name).ToList();

            Console.WriteLine();
            Console.WriteLine("System Lists");
            Console.WriteLine("----------------------");
            foreach (var systemList in sortedLists.Where(x => x.IsLocked))
            {
                Console.WriteLine(systemList.Name);
            }

            Console.WriteLine();
            Console.WriteLine("User Lists");
            Console.WriteLine("----------------------");
            foreach (var userList in sortedLists.Where(x => !x.IsLocked && !x.IsArchived && !x.IsSmart))
            {
                Console.WriteLine(userList.Name);
            }

            Console.WriteLine();
            Console.WriteLine("Smart Lists");
            Console.WriteLine("----------------------");
            foreach (var smartList in sortedLists.Where(x => x.IsSmart))
            {
                Console.WriteLine(smartList.Name);
            }

            Console.WriteLine();
            Console.WriteLine("Archived Lists");
            Console.WriteLine("----------------------");
            foreach (var archivedList in sortedLists.Where(x => x.IsArchived))
            {
                Console.WriteLine(archivedList.Name);
            }
        }
    }
}