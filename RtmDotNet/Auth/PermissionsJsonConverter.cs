﻿// <copyright file="PermissionsJsonConverter.cs" author="Aaron Morris">
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
using Newtonsoft.Json;

namespace RtmDotNet.Auth
{
    public class PermissionsJsonConverter : JsonConverter
    {
        private readonly IPermissionLevelConverter _permissionLevelConverter;

        // CTOR used by Json.Net
        public PermissionsJsonConverter()
        {
            _permissionLevelConverter = new PermissionLevelConverter();
        }

        // CTOR for unit testing
        public PermissionsJsonConverter(IPermissionLevelConverter permissionLevelConverter)
        {
            _permissionLevelConverter = permissionLevelConverter;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var permissionLevel = (PermissionLevel) value;
            writer.WriteValue(_permissionLevelConverter.ToString(permissionLevel));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().Trim();
            return _permissionLevelConverter.ToEnum(value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PermissionLevel);
        }
    }
}