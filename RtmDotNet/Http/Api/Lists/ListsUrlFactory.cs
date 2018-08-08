// -----------------------------------------------------------------------
// <copyright file="ListsUrlFactory.cs" author="Aaron Morris">
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

namespace RtmDotNet.Http.Api.Lists
{
    public class ListsUrlFactory : IListsUrlFactory
    {
        private readonly IListsUrlBuilderFactory _urlBuilderFactory;

        public ListsUrlFactory(IListsUrlBuilderFactory urlBuilderFactory)
        {
            _urlBuilderFactory = urlBuilderFactory;
        }

        public string CreateGetListsUrl(string authToken)
        {
            return _urlBuilderFactory.CreateGetListsUrlBuilder(authToken).BuildUrl();
        }
    }
}