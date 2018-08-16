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
using System.Threading.Tasks;
using RtmDotNet.Tasks;

namespace RtmDotNet.Lists
{
    public class RtmList : IRtmList
    {
        private readonly ITaskRepository _taskRepository;

        public RtmList(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsLocked { get; set; }

        public bool IsArchived { get; set; }

        public bool IsSmart { get; set; }

        public int Position { get; set; }

        public int SortOrder { get; set; }

        public string Permission { get; set; }

        public string Filter { get; set; }

        public async Task<IList<IRtmTask>> GetTasksAsync()
        {
            return await _taskRepository.GetTasksByListIdAsync(Id).ConfigureAwait(false);
        }
    }
}