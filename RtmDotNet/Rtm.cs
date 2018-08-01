// -----------------------------------------------------------------------
// <copyright file="Rtm.cs" author="Aaron Morris">
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
using RtmDotNet.Auth;
using RtmDotNet.Users;

namespace RtmDotNet
{
    public static class Rtm
    {
        public static string ApiKey { get; set; }

        public static string SharedSecret { get; set; }

        public static void Init(string apiKey, string sharedSecret)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            if (string.IsNullOrEmpty(sharedSecret))
            {
                throw new ArgumentNullException(nameof(sharedSecret));
            }

            ApiKey = apiKey;
            SharedSecret = sharedSecret;
        }

        public static IAuthFactory AuthFactory
        {
            get
            {
                EnforceInitialization();
                return new AuthFactory(ApiKey, SharedSecret);
            }
        }

        public static IRtmUserFactory UserFactory => new RtmUserFactory();

        private static void EnforceInitialization()
        {
            if (string.IsNullOrEmpty(ApiKey) || string.IsNullOrEmpty(SharedSecret))
            {
                throw new InvalidOperationException("You must first initialize the API with an API Key and Shared Secret before accessing its properties.");
            }
        }
    }
}