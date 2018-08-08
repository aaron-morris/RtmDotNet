// -----------------------------------------------------------------------
// <copyright file="ListsUrlBuilderFactoryTests.cs" author="Aaron Morris">
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

using NUnit.Framework;
using RtmDotNet.Http;
using RtmDotNet.Http.Api.Lists;

namespace RtmDotNet.UnitTests.Http.Api.Lists
{
    [TestFixture]
    public class ListsUrlBuilderFactoryTests
    {
        [Test]
        public void CreateGetListsUrlBuilder_ReturnsGetListsUrlBuilder()
        {
            // Execute
            var factory = GetItemUnderTest();
            var actual = factory.CreateGetListsUrlBuilder(string.Empty);

            // Verify
            Assert.IsInstanceOf<IUrlBuilder>(actual);
        }

        private ListsUrlBuilderFactory GetItemUnderTest()
        {
            return new ListsUrlBuilderFactory(null, null);
        }
    }
}