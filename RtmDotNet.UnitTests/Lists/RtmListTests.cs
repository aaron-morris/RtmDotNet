// -----------------------------------------------------------------------
// <copyright file="RtmListTests.cs" author="Aaron Morris">
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

using Newtonsoft.Json;
using NUnit.Framework;
using RtmDotNet.Lists;

namespace RtmDotNet.UnitTests.Lists
{
    [TestFixture]
    public class RtmListTests
    {
        [Test]
        public void SystemList_ParsesFromJson()
        {
            // Setup
            const string json =
                "{\r\n\t\t\t\t\t\"id\": \"123456\",\r\n\t\t\t\t\t\"name\": \"Inbox\",\r\n\t\t\t\t\t\"deleted\": \"0\",\r\n\t\t\t\t\t\"locked\": \"1\",\r\n\t\t\t\t\t\"archived\": \"0\",\r\n\t\t\t\t\t\"position\": \"-1\",\r\n\t\t\t\t\t\"smart\": \"0\",\r\n\t\t\t\t\t\"sort_order\": \"0\",\r\n\t\t\t\t\t\"permission\": \"owner\"\r\n\t\t\t\t}";

            // Execute
            var actual = JsonConvert.DeserializeObject<RtmList>(json);

            // Verify
            Assert.AreEqual("123456", actual.Id);
            Assert.AreEqual("Inbox", actual.Name);
            Assert.IsTrue(actual.IsLocked);
            Assert.IsFalse(actual.IsArchived);
            Assert.AreEqual(-1, actual.Position);
            Assert.IsFalse(actual.IsSmart);
            Assert.AreEqual(0, actual.SortOrder);
            Assert.AreEqual("owner", actual.Permission);
        }

        [Test]
        public void ArchivedList_ParsesFromJson()
        {
            // Setup
            const string json =
                "{\r\n\t\t\t\t\t\"id\": \"654321\",\r\n\t\t\t\t\t\"name\": \"My Archived List\",\r\n\t\t\t\t\t\"deleted\": \"0\",\r\n\t\t\t\t\t\"locked\": \"0\",\r\n\t\t\t\t\t\"archived\": \"1\",\r\n\t\t\t\t\t\"position\": \"0\",\r\n\t\t\t\t\t\"smart\": \"0\",\r\n\t\t\t\t\t\"sort_order\": \"0\",\r\n\t\t\t\t\t\"permission\": \"owner\"\r\n\t\t\t\t}";

            // Execute
            var actual = JsonConvert.DeserializeObject<RtmList>(json);

            // Verify
            Assert.AreEqual("654321", actual.Id);
            Assert.AreEqual("My Archived List", actual.Name);
            Assert.IsFalse(actual.IsLocked);
            Assert.IsTrue(actual.IsArchived);
            Assert.AreEqual(0, actual.Position);
            Assert.IsFalse(actual.IsSmart);
            Assert.AreEqual(0, actual.SortOrder);
            Assert.AreEqual("owner", actual.Permission);
        }

        [Test]
        public void SmartList_ParsesFromJson()
        {
            // Setup
            const string json =
                "{\r\n\t\t\t\t\t\"id\": \"654321\",\r\n\t\t\t\t\t\"name\": \"My Smart List\",\r\n\t\t\t\t\t\"deleted\": \"0\",\r\n\t\t\t\t\t\"locked\": \"0\",\r\n\t\t\t\t\t\"archived\": \"0\",\r\n\t\t\t\t\t\"position\": \"0\",\r\n\t\t\t\t\t\"smart\": \"1\",\r\n\t\t\t\t\t\"sort_order\": \"1\",\r\n\t\t\t\t\t\"filter\": \"not tagContains:. AND (list:Projects OR list:\\\"Someday/Maybe\\\") AND isSubtask:false\"\r\n\t\t\t\t}";
                //  "{\r\n\t\t\t\t\t\"id\": \"654321\",\r\n\t\t\t\t\t\"name\": \"My Smart List\",\r\n\t\t\t\t\t\"deleted\": \"0\",\r\n\t\t\t\t\t\"locked\": \"0\",\r\n\t\t\t\t\t\"archived\": \"0\",\r\n\t\t\t\t\t\"position\": \"0\",\r\n\t\t\t\t\t\"smart\": \"1\",\r\n\t\t\t\t\t\"sort_order\": \"0\",\r\n\t\t\t\t\t\"permission\": \"owner\"\r\n\t\t\t\t}";
            ;

            // Execute
            var actual = JsonConvert.DeserializeObject<RtmList>(json);

            // Verify
            Assert.AreEqual("654321", actual.Id);
            Assert.AreEqual("My Smart List", actual.Name);
            Assert.IsFalse(actual.IsLocked);
            Assert.IsFalse(actual.IsArchived);
            Assert.AreEqual(0, actual.Position);
            Assert.IsTrue(actual.IsSmart);
            Assert.AreEqual(1, actual.SortOrder);
            Assert.AreEqual("not tagContains:. AND (list:Projects OR list:\"Someday/Maybe\") AND isSubtask:false", actual.Filter);
        }
    }
}