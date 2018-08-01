// -----------------------------------------------------------------------
// <copyright file="UrlBuilderFactory.cs" author="Aaron Morris">
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

using RtmDotNet.Http.Api;
using RtmDotNet.Http.Api.Auth;
using RtmDotNet.Http.Auth;

namespace RtmDotNet.Http
{
    public class UrlBuilderFactory : IUrlBuilderFactory
    {
        private readonly string _apiKey;

        private readonly IApiSignatureGenerator _signatureGenerator;

        public UrlBuilderFactory(string apiKey, IApiSignatureGenerator signatureGenerator)
        {
            _apiKey = apiKey;
            _signatureGenerator = signatureGenerator;
        }

        public IUrlBuilder CreateCheckTokenUrlBuilder(string authToken)
        {
            return new CheckTokenUrlBuilder(_apiKey, _signatureGenerator, authToken);
        }

        public IUrlBuilder CreateGetFrobUrlBuilder()
        {
            return new GetFrobUrlBuilder(_apiKey, _signatureGenerator);
        }

        public IUrlBuilder CreateGetTokenUrlBuilder(string frob)
        {
            return new GetTokenUrlBuilder(_apiKey, _signatureGenerator, frob);
        }

        public IUrlBuilder CreateAuthUrlBuilder(string permissionLevel)
        {
            return new AuthUrlBuilder(_apiKey, _signatureGenerator, permissionLevel);
        }

        public IUrlBuilder CreateAuthUrlBuilder(string permissionLevel, string frob)
        {
            return new AuthUrlBuilder(_apiKey, _signatureGenerator, permissionLevel, frob);
        }
    }
}