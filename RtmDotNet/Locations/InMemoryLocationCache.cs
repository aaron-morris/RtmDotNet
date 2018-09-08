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
using System.Threading.Tasks;

namespace RtmDotNet.Locations
{
    public class InMemoryLocationCache : ILocationCache
    {
        private readonly List<IRtmLocation> _cachedLocations;

        private readonly object _lock;

        public InMemoryLocationCache()
        {
            _cachedLocations = new List<IRtmLocation>();
            _lock = new object();
        }

        public Task AddOrReplaceAsync(IEnumerable<IRtmLocation> locations)
        {
            lock (_lock)
            {
                foreach (var location in locations)
                {
                    if (_cachedLocations.Contains(location))
                    {
                        _cachedLocations.Remove(location);
                    }

                    _cachedLocations.Add(location);
                } 
            }

            return Task.CompletedTask;
        }

        public Task<IList<IRtmLocation>> GetAllAsync()
        {
            IList<IRtmLocation> locations;

            lock (_lock)
            {
                locations = _cachedLocations.ToList();
            }

            return Task.FromResult(locations);
        }

        public Task<IRtmLocation> GetByIdAsync(string locationId)
        {
            IRtmLocation location;

            lock (_lock)
            {
                location = _cachedLocations.FirstOrDefault(x => x.Id.Equals(locationId));
            }

            return Task.FromResult(location);
        }

        public Task ClearAsync()
        {
            lock (_lock)
            {
                _cachedLocations.Clear();
            }

            return Task.CompletedTask;
        }
    }
}