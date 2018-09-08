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
using System.Linq;
using RtmDotNet.Locations;

namespace RtmDotNet.Http.Api.Locations
{
    public class ResponseParser : IResponseParser
    {
        public IList<IRtmLocation> GetLocations(GetListResponseData responseData)
        {
            if (responseData?.Locations?.Location == null)
            {
                return new List<IRtmLocation>();
            }

            return responseData.Locations.Location.Select(locationData => new RtmLocation(locationData.Id)
                {
                    Name = locationData.Name,
                    Latitude = locationData.Latitude,
                    Longitude = locationData.Longitude,
                    Address = locationData.Address,
                    Zoom = locationData.Zoom,
                    IsViewable = locationData.IsViewable
                })
                .Cast<IRtmLocation>()
                .ToList();
        }
    }
}