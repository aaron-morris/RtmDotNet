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
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RtmDotNet.Http.Api.Locations
{
    public class GetListResponseData : ApiResponseData
    {
        [JsonProperty("locations")]
        public ListOfLocations Locations { get; set; }

        public class ListOfLocations
        {
            [JsonProperty("location")]
            public IList<LocationData> Location { get; set; }
        }

        public class LocationData
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("longitude")]
            public double Longitude { get; set; }

            [JsonProperty("latitude")]
            public double Latitude { get; set; }

            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("zoom")]
            public int Zoom { get; set; }

            [JsonProperty("viewable")]
            [JsonConverter(typeof(RtmBooleanJsonConverter))]
            public bool IsViewable { get; set; }
        }
    }
}