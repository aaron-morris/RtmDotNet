// -----------------------------------------------------------------------
// <copyright file="ListConverterTests.cs" author="Aaron Morris">
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

using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Http.Api.Lists;
using RtmDotNet.Tasks;

namespace RtmDotNet.UnitTests.Http.Api.Lists
{
    [TestFixture]
    public class ListConverterTests
    {
        private const string ExpectedId = "123456";
        private const string ExpectedName = "My List";
        private const bool ExpectedIsLocked = true;
        private const bool ExpectedIsArchived = true;
        private const bool ExpectedIsSmart = true;
        private const int ExpectedPosition = 1;
        private const int ExpectedSortOrder = 2;
        private const string ExpectedPermission = "My Permission";
        private const string ExpectedFilter = "My Filter";

        [Test]
        public void ConvertToLists_ListData_InitsFromListData()
        {
            var fakeResponseData = GetFakeResponsesData();
            var converter = GetItemUnderTest();
            var actual = converter.ConvertToLists(fakeResponseData);


            // Verify
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(ExpectedId, actual[0].Id);
            Assert.AreEqual(ExpectedName, actual[0].Name);
            Assert.AreEqual(ExpectedIsLocked, actual[0].IsLocked);
            Assert.AreEqual(ExpectedIsArchived, actual[0].IsArchived);
            Assert.AreEqual(ExpectedIsSmart, actual[0].IsSmart);
            Assert.AreEqual(ExpectedPosition, actual[0].Position);
            Assert.AreEqual(ExpectedSortOrder, actual[0].SortOrder);
            Assert.AreEqual(ExpectedPermission, actual[0].Permission);
            Assert.AreEqual(ExpectedFilter, actual[0].Filter);
        }

        private IListConverter GetItemUnderTest()
        {
            return new ListConverter(Substitute.For<ITaskRepository>());
        }

        private GetListResponseData GetFakeResponsesData()
        {
            return new GetListResponseData
            {
                Lists = new GetListResponseData.ListOfLists
                {
                    Lists = new List<GetListResponseData.ListData>
                    {
                        new GetListResponseData.ListData
                        {
                            Id = ExpectedId,
                            Filter = ExpectedFilter,
                            IsArchived = ExpectedIsArchived,
                            IsLocked = ExpectedIsLocked,
                            IsSmart = ExpectedIsSmart,
                            Name = ExpectedName,
                            Permission = ExpectedPermission,
                            Position = ExpectedPosition,
                            SortOrder = ExpectedSortOrder
                        }
                    }
                }
            };
        }
    }
}