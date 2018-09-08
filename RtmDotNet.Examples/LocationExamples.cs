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
using RtmDotNet.Locations;

namespace RtmDotNet.Examples
{
    public class LocationExamples
    {
        private static readonly ILocationRepository LocationRepository = InitLocationRepository();

        public static async Task Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Location Examples - Select an option:");
                Console.WriteLine();
                Console.WriteLine("   1) Display All Locations");
                Console.WriteLine();
                Console.WriteLine("   0) Back to Main");
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine();
                Console.Write("--> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await DisplayAllLocations().ConfigureAwait(false);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid Entry");
                        break;
                }
            }
        }

        private static ILocationRepository InitLocationRepository()
        {
            // Load a user from JSON
            var userJson = File.ReadAllText("myRtmUser.json");
            var user = Rtm.GetUserFactory().LoadFromJson(userJson);

            return Rtm.GetLocationRepository(user.Token);
        }

        private static async Task<IList<IRtmLocation>> DisplayAllLocations()
        {
            // Download a list of all the user's locations in RTM.
            var locations = await LocationRepository.GetAllLocationsAsync().ConfigureAwait(false);

            WriteLocationsToConsole(locations);

            return locations;
        }

        private static void WriteLocationsToConsole(IList<IRtmLocation> locations)
        {
            // Display the list names, organized and sorted.
            var sortedLocations = locations.ToList().OrderBy(x => x.Name).ToList();

            Console.WriteLine();
            Console.WriteLine("Locations");
            Console.WriteLine("----------------------");
            foreach (var location in sortedLocations)
            {
                Console.WriteLine($"[ID: {location.Id}] {location.Name} - {location.Address}");
            }
        }
    }
}