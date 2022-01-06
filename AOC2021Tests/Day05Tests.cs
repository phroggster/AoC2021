using AoC2021.Day05;
using NUnit.Framework;
using System;

namespace AoC2021.Tests
{
  public class Day05Tests
  {
    [Test]
    public void PartATest()
    {
      Assert.AreEqual(5, HydrothermalVenture.CountOrientedOverlaps(Data.ExampleDataDay05));
    }

    [Test]
    public void PartBTest()
    {
      Assert.AreEqual(12, HydrothermalVenture.CountAllOverlaps(Data.ExampleDataDay05));
    }
  }
}
