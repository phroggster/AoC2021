using AoC2021.Day15;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests
{
  [TestFixture]
  public class Day15Tests
  {
    [Test,
      TestOf(typeof(ChitonStar)),
      TestOf(nameof(ChitonStar.FindLeastRisk))]
    public void TestSinglePathing()
    {
      Assert.AreEqual(40, ChitonStar.FindLeastRisk(Data.Day15ExampleData));
    }

    [Test,
      TestOf(typeof(ChitonStar)),
      TestOf(nameof(ChitonStar.FindLeastRiskX5))]
    public void TestQuintuplePathing()
    {
      Assert.AreEqual(315, ChitonStar.FindLeastRiskX5(Data.Day15ExampleData));
    }

    [Test,
      TestOf(typeof(AStarPathing)),
      TestOf(nameof(AStarPathing.ElementAt)),
      TestCaseSource(nameof(InconclusiveData))]
    // Tests non-interpolated results from PathFinder.ElementAt
    public void TestNonTiledCases(int expectedInt, int x, int y)
    {
      if (expectedInt < 0 || expectedInt > byte.MaxValue)
        throw new ArgumentOutOfRangeException(nameof(expectedInt));
      var expectedByte = (byte)expectedInt;
      if (expectedByte != expectedInt)
        throw new InvalidCastException();

      var oneX = new AStarPathing(Data.Day15ExampleData, scale: 1);
      var fiveX = new AStarPathing(Data.Day15ExampleData, scale: 5);
      var oneR = oneX.ElementAt(x, y);
      var fiveR = fiveX.ElementAt(x, y);

      if (expectedByte != oneR)
      {
        Assert.Inconclusive($"Untiled result was {oneR}; expecting {expectedByte}.");
      }
      else if (expectedByte != fiveR)
      {
        Assert.Inconclusive($"Tiled result was {fiveR}; expecting {expectedByte}.");
      }
    }

    [Test,
      TestOf(typeof(AStarPathing)),
      TestOf(nameof(AStarPathing.ElementAt)),
      TestCaseSource(nameof(TilingData))]
    public void TestTiledElementAt(int expectedInt, int x, int y)
    {
      var expectedByte = (byte)expectedInt;
      if (expectedByte != expectedInt)
        throw new ArgumentOutOfRangeException(nameof(expectedInt));

      var pf = new AStarPathing(Data.Day15ExampleData, scale: 5);
      Assert.AreEqual(expectedByte, pf.ElementAt(x, y));
    }

    static readonly object[] InconclusiveData = new[]
    {
      // Row 0: 1163751742
      new object[] { 1, 0, 0 },
      new object[] { 1, 1, 0 },
      new object[] { 6, 2, 0 },
      new object[] { 3, 3, 0 },
      new object[] { 7, 4, 0 },
      new object[] { 5, 5, 0 },
      new object[] { 1, 6, 0 },
      new object[] { 7, 7, 0 },
      new object[] { 4, 8, 0 },
      new object[] { 2, 9, 0 },
      // Row 8: 1293138521
      new object[] { 1, 0, 8 },
      new object[] { 2, 1, 8 },
      new object[] { 9, 2, 8 },
      new object[] { 3, 3, 8 },
      new object[] { 1, 4, 8 },
      new object[] { 3, 5, 8 },
      new object[] { 8, 6, 8 },
      new object[] { 5, 7, 8 },
      new object[] { 2, 8, 8 },
      new object[] { 1, 9, 8 }
    };

    static readonly object[] TilingData = new[]
    {
      // top row:
      // 1163751742 2274862853 3385973964 4496184175 5517295286
      new object[] { 1,  0, 0 }, // inconclusive
      new object[] { 2,  9, 0 }, // inconclusive
      new object[] { 2, 10, 0 },
      new object[] { 3, 19, 0 },
      new object[] { 3, 20, 0 },
      new object[] { 4, 29, 0 },
      new object[] { 4, 30, 0 },
      new object[] { 5, 39, 0 },
      new object[] { 5, 40, 0 },
      new object[] { 6, 49, 0 },

      // second from the non-tiled bottom row:
      // 1293138521 2314249632 3425351743 4536462854 5647573965
      new object[] { 1,  0, 8 }, // inconclusive
      new object[] { 1,  9, 8 }, // inconclusive
      new object[] { 2, 10, 8 },
      new object[] { 2, 19, 8 },
      new object[] { 3, 20, 8 },
      new object[] { 3, 29, 8 },
      new object[] { 4, 30, 8 },
      new object[] { 4, 39, 8 },
      new object[] { 5, 40, 8 },
      new object[] { 5, 49, 8 },

      // tiled bottom row:
      // 6755488935 7866599146 8977611257 9188722368 1299833479
      new object[] { 6,  0, 49 },
      new object[] { 5,  9, 49 },
      new object[] { 7, 10, 49 },
      new object[] { 6, 19, 49 },
      new object[] { 8, 20, 49 },
      new object[] { 7, 29, 49 },
      new object[] { 9, 30, 49 },
      new object[] { 8, 39, 49 },
      new object[] { 1, 40, 49 }, // fail
      new object[] { 9, 49, 49 }
    };
  }
}
