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
using NUnit.Framework;
using RtmDotNet.Http;

namespace RtmDotNet.UnitTests.Http
{
    public abstract class SignedUrlBuilderTests : UrlBuilderTests
    {
        protected const string FakeApiSig = "abc123xyz";

        [Test]
        public void BuildUrl_SignsWithExpectedParameters()
        {
            // Setup
            var mockSignatureGenerator = new MockSignatureGenerator();
            
            // Execute
            var urlBuilder = GetItemUnderTest(mockSignatureGenerator);
            urlBuilder.BuildUrl();

            // Verify
            Assert.IsTrue(mockSignatureGenerator.WasCalledWithParams(ExpectedParams));
        }

        protected abstract IDictionary<string, string> ExpectedParams { get; }

        protected abstract IUrlBuilder GetItemUnderTest(IApiSignatureGenerator signatureGenerator);

        protected override IUrlBuilder GetItemUnderTest()
        {
            return GetItemUnderTest(new MockSignatureGenerator());
        }

        protected class MockSignatureGenerator : IApiSignatureGenerator
        {
            private IDictionary<string, string> _parameters;

            public string GenerateSignature(IDictionary<string, string> parameters)
            {
                _parameters = parameters;
                return FakeApiSig;
            }

            public bool WasCalledWithParams(IDictionary<string, string> parameters)
            {
                if (_parameters == null)
                {
                    // Method wasn't called at all.
                    return false;
                }

                if (!parameters.Keys.All(_parameters.Keys.Contains) || parameters.Keys.Count != _parameters.Keys.Count)
                {
                    // The two parameter dictionaries have different sets of keys.
                    return false;
                }

                // Check if all parameter values matched.
                return parameters.All(parameter => parameter.Value.Equals(_parameters[parameter.Key]));
            }
        }
    }
}