// -----------------------------------------------------------------------
// <copyright file="RtmApiClientTests.cs" author="Aaron Morris">
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Http;
using RtmDotNet.Http.Api;

namespace RtmDotNet.UnitTests.Http.Api
{
    [TestFixture]
    public class RtmApiClientTests
    {
        [Test]
        public async Task GetAsync_ParsesJsonToReturnObject()
        {
            // Setup
            const string fakeUrl = "My_Fake_Url";
            const string expectedStatus = "My Fake Status";
            const string expectedPropertyValue = "My Fake Property Value";
            var fakeJsonResult = $"{{\r\n    \"rsp\":\r\n    {{\r\n        \"stat\":\"{expectedStatus}\",\r\n        \"fake_property\":\"{expectedPropertyValue}\"\r\n    }}\r\n}}";

            var fakeHttpClient = Substitute.For<IHttpClient>();
            fakeHttpClient.DefaultRequestHeaders.Returns(new HttpClient().DefaultRequestHeaders);
            fakeHttpClient.GetStringAsync(fakeUrl).Returns(Task.FromResult(fakeJsonResult));

            // Execute
            var jsonHttpClient = GetItemUnderTest(fakeHttpClient);
            var actual = await jsonHttpClient.GetAsync<FakeRtmResponseData>(fakeUrl).ConfigureAwait(false);

            // Verify
            Assert.AreEqual(expectedStatus, actual.Status);
            Assert.AreEqual(expectedPropertyValue, actual.MyFakeProperty);
        }

        [Test]
        public void GetAsync_OnErrorResponse_ThrowsRtmApiException()
        {
            // Setup
            const string fakeUrl = "My_Fake_Url";
            const string expectedCode = "123";
            const string expectedMessage = "Some error has occurred.";
            var fakeJsonResult = $"{{\"rsp\":{{\r\n        \"stat\":\"fail\",\r\n        \"err\":{{\r\n               \"code\":\"{expectedCode}\",\r\n               \"msg\":\"{expectedMessage}\"\r\n              }}\r\n       }}\r\n}}";

            var fakeHttpClient = Substitute.For<IHttpClient>();
            fakeHttpClient.DefaultRequestHeaders.Returns(new HttpClient().DefaultRequestHeaders);
            fakeHttpClient.GetStringAsync(fakeUrl).Returns(Task.FromResult(fakeJsonResult));

            // Execute
            var jsonHttpClient = GetItemUnderTest(fakeHttpClient);
            var actual = Assert.ThrowsAsync<RtmException>(async () => await jsonHttpClient.GetAsync<FakeRtmResponseData>(fakeUrl).ConfigureAwait(false));

            // Verify
            Assert.AreEqual(expectedCode, actual.ErrorCode);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [Test]
        public void GetAsync_MultipleRequests_EnforcesApiRateLimiting()
        {
            // Setup
            var mockHttpClient = new FakeHttpClient();

            // Execute
            var apiClient = GetItemUnderTest(mockHttpClient);

            var tasks = new List<Task>
            {
                apiClient.GetAsync<FakeRtmResponseData>(string.Empty),
                apiClient.GetAsync<FakeRtmResponseData>(string.Empty),
                apiClient.GetAsync<FakeRtmResponseData>(string.Empty),
                apiClient.GetAsync<FakeRtmResponseData>(string.Empty),
                apiClient.GetAsync<FakeRtmResponseData>(string.Empty)
            };

            Task.WaitAll(tasks.ToArray());

            // Verify
            Assert.AreEqual(5, mockHttpClient.RequestTimeStamps.Count);

            for (var index = 1; index < 5; index++)
            {
                // Verify that 1 second elapsed between each request.  This can only be verified to the accuracy of +/- 2 millisecond.  Therefore,
                // we're asserting that at least 998 milliseconds have passed.  (Basically 1 second.)
                var elapsedMilliSeconds = mockHttpClient.RequestTimeStamps[index].Subtract(mockHttpClient.RequestTimeStamps[index - 1]).TotalMilliseconds;
                Assert.IsTrue(elapsedMilliSeconds >= 998);
            }
        }

        private RtmApiClient GetItemUnderTest(IHttpClient httpClient)
        {
            return new RtmApiClient(httpClient);
        }

        private class FakeRtmResponseData : RtmApiResponseData
        {
            [JsonProperty("fake_property")]
            public string MyFakeProperty { get; set; }
        }

        private class FakeHttpClient : IHttpClient
        {
            public FakeHttpClient()
            {
                RequestTimeStamps = new List<DateTime>();
                DefaultRequestHeaders = new HttpClient().DefaultRequestHeaders;
            }

            public HttpRequestHeaders DefaultRequestHeaders { get; }

            public Task<string> GetStringAsync(string url)
            {
                lock (this)
                {
                    RequestTimeStamps.Add(DateTime.Now); 
                }

                return Task.FromResult(@"{""rsp"": {""stat"": ""pass""}}");
            }

            public List<DateTime> RequestTimeStamps { get; }
        }
    }
}