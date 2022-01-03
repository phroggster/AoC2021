using AoC2021.Day02;
using NUnit.Framework;
using System;

namespace AoC2021.Tests
{
    public class Day02Tests
    {
        static readonly Instruction[] s_ExampleData =
        {
            new("forward", 5),
            new("down", 5),
            new("forward", 8),
            new("up", 3),
            new("down", 8),
            new("forward", 2)
        };

        [Test]
        public void PartATest()
        {
            Assert.AreEqual(150, PositionTracker.CalculateSimple(s_ExampleData));
        }

        [Test]
        public void PartBTest()
        {
            Assert.AreEqual(900, PositionTracker.CalculateWithAim(s_ExampleData));
        }
    }
}
