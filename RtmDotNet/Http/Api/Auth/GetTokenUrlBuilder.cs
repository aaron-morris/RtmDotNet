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
namespace RtmDotNet.Http.Api.Auth
{
    public class GetTokenUrlBuilder : ApiUrlBuilder
    {
        public GetTokenUrlBuilder(string apiKey, IApiSignatureGenerator signatureGenerator, string frob) : base(apiKey, signatureGenerator, MethodName)
        {
            Parameters.Add("frob", frob);
        }

        public static string MethodName => "rtm.auth.getToken";
    }
}