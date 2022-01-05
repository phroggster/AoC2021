using AoC2021.Day04;
using NUnit.Framework;
using System;

namespace AoC2021.Tests
{
  public class Day04Tests
  {
    [Test]
    public void PartATest()
    {
      Assert.AreEqual(4512, SquidGame.GetScoreOfWinningBoard(Data.ExampleDataDay04));
    }

    [Test]
    public void PartBTest()
    {
      Assert.AreEqual(1924, SquidGame.GetScoreOfWorstBoard(Data.ExampleDataDay04));
    }
  }
}
