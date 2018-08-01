// -----------------------------------------------------------------------
// <copyright file="Md5DataHasher.cs" author="Aaron Morris">
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
using System.Security.Cryptography;
using System.Text;

namespace RtmDotNet.Http
{
    public class Md5DataHasher : IDataHasher
    {
        public string GetHash(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            using (var md5Hasher = MD5.Create())
            {
                var hashBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
                var hexString = GetHexString(hashBytes);
                return hexString;
            }
        }

        private string GetHexString(byte[] bytes)
        {
            var stringBuilder = new StringBuilder();

            foreach (var t in bytes)
            {
                stringBuilder.Append(t.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}