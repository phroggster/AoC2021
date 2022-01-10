using AoC2021.Day07;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AoC2021.Tests
{
  [TestFixture]
  public class Day07Tests
  {
    [Test]
    public void TestMedianFuelCosts()
    {
      Assert.AreEqual(37, WhalesTreachery.CalcMedianFuelCosts(Data.Day07ExampleData));
    }

    [Test]
    public void TestTriangularFuelCosts()
    {
      Assert.AreEqual(168, WhalesTreachery.CalcTriangularFuelCosts(Data.Day07ExampleData));
    }


    // various groups of data, odd and even set sizes, designed for prodding.
    private static readonly int[] s_gpA = new[] { 83, 91, 94, 89, 89, 96, 91, 92, 90 };
    private static readonly int[] s_gpB = new[] { 91, 90, 81, 83, 84, 83, 88, 91, 89, 84 };
    private static readonly int[] s_gpC = new[] { 101, 100, 91, 93, 96, 95, 94 };
    private static readonly int[] s_gpD = new[] { 78, 82, 81, 77, 79, 81, 80, 81 };

    private const double FPTolerance = 0.00001d;

    [Test]
    public void TestTriangularNumberGenerator()
    {
      Assert.Multiple(() =>
      {
        Assert.AreEqual(0, Extensions.GetTriangularNumber(0));
        Assert.AreEqual(1, Extensions.GetTriangularNumber(1));
        Assert.AreEqual(3, Extensions.GetTriangularNumber(2));
        Assert.AreEqual(6, Extensions.GetTriangularNumber(3));

        Assert.AreEqual(10, Extensions.GetTriangularNumber(-4));
        Assert.AreEqual(15, Extensions.GetTriangularNumber(-5));
        Assert.AreEqual(21, Extensions.GetTriangularNumber(-6));
        Assert.AreEqual(28, Extensions.GetTriangularNumber(-7));

        Assert.AreEqual(8256, Extensions.GetTriangularNumber(128));
        Assert.AreEqual(8385, Extensions.GetTriangularNumber(129));
        Assert.AreEqual(8515, Extensions.GetTriangularNumber(130));
      });
    }

    [Test]
    public void TestUtilityMedian()
    {
      Assert.Multiple(() =>
      {
        Assert.AreEqual(2.0d, Extensions.Median(Data.Day07ExampleData), FPTolerance);

        Assert.AreEqual(91.0d, Extensions.Median(s_gpA), FPTolerance);
        Assert.AreEqual(86.0d, Extensions.Median(s_gpB), FPTolerance);
        Assert.AreEqual(95.0d, Extensions.Median(s_gpC), FPTolerance);
        Assert.AreEqual(80.5d, Extensions.Median(s_gpD), FPTolerance);

        Assert.AreEqual(89.0d, Extensions.Median(s_gpA.Concat(s_gpB)), FPTolerance);
        Assert.AreEqual(92.5d, Extensions.Median(s_gpA.Concat(s_gpC)), FPTolerance);
        Assert.AreEqual(83.0d, Extensions.Median(s_gpA.Concat(s_gpD)), FPTolerance);
        Assert.AreEqual(91.0d, Extensions.Median(s_gpB.Concat(s_gpC)), FPTolerance);
        Assert.AreEqual(82.5d, Extensions.Median(s_gpB.Concat(s_gpD)), FPTolerance);
        Assert.AreEqual(82.0d, Extensions.Median(s_gpC.Concat(s_gpD)), FPTolerance);

        Assert.AreEqual(91.0d, Extensions.Median(s_gpA.Concat(s_gpB).Concat(s_gpC)), FPTolerance);
        Assert.AreEqual(84.0d, Extensions.Median(s_gpA.Concat(s_gpB).Concat(s_gpD)), FPTolerance);
        Assert.AreEqual(84.0d, Extensions.Median(s_gpB.Concat(s_gpC).Concat(s_gpD)), FPTolerance);

        Assert.AreEqual(89.0d, Extensions.Median(s_gpA.Concat(s_gpB).Concat(s_gpC).Concat(s_gpD)), FPTolerance);
      });
    }

    [Test]
    public void TestUtilityRoundedMedian()
    {
      Assert.Multiple(() =>
      {
        Assert.AreEqual(2, Extensions.RoundedMedian(Data.Day07ExampleData));

        Assert.AreEqual(91, Extensions.RoundedMedian(s_gpA));
        Assert.AreEqual(86, Extensions.RoundedMedian(s_gpB));
        Assert.AreEqual(95, Extensions.RoundedMedian(s_gpC));
        Assert.AreEqual(81, Extensions.RoundedMedian(s_gpD));

        Assert.AreEqual(89, Extensions.RoundedMedian(s_gpA.Concat(s_gpB)));
        Assert.AreEqual(93, Extensions.RoundedMedian(s_gpA.Concat(s_gpC)));
        Assert.AreEqual(83, Extensions.RoundedMedian(s_gpA.Concat(s_gpD)));
        Assert.AreEqual(91, Extensions.RoundedMedian(s_gpB.Concat(s_gpC)));
        Assert.AreEqual(83, Extensions.RoundedMedian(s_gpB.Concat(s_gpD)));
        Assert.AreEqual(82, Extensions.RoundedMedian(s_gpC.Concat(s_gpD)));

        Assert.AreEqual(91, Extensions.RoundedMedian(s_gpA.Concat(s_gpB).Concat(s_gpC)));
        Assert.AreEqual(84, Extensions.RoundedMedian(s_gpA.Concat(s_gpB).Concat(s_gpD)));
        Assert.AreEqual(84, Extensions.RoundedMedian(s_gpB.Concat(s_gpC).Concat(s_gpD)));

        Assert.AreEqual(89, Extensions.RoundedMedian(s_gpA.Concat(s_gpB).Concat(s_gpC).Concat(s_gpD)));
      });
    }
  }
}
