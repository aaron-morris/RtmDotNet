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
using NUnit.Framework;
using RtmDotNet.Http;
using RtmDotNet.Http.Api.Locations;

namespace RtmDotNet.UnitTests.Http.Api.Locations
{
    [TestFixture]
    public class GetListUrlBuilderTests : ApiUrlBuilderTests
    {
        private const string FakeToken = "My Fake Auth Token";

        protected override IUrlBuilder GetItemUnderTest(IApiSignatureGenerator signatureGenerator)
        {
            return new GetListUrlBuilder(FakeApiKey, signatureGenerator, FakeToken);
        }

        protected override string MethodName => GetListUrlBuilder.MethodName;

        protected override IDictionary<string, string> ExpectedParams
        {
            get
            {
                var expectedParams = base.ExpectedParams;
                expectedParams.Add("auth_token", FakeToken);
                return expectedParams;
            }
        }
    }
}