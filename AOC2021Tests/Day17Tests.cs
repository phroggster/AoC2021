using AoC2021.Day17;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests.Day17
{
  [TestFixture,
    TestOf(typeof(TrickShot))]
  public class Day17Tests
  {
    internal const string ExampleTarget = "target area: x=20..30, y=-10..-5";

    [Test,
      TestCase(45, ExampleTarget),
      TestOf(nameof(TrickShot.MaxAltitude))]
    public void TestMaxYIntercept(int expectedValue, string tgtString)
    {
      Assert.AreEqual(expectedValue, TrickShot.MaxAltitude(tgtString));
    }

    [Test,
      TestCase(112, ExampleTarget),
      TestOf(nameof(TrickShot.CountSolutions))]
    public void TestSolutionsCount(int expectedValue, string tgtString)
    {
      Assert.AreEqual(expectedValue, TrickShot.CountSolutions(tgtString));
    }
  }

  [TestFixture,
    TestOf(typeof(FireController))]
  public class FiringTests
  {
    // Used solely by TestSolutions to verify that some known example solutions
    // were caught. No point recomputing this for every solution verification.
    static List<(int dX, int dY, int mY)> Solutions;

    [OneTimeSetUp]
    public static void Init()
    {
      Solutions = new FireController(Day17Tests.ExampleTarget)
        .Solutions()
        .ToList();
    }

    [Test,
      TestCase(20, 30, -10, -5, 10, 5, Day17Tests.ExampleTarget),
      TestCase(14, 50, -267, -225, 36, 42, "target area: x=14..50, y=-267..-225"),
      TestOf(nameof(FireController))]
    public void TestParsing(int exLeft, int exRight, int exBottom, int exTop,
      int exWidth, int exHeight, string tgtString)
    {
      var pt = new FireController(tgtString);
      Assert.IsNotNull(pt);
      Assert.IsNotNull(pt.Target);

      Assert.Multiple(() =>
      {
        Assert.AreEqual(exLeft, pt.Target.Left);
        Assert.AreEqual(exRight, pt.Target.Right);
        Assert.AreEqual(exTop, pt.Target.Top);
        Assert.AreEqual(exBottom, pt.Target.Bottom);
        Assert.AreEqual(exWidth, pt.Target.Width);
        Assert.AreEqual(exHeight, pt.Target.Height);
      });
    }

    [Test,
      TestCase(7, 2, 3),
      TestCase(6, 3, 6),
      TestCase(9, 0, 0),
      TestCase(6, 9, 45),
      TestOf(nameof(FireController.Solutions))]
    public void TestSolutions(int dX, int dY, int maxY)
    {
      Assert.Contains((dX, dY, maxY), Solutions);
    }
  }

  [TestFixture,
    TestOf(typeof(Rectangle))]
  public class RectangleTests
  {
    static Rectangle TestRect;

    [OneTimeSetUp]
    public static void Init()
    {
      TestRect = new Rectangle(20, -5, 10, 5);
    }

    [Test,
      TestCase(true, 20, -5),
      TestCase(true, 30, -5),
      TestCase(true, 25, -7),
      TestCase(true, 20, -10),
      TestCase(true, 30, -10),
      TestCase(false, 19, -5),
      TestCase(false, 31, -5),
      TestCase(false, 20, -11),
      TestCase(false, 30, -4),
      TestCase(false, 0, 0),
      TestOf(nameof(Rectangle.Contains))]
    public void TestRectContains(bool expectedResult, int x, int y)
    {
      Assert.AreEqual(expectedResult, TestRect.Contains(x, y));
    }
  }
}
