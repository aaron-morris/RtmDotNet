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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RtmDotNet.Locations
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ILocationApiClient _apiClient;
        private readonly ILocationCache _locationCache;

        public LocationRepository(ILocationApiClient apiClient, ILocationCache locationCache)
        {
            _apiClient = apiClient;
            _locationCache = locationCache;
        }

        public async Task<IList<IRtmLocation>> GetAllLocationsAsync()
        {
            await RefreshLocationCache().ConfigureAwait(false);

            // Return a copy of the internal cache so that it cannot be externally modified.
            return await _locationCache.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<IRtmLocation> GetLocationByIdAsync(string locationId)
        {
            if (string.IsNullOrEmpty(locationId))
            {
                throw new ArgumentNullException(nameof(locationId));
            }

            var location = await _locationCache.GetByIdAsync(locationId).ConfigureAwait(false);

            if (location == null)
            {
                await RefreshLocationCache().ConfigureAwait(false);
                location = await _locationCache.GetByIdAsync(locationId).ConfigureAwait(false);

                if (location == null)
                {
                    throw new InvalidOperationException($"The specified location ID does not exist.  ID = {locationId}");
                }
            }

            return location;
        }

        private async Task RefreshLocationCache()
        {
            await _locationCache.ClearAsync().ConfigureAwait(false);
            var locations = await _apiClient.GetAllLocationsAsync().ConfigureAwait(false);
            await _locationCache.AddOrReplaceAsync(locations).ConfigureAwait(false);
        }
    }
}