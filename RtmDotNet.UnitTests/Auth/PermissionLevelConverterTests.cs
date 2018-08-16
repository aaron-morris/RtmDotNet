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
using System;
using NUnit.Framework;
using RtmDotNet.Auth;

namespace RtmDotNet.UnitTests.Auth
{
    [TestFixture]
    public class PermissionLevelConverterTests
    {
        [TestCase(PermissionLevel.Read, "read")]
        [TestCase(PermissionLevel.Write, "write")]
        [TestCase(PermissionLevel.Delete, "delete")]
        [TestCase(PermissionLevel.Undefined, "")]
        public void ToString_PermissionLevel_GetsCorrectName(PermissionLevel permissionLevel, string expectedName)
        {
            // Execute
            var converter = GetItemUnderTest();
            var actual = converter.ToString(permissionLevel);

            // Verify
            Assert.AreEqual(expectedName, actual);
        }

        [Test]
        public void ToString_UnknownPermissionLevel_ThrowsNotImplementedException()
        {
            // Setup
            Assert.IsFalse(Enum.IsDefined(typeof(PermissionLevel), int.MaxValue));  // Ensure test enum value is truly undefined
            var fakePermissionLevel = (PermissionLevel) int.MaxValue;

            // Execute
            var converter = GetItemUnderTest();
            Assert.Catch<NotImplementedException>(() => converter.ToString(fakePermissionLevel));
        }

        [TestCase("", PermissionLevel.Undefined)]
        [TestCase("read", PermissionLevel.Read)]
        [TestCase("READ", PermissionLevel.Read)]
        [TestCase("write", PermissionLevel.Write)]
        [TestCase("WRITE", PermissionLevel.Write)]
        [TestCase("delete", PermissionLevel.Delete)]
        [TestCase("DELETE", PermissionLevel.Delete)]
        public void ToEnum_ValidStringValue_ReturnsEnum(string value, PermissionLevel expected)
        {
            // Execute
            var converter = GetItemUnderTest();
            var actual = converter.ToEnum(value);

            // Verify
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToEnum_UnknownStringValue_ThrowsFormatException()
        {
            // Execute
            var converter = GetItemUnderTest();
            Assert.Catch<FormatException>(() => converter.ToEnum("Bad Value"));
        }

        private PermissionLevelConverter GetItemUnderTest()
        {
            return new PermissionLevelConverter();
        }
    }
}