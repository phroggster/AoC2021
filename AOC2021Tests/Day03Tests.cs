using AoC2021.Day03;
using NUnit.Framework;
using System;

namespace AoC2021.Tests
{
  public class Day03Tests
  {
    [Test]
    public void PartATest()
    {
      Assert.AreEqual(198, ReportParser.CalculatePowerConsumption(Data.Day3ExampleData));
    }

    [Test]
    public void PartBTest()
    {
      Assert.AreEqual(230, ReportParser.CalculateLifeSupportRating(Data.Day3ExampleData));
    }
  }
}
