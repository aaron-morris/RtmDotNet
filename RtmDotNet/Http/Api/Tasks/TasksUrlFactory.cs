// -----------------------------------------------------------------------
// <copyright file="TasksUrlFactory.cs" author="Aaron Morris">
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

namespace RtmDotNet.Http.Api.Tasks
{
    public class TasksUrlFactory : ITasksUrlFactory
    {
        private readonly ITasksUrlBuilderFactory _urlBuilderFactory;

        public TasksUrlFactory(ITasksUrlBuilderFactory urlBuilderFactory)
        {
            _urlBuilderFactory = urlBuilderFactory;
        }

        public string CreateGetListsUrl(string authToken, DateTime? lastSync = null, string listId = "", string filter = "")
        {
            return _urlBuilderFactory.CreateGetListsUrlBuilder(authToken, lastSync, listId, filter).BuildUrl();
        }
    }
}