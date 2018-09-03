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
using System.Collections.Concurrent;

namespace RtmDotNet.Tasks
{
    public class InMemorySyncTracker : ISyncTracker
    {
        private readonly ConcurrentDictionary<string, DateTime> _syncTimes;

        public InMemorySyncTracker()
        {
            _syncTimes = new ConcurrentDictionary<string, DateTime>();
        }

        public void SetLastSync(string listId, DateTime lastSync)
        {
            _syncTimes[listId] = lastSync;
        }

        public DateTime? GetLastSync(string listId)
        {
            if (listId == null || !_syncTimes.ContainsKey(listId))
            {
                return null;
            }

            return _syncTimes[listId];
        }
    }
}