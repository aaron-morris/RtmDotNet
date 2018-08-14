// -----------------------------------------------------------------------
// <copyright file="RtmArrayJsonConverter.cs" author="Aaron Morris">
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RtmDotNet.Http.Api.Tasks
{
    /// <summary>
    /// A JSON converter for RTM array properties.
    /// </summary>
    /// <typeparam name="T">The expected data type of elements within the RTM array.</typeparam>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    /// <remarks>
    /// The RTM API has a querky behavior in which array properties (e.g.: task.Tags, task.Notes) are sometimes formatted
    /// as objects and sometimes formatted as arrays:
    /// 
    ///    - When the property is an empty list, it is formatted as an empty JSON array.
    ///       for example:  "tags": [] 
    ///    - When the property is not an empty list, it is formatted as a JSON object with an array sub-property
    ///       for example:  "tags": { "tag": ["myTag1", "myTag2", "etc."] }
    ///
    /// This converter will detect whether the property is JSON object or a JSON array.  For a JSON object, it searches
    /// for the actual array in the object sub-properties.  For a JSON array, it parses as an array.
    /// </remarks>
    public class RtmArrayJsonConverter<T> : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Serialization is not supported.  Use a standard serializer.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    return ParseAsObject(reader);

                case JsonToken.StartArray:
                    return ParseAsArray(reader);

                default:
                    throw new FormatException("An unexpected JSON token was encountered while parsing an array property of an RTM response.");
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IList<T>);
        }

        private IList<T> ParseAsObject(JsonReader reader)
        {
            var jsonObject = JObject.Load(reader);

            // The actual array will be a property of this object.  Find the find the first array property and parse it to a list.
            // Note:  In real-world data, there should only be one property, and it should always be an array.
            foreach (var element in jsonObject)
            {
                if (element.Value.Type == JTokenType.Array)
                {
                    var arrayReader = element.Value.CreateReader();
                    var array = JArray.Load(arrayReader);
                    return array.ToObject<List<T>>();
                }
            }

            throw new FormatException($"No array element was found within this JSON property: {jsonObject.Path}");
        }

        private IList<T> ParseAsArray(JsonReader reader)
        {
            var jsonArray = JArray.Load(reader);
            return jsonArray.ToObject<List<T>>();
        }
    }
}