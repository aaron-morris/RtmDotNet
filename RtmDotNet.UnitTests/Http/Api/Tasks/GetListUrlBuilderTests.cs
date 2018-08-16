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
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Http;
using RtmDotNet.Http.Api.Tasks;

namespace RtmDotNet.UnitTests.Http.Api.Tasks
{
    [TestFixture]
    public class GetListUrlBuilderTests : ApiUrlBuilderTests
    {
        private const string FakeToken = "My Fake Auth Token";

        protected override IUrlBuilder GetItemUnderTest(IApiSignatureGenerator signatureGenerator)
        {
            return new GetListUrlBuilder(FakeApiKey, signatureGenerator, FakeToken);
        }

        [Test]
        public void Ctor_NoOptionalArgs_OmmitsOptionalParams()
        {
            var actual = new GetListUrlBuilder(FakeApiKey, Substitute.For<IApiSignatureGenerator>(), FakeToken);

            Assert.IsFalse(actual.Parameters.ContainsKey("last_sync"));
            Assert.IsFalse(actual.Parameters.ContainsKey("list_id"));
            Assert.IsFalse(actual.Parameters.ContainsKey("filter"));
        }

        [Test]
        public void Ctor_LastSync_AddsLastSyncParam()
        {
            var testTime = DateTime.Now;
            var actual = new GetListUrlBuilder(FakeApiKey, Substitute.For<IApiSignatureGenerator>(), FakeToken, lastSync: testTime);

            Assert.IsTrue(actual.Parameters.ContainsKey("last_sync"));
            Assert.AreEqual(testTime.ToString("yyyy-MM-ddTHH:mm:ssZ"), actual.Parameters["last_sync"]);

            Assert.IsFalse(actual.Parameters.ContainsKey("list_id"));
            Assert.IsFalse(actual.Parameters.ContainsKey("filter"));
        }

        [Test]
        public void Ctor_ListId_AddsListIdParam()
        {
            const string listId = "123456";
            var actual = new GetListUrlBuilder(FakeApiKey, Substitute.For<IApiSignatureGenerator>(), FakeToken, listId: listId);

            Assert.IsFalse(actual.Parameters.ContainsKey("last_sync"));
            Assert.IsTrue(actual.Parameters.ContainsKey("list_id"));
            Assert.AreEqual(listId, actual.Parameters["list_id"]);
            Assert.IsFalse(actual.Parameters.ContainsKey("filter"));
        }

        [Test]
        public void Ctor_Filter_AddsFilterParam()
        {
            const string filter = "param:123456";
            var actual = new GetListUrlBuilder(FakeApiKey, Substitute.For<IApiSignatureGenerator>(), FakeToken, filter: filter);

            Assert.IsFalse(actual.Parameters.ContainsKey("last_sync"));
            Assert.IsFalse(actual.Parameters.ContainsKey("list_id"));
            Assert.IsTrue(actual.Parameters.ContainsKey("filter"));
            Assert.AreEqual(filter, actual.Parameters["filter"]);
        }

        [Test]
        public void Ctor_AllArgs_AddsAllParams()
        {
            var testTime = DateTime.Now;
            const string listId = "123456";
            const string filter = "param:123456";
            var actual = new GetListUrlBuilder(FakeApiKey, Substitute.For<IApiSignatureGenerator>(), FakeToken, testTime, listId, filter);

            Assert.IsTrue(actual.Parameters.ContainsKey("last_sync"));
            Assert.AreEqual(testTime.ToString("yyyy-MM-ddTHH:mm:ssZ"), actual.Parameters["last_sync"]);

            Assert.IsTrue(actual.Parameters.ContainsKey("list_id"));
            Assert.AreEqual(listId, actual.Parameters["list_id"]);
                
            Assert.IsTrue(actual.Parameters.ContainsKey("filter"));
            Assert.AreEqual(filter, actual.Parameters["filter"]);
        }

        protected override string MethodName => GetListUrlBuilder.MethodName;

        protected override IDictionary<string, string> ExpectedParams
        {
            get
            {
                var expectedParams = base.ExpectedParams;
                expectedParams.Add("auth_token", FakeToken);
                return expectedParams;
            }
        }
    }
}