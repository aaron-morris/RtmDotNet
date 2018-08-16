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
namespace RtmDotNet.Http.Auth
{
    public class AuthUrlBuilder : SignedUrlBuilder
    {
        public AuthUrlBuilder(string apiKey, IApiSignatureGenerator signatureGenerator, string permissionLevel) : base(apiKey, signatureGenerator)
        {
            Parameters.Add("perms", permissionLevel);
        }

        public AuthUrlBuilder(string apiKey, IApiSignatureGenerator signatureGenerator, string permissionLevel, string frob)
            : this(apiKey, signatureGenerator, permissionLevel)
        {
            Parameters.Add("frob", frob);
        }

        public static string AuthUrl => "https://www.rememberthemilk.com/services/auth/?";

        protected override string BaseUrl => AuthUrl;
    }
}