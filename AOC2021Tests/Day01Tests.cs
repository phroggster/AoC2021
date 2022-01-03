using AoC2021.Day01;
using System;
using NUnit.Framework;

namespace AoC2021.Tests
{
    public class Day01Tests
    {
        static readonly int[] s_ExampleDepths =
        {
            199, // N/A - no previous measurement
            200, // increased
            208, // increased
            210, // increased
            200, // decreased
            207, // increased
            240, // increased
            269, // increased
            260, // decreased
            263  // increased
        };

        [Test]
        public void DepthIncreasedCountTest()
        {
            Assert.AreEqual(7, Day01.DepthTest.CalculateIncreases(s_ExampleDepths));
        }

        [Test]
        public void SlidingWindowCountTest()
        {
            Assert.AreEqual(5, Day01.DepthTest.CalculateSlidingWindowIncreases(s_ExampleDepths));
        }
    }
}
