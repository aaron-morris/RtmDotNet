﻿// -----------------------------------------------------------------------
// <copyright file="SignedUrlBuilder.cs" author="Aaron Morris">
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

namespace RtmDotNet.Http
{
    public abstract class SignedUrlBuilder : UrlBuilder
    {
        private readonly IApiSignatureGenerator _signatureGenerator;

        protected SignedUrlBuilder(string apiKey, IApiSignatureGenerator signatureGenerator) : base(apiKey)
        {
            _signatureGenerator = signatureGenerator;
        }

        public override string BuildUrl()
        {
            return BuildUrlWithSignature();
        }

        private string BuildUrlWithSignature()
        {
            var urlWithoutSignature = base.BuildUrl();
            var signature = _signatureGenerator.GenerateSignature(Parameters);
            var urlWithSignature = urlWithoutSignature + "&api_sig=" + signature;

            return urlWithSignature;
        }
    }
}