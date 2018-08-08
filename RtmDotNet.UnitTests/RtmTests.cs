// -----------------------------------------------------------------------
// <copyright file="RtmTests.cs" author="Aaron Morris">
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
using NUnit.Framework;
using RtmDotNet.Auth;
using RtmDotNet.Lists;
using RtmDotNet.Users;

namespace RtmDotNet.UnitTests
{
    [TestFixture]
    public class RtmTests
    {
        [TearDown]
        public void TearDown()
        {
            Rtm.ApiKey = null;
            Rtm.SharedSecret = null;
        }

        [Test]
        public void Init_ValidArgs_InitializesApi()
        {
            const string apiKey = "My API Key";
            const string sharedSecret = "My Shared Secret";

            Rtm.Init(apiKey, sharedSecret);

            Assert.AreEqual(apiKey, Rtm.ApiKey);
            Assert.AreEqual(sharedSecret, Rtm.SharedSecret);
        }

        [TestCase("", "test")]
        [TestCase("test", "")]
        [TestCase("", "")]
        public void Init_NullValues_ThrowsArgumentNullException(string apiKey, string sharedSecret)
        {
            Assert.Throws<ArgumentNullException>(() => Rtm.Init(apiKey, sharedSecret));
        }

        [Test]
        public void GetUserFactory_CreatesUserFactory()
        {
            var actual = Rtm.GetUserFactory();
            Assert.IsInstanceOf<IRtmUserFactory>(actual);
        }

        [Test]
        public void GetAuthFactory_ApiIsInitialized_CreatesAuthFactory()
        {
            Rtm.Init("test", "test");
            var actual = Rtm.GetAuthFactory();
            Assert.IsInstanceOf<IAuthFactory>(actual);
        }

        [Test]
        public void GetAuthFactory_ApiNotInitialized_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => Rtm.GetAuthFactory());
        }

        [Test]
        public void GetListRepository_ApiIsInitialized_CreatesListRepository()
        {
            Rtm.Init("test", "test");
            var actual = Rtm.GetListRepository(new AuthorizationToken());
            Assert.IsInstanceOf<IListRepository>(actual);
        }

        [Test]
        public void GetListRepository_NullToken_ThrowsArgumentNullException()
        {
            Rtm.Init("test", "test");
            Assert.Throws<ArgumentNullException>(() => Rtm.GetListRepository(null));
        }

        [Test]
        public void GetListRepostiory_ApiNotInitialized_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => Rtm.GetListRepository(new AuthorizationToken()));
        }
    }
}