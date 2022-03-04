using AoC2021.Day05;
using NUnit.Framework;
using System;

namespace AoC2021.Tests.Day05
{
  public class Day05Tests
  {
    [Test]
    public void PartATest()
    {
      Assert.AreEqual(5, HydrothermalVenture.CountOrientedOverlaps(Data.Day05ExampleData));
    }

    [Test]
    public void PartBTest()
    {
      Assert.AreEqual(12, HydrothermalVenture.CountAllOverlaps(Data.Day05ExampleData));
    }
  }
}
