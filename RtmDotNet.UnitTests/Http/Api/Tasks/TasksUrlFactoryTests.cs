// -----------------------------------------------------------------------
// <copyright file="TasksUrlFactoryTests.cs" author="Aaron Morris">
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

using NSubstitute;
using NUnit.Framework;
using RtmDotNet.Http;
using RtmDotNet.Http.Api.Tasks;

namespace RtmDotNet.UnitTests.Http.Api.Tasks
{
    [TestFixture]
    public class TasksUrlFactoryTests
    {
        [Test]
        public void CreateGetListsUrl_ReturnsGetListsUrl()
        {
            // Setup
            const string expectedUrl = "My_Fake_Url";
            const string fakeToken = "My Fake Token";

            var fakeUrlBuilder = Substitute.For<IUrlBuilder>();
            fakeUrlBuilder.BuildUrl().Returns(expectedUrl);

            var fakeUrlBuilderFactory = Substitute.For<ITasksUrlBuilderFactory>();
            fakeUrlBuilderFactory.CreateGetListsUrlBuilder(fakeToken).Returns(fakeUrlBuilder);

            // Execute
            var factory = GetItemUnderTest(fakeUrlBuilderFactory);
            var actual = factory.CreateGetListsUrl(fakeToken);

            // Verify
            Assert.AreEqual(expectedUrl, actual);
        }

        private TasksUrlFactory GetItemUnderTest(ITasksUrlBuilderFactory urlBuilderFactory)
        {
            return new TasksUrlFactory(urlBuilderFactory);
        }
    }
}