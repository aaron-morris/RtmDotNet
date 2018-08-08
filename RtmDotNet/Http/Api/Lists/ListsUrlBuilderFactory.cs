﻿// -----------------------------------------------------------------------
// <copyright file="ListsUrlBuilderFactory.cs" author="Aaron Morris">
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

namespace RtmDotNet.Http.Api.Lists
{
    public class ListsUrlBuilderFactory : UrlBuilderFactory, IListsUrlBuilderFactory
    {
        public ListsUrlBuilderFactory(string apiKey, IApiSignatureGenerator signatureGenerator) : base(apiKey, signatureGenerator)
        {
        }

        public IUrlBuilder CreateGetListsUrlBuilder(string authToken)
        {
            return new GetListUrlBuilder(ApiKey, SignatureGenerator, authToken);
        }
    }
}