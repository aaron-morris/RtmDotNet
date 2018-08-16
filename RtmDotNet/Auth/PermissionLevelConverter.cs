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

namespace RtmDotNet.Auth
{
    public class PermissionLevelConverter : IPermissionLevelConverter
    {
        public PermissionLevel ToEnum(string permissionName)
        {
            if (string.IsNullOrEmpty(permissionName))
            {
                return PermissionLevel.Undefined;
            }

            if (permissionName.Equals(PermissionNames.Read, StringComparison.InvariantCultureIgnoreCase))
            {
                return PermissionLevel.Read;
            }

            if (permissionName.Equals(PermissionNames.Write, StringComparison.InvariantCultureIgnoreCase))
            {
                return PermissionLevel.Write;
            }

            if (permissionName.Equals(PermissionNames.Delete, StringComparison.InvariantCultureIgnoreCase))
            {
                return PermissionLevel.Delete;
            }

            throw new FormatException($"An unrecognized RTM permission value was found.  Found: {permissionName}");
        }

        public string ToString(PermissionLevel permissionLevel)
        {
            switch (permissionLevel)
            {
                case PermissionLevel.Undefined:
                    return string.Empty;

                case PermissionLevel.Read:
                    return PermissionNames.Read;

                case PermissionLevel.Write:
                    return PermissionNames.Write;

                case PermissionLevel.Delete:
                    return PermissionNames.Delete;

                default:
                    throw new NotImplementedException($"The following RTM permission level is not recognized: {Enum.GetName(typeof(PermissionLevel), permissionLevel)}");
            }
        }
    }
}