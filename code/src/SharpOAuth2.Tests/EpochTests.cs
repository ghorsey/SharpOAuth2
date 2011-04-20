using System;
using NUnit.Framework;
using SharpOAuth2.Provider.Utility;

namespace SharpOAuth2.Tests
{
    [TestFixture]
    public class EpochTests
    {
        [Test]
        public void TestToEpochValue()
        {
            DateTime time = new DateTime(1977, 4, 4, 6, 0, 0, 0);

            Assert.AreEqual(229006800, time.ToEpoch());
        }
        [Test]
        public void TestFromEpoch()
        {
            DateTime time = Epoch.FromEpoch(229006800);

            Assert.AreEqual(new DateTime(1977, 4, 4, 6, 0, 0, 0), time);
        }
    }
}
