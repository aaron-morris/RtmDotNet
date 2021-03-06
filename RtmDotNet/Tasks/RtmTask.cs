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
using System;
using System.Collections.Generic;
using RtmDotNet.Locations;

namespace RtmDotNet.Tasks
{
    public class RtmTask : IRtmTask
    {
        public RtmTask(string id)
        {
            Id = id;

            Tags = new List<string>();
            Notes = new List<ITaskNote>();
            Subtasks = new List<IRtmTask>();
        }

        public string Id { get; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public DateTime? Due { get; set; }

        public bool HasDueTime { get; set; }

        public DateTime Added { get; set; }

        public DateTime? Completed { get; set; }

        public DateTime? Deleted { get; set; }

        public DateTime? Start { get; set; }

        public bool HasStartTime { get; set; }

        public string Source { get; set; }

        public string Url { get; set; }

        public IRtmLocation Location { get; set; }

        public string ParentTaskId { get; set; }

        public string SeriesId { get; set; }

        public string ListId { get; set; }

        public IList<string> Tags { get; set; }

        public IList<ITaskNote> Notes { get; set; }

        public string Priority { get; set; }

        public int Postponed { get; set; }

        public string Estimate { get; set; }

        public IList<IRtmTask> Subtasks { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RtmTask task && Equals(task);
        }

        public bool Equals(IRtmTask other)
        {
            return other != null && string.Equals(Id, other.Id);
        }

        public int CompareTo(IRtmTask other)
        {
            if (other == null)
            {
                return 1;
            }

            return string.Compare(Id, other.Id, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Id != null ? Id.GetHashCode() : 0;
        }

        public class Note : ITaskNote
        {
            public string Id { get; set; }

            public DateTime Created { get; set; }

            public DateTime Modified { get; set; }

            public string Title { get; set; }

            public string Text { get; set; }
        }
    }
}