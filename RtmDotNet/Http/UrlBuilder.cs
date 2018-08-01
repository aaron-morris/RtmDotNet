// -----------------------------------------------------------------------
// <copyright file="UrlBuilder.cs" author="Aaron Morris">
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

using System.Collections.Generic;
using System.Linq;

namespace RtmDotNet.Http
{
    public abstract class UrlBuilder : IUrlBuilder
    {
        protected UrlBuilder(string apiKey)
        {
            Parameters = new Dictionary<string, string>
            {
                { "api_key", apiKey }
            };
        }

        public IDictionary<string, string> Parameters { get; }

        protected abstract string BaseUrl { get; }

        public virtual string BuildUrl()
        {
            // Create parameter pairs by joining keys and values with '=' signs.
            var paramPairs = Parameters.Select(x => x.Key + "=" + x.Value);

            // Create a parameter string by joining parameter pairs with '&'.
            var paramString = string.Join("&", paramPairs);

            var url = BaseUrl + paramString;

            return url;
        }
    }
}