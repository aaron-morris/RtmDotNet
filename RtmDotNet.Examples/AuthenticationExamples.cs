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
using System.IO;
using System.Threading.Tasks;
using RtmDotNet.Auth;

namespace RtmDotNet.Examples
{
    public static class AuthenticationExamples
    {
        public static async Task Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine("Authentication Examples - Enter number option or enter 'x' to return to main menu:");
                Console.WriteLine();
                Console.WriteLine("   1) Authenticate New Desktop User");
                Console.WriteLine("   2) Check Saved User Token");
                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine();
                Console.Write("--> ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await AuthenticateNewDesktopUser().ConfigureAwait(false);
                        break;

                    case "2":
                        await CheckAuthenticationToken().ConfigureAwait(false);
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

        private static async Task AuthenticateNewDesktopUser()
        {
            // Get an authentication URL for the user.  This example will request authorization for READ permissions.
            try
            {
                var desktopAuthenticator = Rtm.GetAuthFactory().CreateDesktopAuthenticator();
                var authUrl = await desktopAuthenticator.GetAuthenticationUrlAsync(PermissionLevel.Read).ConfigureAwait(false);

                // Instruct the user to navigate to the URL and authorize your application.  They can continue using
                // your application after authentication in the web browser is complete.
                Console.WriteLine($"Step 1:  Please navigate to the following URL using a web browser: {authUrl}");
                Console.WriteLine("Step 2:  Follow the instructions from Remember the Milk to authorize this application to access your RTM account.");
                Console.WriteLine("Step 3: Press [Enter] when finished");
                Console.ReadLine();

                var rtmUser = await desktopAuthenticator.GetAutheticatedUserAsync().ConfigureAwait(false);

                // Save the user data to a file or database.
                var userJson = rtmUser.ToJson();
                File.WriteAllText("myRtmUser.json", userJson);
            }
            catch (RtmException ex)
            {
                // Read the exception details.  There may be an issue with your API Key or Shared Secret, or the user may not
                // have followed the instructions for authentication.
                Console.WriteLine($"RTM Error Code: {ex.ErrorCode} -- RTM Error Message: {ex.Message}");
            }
        }

        private static async Task CheckAuthenticationToken()
        {
            // Load a user from JSON
            var userJson = File.ReadAllText("myRtmUser.json");
            var user = Rtm.GetUserFactory().LoadFromJson(userJson);

            var tokenVerifier = Rtm.GetAuthFactory().CreateTokenVerifier();

            try
            {
                if (await tokenVerifier.VerifyAsync(user.Token).ConfigureAwait(false))
                {
                    Console.WriteLine("Token is still valid.");
                }
                else
                {
                    // Token is expired or otherwise invalid.  you'll need to create a new authorization URL
                    // and instruct the user to reauthorize your app.
                    Console.WriteLine("Token is expired or invalid.");
                }
            }
            catch (RtmException ex)
            {
                // Some other API error was returned.
                Console.WriteLine($"RTM Error Code: {ex.ErrorCode}");
                Console.WriteLine($"RTM Error Message: {ex.Message}");
            }
        }
    }
}