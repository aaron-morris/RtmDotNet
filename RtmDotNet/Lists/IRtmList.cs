﻿// -----------------------------------------------------------------------
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
    public interface IRtmList
    {
        string Id { get; set; }
        string Name { get; set; }
        bool IsLocked { get; set; }
        bool IsArchived { get; set; }
        bool IsSmart { get; set; }
        int Position { get; set; }
        int SortOrder { get; set; }
        string Permission { get; set; }
        string Filter { get; set; }
        Task<IList<IRtmTask>> GetTasksAsync();
    }
}