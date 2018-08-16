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
using NUnit.Framework;
using RtmDotNet.Http;

namespace RtmDotNet.UnitTests.Http
{
    public abstract class UrlBuilderTests
    {
        protected const string FakeApiKey = "My_Fake_API_Key";

        [Test]
        public void BuildUrl_ReturnsExpectedUrl()
        {
            // Execute
            var urlBuilder = GetItemUnderTest();
            var actual = urlBuilder.BuildUrl();

            // Verify
            Assert.AreEqual(ExpectedUrl, actual);
        }

        protected abstract string ExpectedUrl { get; }

        protected abstract IUrlBuilder GetItemUnderTest();
    }
}