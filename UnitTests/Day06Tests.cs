using AoC2021.Day06;
using NUnit.Framework;
using System;

namespace AoC2021.Tests.Day06
{
  public class Day06Tests
  {
    [Test]
    public void TestPopulationAfter80Days()
    {
      Assert.AreEqual(5934L, Lanternfish.SimulateDays(Data.Day06ExampleData, 80));
    }

    [Test]
    public void TestPopulationAfter256Days()
    {
      Assert.AreEqual(26984457539L, Lanternfish.SimulateDays(Data.Day06ExampleData, 256));
    }
  }
}
