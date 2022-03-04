using AoC2021.Day09;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AoC2021.Tests.Day09
{
  [TestFixture]
  public class Day09Tests
  {
    [Test]
    public void TestSmokeBasinPartA()
    {
      Assert.AreEqual(15, SmokeBasin.SummateLowPointsRisk(Data.Day09Exampledata));
    }

    [Test]
    public void TestSmokeBasinPartB()
    {
      Assert.AreEqual(1134, SmokeBasin.ThreeLargestBasinsSizeProduct(Data.Day09Exampledata));
    }
  }
}
