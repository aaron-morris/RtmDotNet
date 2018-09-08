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
using RtmDotNet.Http.Api.Locations;

namespace RtmDotNet.UnitTests.Http.Api.Locations
{
    [TestFixture]
    public class ResponseParserTests
    {
        private const string ExpectedId = "Fake Location Id";
        private const string ExpectedName = "Fake Location Name";
        private const string ExpectedAddress = "Fake Location Address";
        private const bool ExpectedIsViewable = true;
        private const double ExpectedLatitude = 1.23456789d;
        private const double ExpectedLongitude = 98.7654321d;
        private const int ExpectedZoom = 5;

        [Test]
        public void GetLocations_LocationData_InitsFromLocationData()
        {
            // Setup
            var fakeResponseData = GetFakeResponsesData();
            
            // Execute
            var responseParser = GetItemUnderTest();
            var actual = responseParser.GetLocations(fakeResponseData);

            // Verify
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(ExpectedId, actual[0].Id);
            Assert.AreEqual(ExpectedName, actual[0].Name);
            Assert.AreEqual(ExpectedAddress, actual[0].Address);
            Assert.AreEqual(ExpectedIsViewable, actual[0].IsViewable);
            Assert.AreEqual(ExpectedLatitude, actual[0].Latitude);
            Assert.AreEqual(ExpectedLongitude, actual[0].Longitude);
            Assert.AreEqual(ExpectedZoom, actual[0].Zoom);
        }

        [Test]
        public void GetLocations_NullData_InitsEmptyList()
        {
            var responseParser = GetItemUnderTest();
            var actual = responseParser.GetLocations(null);

            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void GetLocations_NullListOfLocationsElement_InitsEmptyList()
        {
            var fakeResponseData = new GetListResponseData { Locations = null};
            var responseParser = GetItemUnderTest();
            var actual = responseParser.GetLocations(fakeResponseData);

            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void GetLocations_NullListsElement_InitsEmptyList()
        {
            var fakeResponseData = new GetListResponseData
            {
                Locations = new GetListResponseData.ListOfLocations
                {
                    Location = null
                }
            };
            var responseParser = GetItemUnderTest();
            var actual = responseParser.GetLocations(fakeResponseData);

            Assert.AreEqual(0, actual.Count);
        }

        private IResponseParser GetItemUnderTest()
        {
            return new ResponseParser();
        }

        private GetListResponseData GetFakeResponsesData()
        {
            return new GetListResponseData
            {
                Locations = new GetListResponseData.ListOfLocations
                {
                    Location = new List<GetListResponseData.LocationData>
                    {
                        new GetListResponseData.LocationData
                        {
                            Id = ExpectedId,
                            Name = ExpectedName,
                            Address = ExpectedAddress,
                            IsViewable = ExpectedIsViewable,
                            Latitude = ExpectedLatitude,
                            Longitude = ExpectedLongitude,
                            Zoom = ExpectedZoom
                        }
                    }
                }
            };
        }
    }
}