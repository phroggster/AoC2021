using AoC2021.Day02;
using NUnit.Framework;
using System;

namespace AoC2021.Tests
{
  public class Day02Tests
  {
    [Test]
    public void PartATest()
    {
      Assert.AreEqual(150, PositionTracker.CalculateSimple(Data.Day02ExampleData));
    }

    [Test]
    public void PartBTest()
    {
      Assert.AreEqual(900, PositionTracker.CalculateWithAim(Data.Day02ExampleData));
    }
  }
}
