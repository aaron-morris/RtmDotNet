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
using RtmDotNet.Http.Api;

namespace RtmDotNet.UnitTests.Http.Api
{
    public abstract class ApiUrlBuilderTests : SignedUrlBuilderTests
    {
        protected abstract string MethodName { get; }

        protected override string ExpectedUrl
        {
            get
            {
                var queryStringParameters = ExpectedParams.Select(x => x.Key + "=" + x.Value);
                var queryString = string.Join("&", queryStringParameters);
                return ApiUrlBuilder.ApiUrl + queryString + $"&api_sig={FakeApiSig}";
            }
        }

        protected override IDictionary<string, string> ExpectedParams => new Dictionary<string, string>
        {
            { "api_key", FakeApiKey },
            { "method", MethodName },
            { "format", "json" },
            { "v", "2" }
        };
    }
}