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

namespace RtmDotNet.Http.Api.Lists
{
    public class GetListResponseData : RtmApiResponseData
    {
        [JsonProperty("lists")]
        public ListOfLists Lists { get; set; }

        public class ListOfLists
        {
            [JsonProperty("list")]
            public IList<ListData> Lists { get; set; }
        }

        public class ListData
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("locked")]
            [JsonConverter(typeof(RtmBooleanJsonConverter))]
            public bool IsLocked { get; set; }

            [JsonProperty("archived")]
            [JsonConverter(typeof(RtmBooleanJsonConverter))]
            public bool IsArchived { get; set; }

            [JsonProperty("smart")]
            [JsonConverter(typeof(RtmBooleanJsonConverter))]
            public bool IsSmart { get; set; }

            [JsonProperty("position")]
            public int Position { get; set; }

            [JsonProperty("sort_order")]
            public int SortOrder { get; set; }

            [JsonProperty("permission")]
            public string Permission { get; set; }

            [JsonProperty("filter")]
            public string Filter { get; set; }
        }
    }
}