// -----------------------------------------------------------------------
// <copyright file="Md5DataHasherTests.cs" author="Aaron Morris">
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
using NUnit.Framework;
using RtmDotNet.Http;

namespace RtmDotNet.UnitTests.Http
{
    [TestFixture]
    public class Md5DataHasherTests
    {
        [TestCase("Hello World!", "ed076287532e86365e841e92bfc50d8c")]
        [TestCase("Remember the Milk", "b70ef6b21c93cc0be4915357565addb0")]
        [TestCase("", "d41d8cd98f00b204e9800998ecf8427e")]
        public void GetHash_KnownInput_KnownMd5Hash(string input, string hash)
        {
            // Execute
            var dataHasher = GetItemUnderTest();
            var actual = dataHasher.GetHash(input);

            // Verify
            Assert.AreEqual(hash, actual);
        }

        [Test]
        public void GetHash_NullInput_ThrowsArgumentNullException()
        {
            var dataHasher = GetItemUnderTest();
            Assert.Catch<ArgumentNullException>(() => dataHasher.GetHash(null));
        }

        private Md5DataHasher GetItemUnderTest()
        {
            return new Md5DataHasher();
        }
    }
}