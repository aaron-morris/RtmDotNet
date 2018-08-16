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
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Http;

namespace RtmDotNet.UnitTests.Http
{
    [TestFixture]
    public class ApiSignatureGeneratorTests
    {
        private const string FakeSecret = "TOP_SECRET";

        [TestCase("apple", "123", "boy", "456", "cat", "789", "apple123boy456cat789")]
        [TestCase("cat", "123", "apple", "456", "boy", "789", "apple456boy789cat123")]
        [TestCase("boy", "123", "cat", "456", "apple", "789", "apple789boy123cat456")]
        public void GenerateSignature_ListOfParameters_OrdersAndHashes(
            string key1, string val1, string key2, string val2, string key3, string val3, string orderedParams)
        {
            // Setup
            var parameters = new Dictionary<string, string>
            {
                { key1, val1 },
                { key2, val2 },
                { key3, val3 }
            };

            var expectedParamString = FakeSecret + orderedParams;

            const string fakeHash = "My fake hash value.";
            var mockDataHasher = Substitute.For<IDataHasher>();
            mockDataHasher.GetHash(expectedParamString).Returns(fakeHash);

            // Execute
            var signatureGenerator = GetItemUnderTest(mockDataHasher);
            var actual = signatureGenerator.GenerateSignature(parameters);

            // Verify
            Assert.AreEqual(fakeHash, actual);
            mockDataHasher.Received(1).GetHash(expectedParamString);
        }

        private ApiSignatureGenerator GetItemUnderTest(IDataHasher dataHasher)
        {
            return new ApiSignatureGenerator(dataHasher, FakeSecret);
        }
    }
}