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
using System;
using NUnit.Framework;
using RtmDotNet.Auth;
using RtmDotNet.Lists;
using RtmDotNet.Locations;
using RtmDotNet.Tasks;
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
            Assert.IsInstanceOf<IUserFactory>(actual);
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
            var actual = Rtm.GetListRepository(new AuthenticationToken());
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
            Assert.Throws<InvalidOperationException>(() => Rtm.GetListRepository(new AuthenticationToken()));
        }


        [Test]
        public void GetTaskRepository_ApiIsInitialized_CreatesTaskRepository()
        {
            Rtm.Init("test", "test");
            var actual = Rtm.GetTaskRepository(new AuthenticationToken());
            Assert.IsInstanceOf<ITaskRepository>(actual);
        }

        [Test]
        public void GetTaskRepository_NullToken_ThrowsArgumentNullException()
        {
            Rtm.Init("test", "test");
            Assert.Throws<ArgumentNullException>(() => Rtm.GetTaskRepository(null));
        }

        [Test]
        public void GetTaskRepostiory_ApiNotInitialized_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => Rtm.GetTaskRepository(new AuthenticationToken()));
        }

        [Test]
        public void GetLocationRepository_ApiIsInitialized_CreatesListRepository()
        {
            Rtm.Init("test", "test");
            var actual = Rtm.GetLocationRepository(new AuthenticationToken());
            Assert.IsInstanceOf<LocationRepository>(actual);
        }

        [Test]
        public void GetLocationRepository_NullToken_ThrowsArgumentNullException()
        {
            Rtm.Init("test", "test");
            Assert.Throws<ArgumentNullException>(() => Rtm.GetLocationRepository(null));
        }

        [Test]
        public void GetLocationRepostiory_ApiNotInitialized_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => Rtm.GetLocationRepository(new AuthenticationToken()));
        }
    }
}