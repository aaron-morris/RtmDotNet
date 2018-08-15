// -----------------------------------------------------------------------
// <copyright file="TokenVerifier.cs" author="Aaron Morris">
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

using System.Threading.Tasks;
using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Auth;

namespace RtmDotNet.Auth
{
    public class TokenVerifier : ITokenVerifier
    {
        private readonly IAuthUrlFactory _urlFactory;
        private readonly IRtmApiClient _apiClient;

        public TokenVerifier(IAuthUrlFactory urlFactory, IRtmApiClient apiClient)
        {
            _urlFactory = urlFactory;
            _apiClient = apiClient;
        }

        public async Task<bool> VerifyAsync(AuthenticationToken token)
        {
            try
            {
                var checkTokenUrl = _urlFactory.CreateCheckTokenUrl(token.Id);
                await _apiClient.GetAsync<GetTokenResponseData>(checkTokenUrl).ConfigureAwait(false);

                return true;
            }
            catch (RtmException ex)
            {
                if (ex.ErrorCode.Equals(RtmErrorCodes.InvalidAuthToken))
                {
                    // Authentication verification failed.
                    return false;
                }

                // Another error
                throw;
            }
            
        }
    }
}