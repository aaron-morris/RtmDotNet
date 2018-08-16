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
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Auth;

namespace RtmDotNet.UnitTests.Auth
{
    [TestFixture]
    public class PermissionsJsonConverterTests
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

        [Test]
        public void WriteJson_ConvertsToString()
        {
            // Setup
            const PermissionLevel permissionLevel = PermissionLevel.Delete;
            const string expectedWriteValue = "My Fake Value";
            var fakePermissionLevelConveter = Substitute.For<IPermissionLevelConverter>();
            fakePermissionLevelConveter.ToString(permissionLevel).Returns(expectedWriteValue);

            var mockJsonWriter = Substitute.For<JsonWriter>();

            // Execute
            var jsonConverter = GetItemUnderTest(fakePermissionLevelConveter);
            jsonConverter.WriteJson(mockJsonWriter, permissionLevel, null);

            // Verify
            mockJsonWriter.Received(1).WriteValue(expectedWriteValue);
        }

        [Test]
        public void ReadJson_ConvertsToEnum()
        {
            // Setup
            const PermissionLevel expectedPermissionLevel = PermissionLevel.Delete;
            const string fakeJsonValue = "My Fake Value";
            var fakePermissionLevelConveter = Substitute.For<IPermissionLevelConverter>();
            fakePermissionLevelConveter.ToEnum(fakeJsonValue).Returns(expectedPermissionLevel);

            var fakeJsonReader = Substitute.For<JsonReader>();
            fakeJsonReader.Value.Returns(fakeJsonValue);

            // Execute
            var jsonConverter = GetItemUnderTest(fakePermissionLevelConveter);
            var actual = jsonConverter.ReadJson(fakeJsonReader, null, null, null);
            
            // Verify
            Assert.AreEqual(expectedPermissionLevel, actual);
        }

        [Test]
        public void CanConvert_PermissionLevelEnumType_IsTrue()
        {
            // Execute
            var jsonConverter = GetItemUnderTest();
            var actual = jsonConverter.CanConvert(typeof(PermissionLevel));

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

        [Test]
        public void DefaultCtor_InstantiatesDefaultDependency()
        {
            // Setup
            var fakeJsonReader = Substitute.For<JsonReader>();
            fakeJsonReader.Value.Returns("Delete");

            // Execute
            var converter = new PermissionsJsonConverter();
            var actual = converter.ReadJson(fakeJsonReader, null, null, null);

            // Verify
            Assert.AreEqual(PermissionLevel.Delete, actual);
        }

        private PermissionsJsonConverter GetItemUnderTest()
        {
            return GetItemUnderTest(Substitute.For<IPermissionLevelConverter>());
        }

        private PermissionsJsonConverter GetItemUnderTest(IPermissionLevelConverter permissionLevelConverter)
        {
            return new PermissionsJsonConverter(permissionLevelConverter);
        }
    }
}