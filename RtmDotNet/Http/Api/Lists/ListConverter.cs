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
using RtmDotNet.Lists;
using RtmDotNet.Tasks;

namespace RtmDotNet.Http.Api.Lists
{
    public class ListConverter : IListConverter
    {
        private readonly ITaskRepository _taskRepository;

        public ListConverter(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IList<IRtmList> ConvertToLists(GetListResponseData responseData)
        {
            return responseData.Lists.Lists.Select(listData => new RtmList(_taskRepository)
                {
                    Id = listData.Id,
                    Name = listData.Name,
                    Filter = listData.Filter,
                    IsArchived = listData.IsArchived,
                    IsLocked = listData.IsLocked,
                    IsSmart = listData.IsSmart,
                    Permission = listData.Permission,
                    Position = listData.Position,
                    SortOrder = listData.SortOrder
                })
                .Cast<IRtmList>()
                .ToList();
        }
    }
}