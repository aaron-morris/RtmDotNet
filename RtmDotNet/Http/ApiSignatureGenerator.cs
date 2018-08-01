// -----------------------------------------------------------------------
// <copyright file="ApiSignatureGenerator.cs" author="Aaron Morris">
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

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtmDotNet.Http
{
    public class ApiSignatureGenerator : IApiSignatureGenerator
    {
        private readonly IDataHasher _dataHasher;
        private readonly string _sharedSecret;

        public ApiSignatureGenerator(IDataHasher dataHasher, string sharedSecret)
        {
            _dataHasher = dataHasher;
            _sharedSecret = sharedSecret;
        }

        public string GenerateSignature(IDictionary<string, string> parameters)
        {
            var orderedParamString = GetOrderedParamString(parameters);
            var stringToHash = _sharedSecret + orderedParamString;
            var hashed = _dataHasher.GetHash(stringToHash);

            return hashed;
        }

        private string GetOrderedParamString(IDictionary<string, string> parameters)
        {
            var orderedPairs = parameters.OrderBy(x => x.Key);

            var stringBuilder = new StringBuilder();

            foreach (var pair in orderedPairs)
            {
                stringBuilder.Append(pair.Key + pair.Value);
            }

            return stringBuilder.ToString();
        }
    }
}