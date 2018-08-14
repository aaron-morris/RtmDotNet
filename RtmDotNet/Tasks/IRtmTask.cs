// -----------------------------------------------------------------------
// <copyright file="IRtmTask.cs" author="Aaron Morris">
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

namespace RtmDotNet.Tasks
{
    public interface IRtmTask
    {
        string Id { get; set; }
        string Name { get; set; }
        DateTime Created { get; set; }
        DateTime Modified { get; set; }
        DateTime? Due { get; set; }
        bool HasDueTime { get; set; }
        DateTime Added { get; set; }
        DateTime? Completed { get; set; }
        DateTime? Deleted { get; set; }
        DateTime? Start { get; set; }
        bool HasStartTime { get; set; }
        string Source { get; set; }
        string Url { get; set; }
        string LocationId { get; set; }
        string ParentTaskId { get; set; }
        string SeriesId { get; set; }
        string ListId { get; set; }
        IList<string> Tags { get; set; }
        IList<ITaskNote> Notes { get; set; }
        string Priority { get; set; }
        int Postponed { get; set; }
        string Estimate { get; set; }
        IList<IRtmTask> Subtasks { get; set; }
    }

    public interface ITaskNote
    {
        string Id { get; set; }

        DateTime Created { get; set; }

        DateTime Modified { get; set; }

        string Title { get; set; }

        string Text { get; set; }
    }
}