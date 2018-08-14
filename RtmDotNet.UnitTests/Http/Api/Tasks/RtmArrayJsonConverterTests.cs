// -----------------------------------------------------------------------
// <copyright file="RtmArrayJsonConverterTests.cs" author="Aaron Morris">
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
using System.Collections.Generic;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Http.Api.Tasks;
namespace RtmDotNet.UnitTests.Http.Api.Tasks
{
    [TestFixture]
    public class RtmArrayJsonConverterTests
    {
        [Test]
        public void CanRead_IsTrue()
        {
            var converter = GetItemUnderTest<object>();
            Assert.IsTrue(converter.CanRead);
        }

        [Test]
        public void CanWrite_IsFalse()
        {
            var converter = GetItemUnderTest<object>();
            Assert.IsFalse(converter.CanWrite);
        }

        [Test]
        public void ReadJson_EmptyArray_ReturnsEmptyList()
        {
            // Setup
            const string json = "[]";
            
            // Execute
            var jsonConverter = GetItemUnderTest<string>();
            var actual = JsonConvert.DeserializeObject<IList<string>>(json, jsonConverter);

            // Verify
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void ReadJson_Array_ReturnsList()
        {
            // Setup
            const string json = "[\"elem1\", \"elem2\", \"elem3\"]";

            // Execute
            var jsonConverter = GetItemUnderTest<string>();
            var actual = JsonConvert.DeserializeObject<IList<string>>(json, jsonConverter);

            // Verify
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains("elem1"));
            Assert.IsTrue(actual.Contains("elem2"));
            Assert.IsTrue(actual.Contains("elem3"));
        }

        [Test]
        public void ReadJson_ObjectWithArrayProperty_ReturnsList()
        {
            // Setup
            const string json = "{\"foo\": [\"elem1\", \"elem2\", \"elem3\"]}";

            // Execute
            var jsonConverter = GetItemUnderTest<string>();
            var actual = JsonConvert.DeserializeObject<IList<string>>(json, jsonConverter);

            // Verify
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains("elem1"));
            Assert.IsTrue(actual.Contains("elem2"));
            Assert.IsTrue(actual.Contains("elem3"));
        }

        [TestCase("{\"foo\": \"bar\"}")]
        [TestCase("{\"foo\": {\"bar\": \"baz\"}}")]
        [TestCase("{}")]
        public void ReadJson_ObjectWithoutArrayProperty_ThrowsFormatException(string json)
        {
            var jsonConverter = GetItemUnderTest<string>();
            Assert.Throws<FormatException>(() => JsonConvert.DeserializeObject<IList<string>>(json, jsonConverter));
        }

        [TestCase(",")]
        [TestCase("\"\"")]
        public void ReadJson_NeitherObjectNorArray_ThrowsFormatException(string json)
        {
            var jsonConverter = GetItemUnderTest<string>();
            Assert.Throws<FormatException>(() => JsonConvert.DeserializeObject<IList<string>>(json, jsonConverter));
        }

        [Test]
        public void WriteJson_ThrowsInvalidOperationException()
        {
            var jsonConverter = GetItemUnderTest<string>();
            Assert.Throws<InvalidOperationException>(() => jsonConverter.WriteJson(Substitute.For<JsonWriter>(), new object(), Substitute.For<JsonSerializer>()));
        }

        [Test]
        public void CanConvert_IListOfCorrectGenericType_IsTrue()
        {
            // Execute
            var jsonConverter = GetItemUnderTest<RtmArrayJsonConverterTests>();
            var actual = jsonConverter.CanConvert(typeof(IList<RtmArrayJsonConverterTests>));

            Assert.IsTrue(actual);
        }

        [Test]
        public void CanConvert_IListOfIncorrectGenericType_IsFalse()
        {
            // Execute
            var jsonConverter = GetItemUnderTest<RtmArrayJsonConverterTests>();
            var actual = jsonConverter.CanConvert(typeof(IList<string>));

            Assert.IsFalse(actual);
        }

        [Test]
        public void CanConvert_OtherType_IsFalse()
        {
            // Execute
            var jsonConverter = GetItemUnderTest<string>();
            var actual = jsonConverter.CanConvert(GetType());

            Assert.IsFalse(actual);
        }

        private RtmArrayJsonConverter<T> GetItemUnderTest<T>()
        {
            return new RtmArrayJsonConverter<T>();
        }
    }
}