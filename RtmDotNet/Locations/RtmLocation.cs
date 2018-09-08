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

namespace RtmDotNet.Locations
{
    public class RtmLocation : IRtmLocation
    {
        public RtmLocation(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public int Zoom { get; set; }
        public bool IsViewable { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RtmLocation location && Equals(location);
        }

        public bool Equals(IRtmLocation other)
        {
            return other != null && string.Equals(Id, other.Id);
        }

        public int CompareTo(IRtmLocation other)
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
    }
}