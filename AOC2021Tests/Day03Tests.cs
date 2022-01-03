using AoC2021.Day03;
using NUnit.Framework;
using System;

namespace AoC2021.Tests
{
  public class Day03Tests
  {
    static readonly string[] s_ExampleData =
    {
      "00100",
      "11110",
      "10110",
      "10111",
      "10101",
      "01111",
      "00111",
      "11100",
      "10000",
      "11001",
      "00010",
      "01010",
    };

    [Test]
    public void PartATest()
    {
      Assert.AreEqual(198, ReportParser.CalculatePowerConsumption(s_ExampleData));
    }

    [Test]
    public void PartBTest()
    {
      Assert.AreEqual(230, ReportParser.CalculateLifeSupportRating(s_ExampleData));
    }
  }
}
