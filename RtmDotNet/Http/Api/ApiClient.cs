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
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RtmDotNet.Http.Api
{
    public class ApiClient : IApiClient
    {
        private const string DefaultMediaType = "application/json";

        // RTM enforces an API usage limit of one request per second.
        private static readonly TimeSpan ApiRateLimit = TimeSpan.FromSeconds(1);

        private readonly IHttpClient _httpClient;

        private DateTime _lastRequestTime;

        public ApiClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(DefaultMediaType));
        }

        public async Task<T> GetAsync<T>(string url) where T : ApiResponseData
        {
            var responseData = await DoGetAsync<T>(url).ConfigureAwait(false);
            ThrowApiExceptionOnFailureResponse(responseData);
            return responseData;
        }

        private void ThrowApiExceptionOnFailureResponse(ApiResponseData responseData)
        {
            if (responseData.Status.Equals("fail"))
            {
                throw new RtmException(responseData.Error.Code, responseData.Error.Message);
            }
        }

        private async Task<T> DoGetAsync<T>(string url) where T : ApiResponseData
        {
            var content = await SendApiRequest(url).ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<ApiResponse<T>>(content);
            return response.Content;
        }

        private async Task<string> SendApiRequest(string url)
        {
            // Enforce the RTM API rate limiting policy before sending the next request.
            // Use the queued lock to ensure that each request is rate-limited in a FIFO manner.
            EnforceApiRateLimiting();

            // Send the request
            return await _httpClient.GetStringAsync(url).ConfigureAwait(false);
        }

        private void EnforceApiRateLimiting()
        {
            try
            {
                Monitor.Enter(this);

                // Determine how much time has passed since the last request was sent.
                var timeSinceLastRequest = DateTime.Now.Subtract(_lastRequestTime);

                // If the elapsed time is less than required by the RTM API usage limit, then wait until enough time has elapsed.
                if (timeSinceLastRequest < ApiRateLimit)
                {
                    Thread.Sleep(ApiRateLimit.Subtract(timeSinceLastRequest));
                }

                _lastRequestTime = DateTime.Now;
            }
            finally
            {
                Monitor.Exit(this);
            }
        }
    }
}