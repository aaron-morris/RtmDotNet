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
using RtmDotNet.Http.Auth;

namespace RtmDotNet.Http.Api.Auth
{
    public class AuthUrlBuilderFactory : UrlBuilderFactory, IAuthUrlBuilderFactory
    {
        public AuthUrlBuilderFactory(string apiKey, IApiSignatureGenerator signatureGenerator) : base(apiKey, signatureGenerator)
        {
        }

        public IUrlBuilder CreateCheckTokenUrlBuilder(string authToken)
        {
            return new CheckTokenUrlBuilder(ApiKey, SignatureGenerator, authToken);
        }

        public IUrlBuilder CreateGetFrobUrlBuilder()
        {
            return new GetFrobUrlBuilder(ApiKey, SignatureGenerator);
        }

        public IUrlBuilder CreateGetTokenUrlBuilder(string frob)
        {
            return new GetTokenUrlBuilder(ApiKey, SignatureGenerator, frob);
        }

        public IUrlBuilder CreateAuthUrlBuilder(string permissionLevel)
        {
            return new AuthUrlBuilder(ApiKey, SignatureGenerator, permissionLevel);
        }

        public IUrlBuilder CreateAuthUrlBuilder(string permissionLevel, string frob)
        {
            return new AuthUrlBuilder(ApiKey, SignatureGenerator, permissionLevel, frob);
        }
    }
}