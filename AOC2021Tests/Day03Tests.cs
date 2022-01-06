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
      Assert.AreEqual(198, BinaryDiagnostic.CalculatePowerConsumption(Data.Day03ExampleData));
    }

    [Test]
    public void PartBTest()
    {
      Assert.AreEqual(230, BinaryDiagnostic.CalculateLifeSupportRating(Data.Day03ExampleData));
    }
  }
}
