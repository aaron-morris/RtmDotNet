// -----------------------------------------------------------------------
// <copyright file="Program.cs" author="Aaron Morris">
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
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RtmDotNet.Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var applicationTask = Run();
            applicationTask.Wait();
        }

        private static async Task Run()
        {
            // Initialize the API with your personal API Key and Shared Secret issued by Remember the Milk.  Do this once per runtime session.
            var configs = LoadApiConfigs();
            Rtm.Init(configs.ApiKey, configs.SharedSecret);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Main Menu - Enter number option or enter 'x' to quit:");
                Console.WriteLine();
                Console.WriteLine("   1) Authentication Examples");
                Console.WriteLine("   2) List Examples");
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine();
                Console.Write("--> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await AuthenticationExamples.Run();
                        break;

                    case "2":
                        await ListExamples.Run();
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

        private static RtmApiConfigs LoadApiConfigs()
        {
            // Loading my personal API Key and Shared Secret from a configuration file.
            var configsJson = File.ReadAllText("rtmConfigs.json");
            return JsonConvert.DeserializeObject<RtmApiConfigs>(configsJson);
        }
    }
}