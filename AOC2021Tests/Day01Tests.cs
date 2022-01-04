using AoC2021.Day01;
using System;
using NUnit.Framework;

namespace AoC2021.Tests
{
  public class Day01Tests
  {
    [Test]
    public void DepthIncreasedCountTest()
    {
      Assert.AreEqual(7, DepthTest.CalculateIncreases(Data.Day01ExampleData));
    }

    [Test]
    public void SlidingWindowCountTest()
    {
      Assert.AreEqual(5, DepthTest.CalculateSlidingWindowIncreases(Data.Day01ExampleData));
    }
  }
}
