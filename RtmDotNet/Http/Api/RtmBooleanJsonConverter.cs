// -----------------------------------------------------------------------
// <copyright file="RtmBooleanJsonConverter.cs" author="Aaron Morris">
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

namespace RtmDotNet.Http.Api
{
    public class RtmBooleanJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var boolValue = (bool) value;

            if (boolValue)
            {
                writer.WriteValue("1");
            }
            else
            {
                writer.WriteValue("0");
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().ToLowerInvariant().Trim();

            if (string.IsNullOrEmpty(value) || value.Equals("0"))
            {
                return false;
            }

            if (value.Equals("1"))
            {
                return true;
            }

            throw new FormatException($"The following value was unexpected by the RTM Boolean value converter: {value}");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}