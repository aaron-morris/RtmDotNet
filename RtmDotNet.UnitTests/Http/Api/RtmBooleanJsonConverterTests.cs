// -----------------------------------------------------------------------
// <copyright file="RtmBooleanJsonConverterTests.cs" author="Aaron Morris">
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
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Http.Api;

namespace RtmDotNet.UnitTests.Http.Api
{
    [TestFixture]
    public class RtmBooleanJsonConverterTests
    {
        [Test]
        public void CanRead_IsTrue()
        {
            var converter = GetItemUnderTest();
            Assert.IsTrue(converter.CanRead);
        }

        [Test]
        public void CanWrite_IsTrue()
        {
            var converter = GetItemUnderTest();
            Assert.IsTrue(converter.CanWrite);
        }

        [TestCase(true, "1")]
        [TestCase(false, "0")]
        public void WriteJson_ConvertsToZeroeAndOnes(bool value, string expectedWriteValue)
        {
            // Setup
            var mockJsonWriter = Substitute.For<JsonWriter>();

            // Execute
            var jsonConverter = GetItemUnderTest();
            jsonConverter.WriteJson(mockJsonWriter, value, null);

            // Verify
            mockJsonWriter.Received(1).WriteValue(expectedWriteValue);
        }

        [TestCase("", false)]
        [TestCase("0", false)]
        [TestCase("1", true)]
        public void ReadJson_ZeroOrOne_ConvertsToBool(string jsonValue, bool expectedValue)
        {
            // Setup
            var fakeJsonReader = Substitute.For<JsonReader>();
            fakeJsonReader.Value.Returns(jsonValue);

            // Execute
            var jsonConverter = GetItemUnderTest();
            var actual = jsonConverter.ReadJson(fakeJsonReader, null, null, null);

            // Verify
            Assert.AreEqual(expectedValue, actual);
        }

        [Test]
        public void ReadJson_OtherThanZeroOrOne_ThrowsFormatException()
        {
            // Setup
            var fakeJsonReader = Substitute.For<JsonReader>();
            fakeJsonReader.Value.Returns("99");

            // Execute
            var jsonConverter = GetItemUnderTest();
            Assert.Throws<FormatException>(() => jsonConverter.ReadJson(fakeJsonReader, null, null, null));
        }

        [Test]
        public void CanConvert_BooleanType_IsTrue()
        {
            // Execute
            var jsonConverter = GetItemUnderTest();
            var actual = jsonConverter.CanConvert(typeof(bool));

            Assert.IsTrue(actual);
        }

        [Test]
        public void CanConvert_OtherType_IsFalse()
        {
            // Execute
            var jsonConverter = GetItemUnderTest();
            var actual = jsonConverter.CanConvert(GetType());

            Assert.IsFalse(actual);
        }
        private RtmBooleanJsonConverter GetItemUnderTest()
        {
            return new RtmBooleanJsonConverter();
        }
    }
}