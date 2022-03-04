using AoC2021.Day01;
using System;
using NUnit.Framework;

namespace AoC2021.Tests.Day01
{
  public class Day01Tests
  {
    [Test]
    public void DepthIncreasedCountTest()
    {
      Assert.AreEqual(7, SonarSweeper.CalculateIncreases(Data.Day01ExampleData));
    }

    [Test]
    public void SlidingWindowCountTest()
    {
      Assert.AreEqual(5, SonarSweeper.CalculateSlidingWindowIncreases(Data.Day01ExampleData));
    }
  }
}
