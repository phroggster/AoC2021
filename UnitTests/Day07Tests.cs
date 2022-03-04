using AoC2021.Day07;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AoC2021.Tests.Day07
{
  [TestFixture,
    TestOf(typeof(WhalesTreachery))]
  public class Day07Tests
  {
    [Test,
      TestOf(nameof(WhalesTreachery.CalcMedianFuelCosts))]
    public void TestMedianFuelCosts()
    {
      Assert.AreEqual(37, WhalesTreachery.CalcMedianFuelCosts(Data.Day07ExampleData));
    }

    [Test,
      TestOf(nameof(WhalesTreachery.CalcTriangularFuelCosts))]
    public void TestTriangularFuelCosts()
    {
      Assert.AreEqual(168, WhalesTreachery.CalcTriangularFuelCosts(Data.Day07ExampleData));
    }

    [Test,
      TestOf(nameof(WhalesTreachery.GetTriangularNumber))]
    public void TestTriangularNumberGenerator()
    {
      Assert.Multiple(() =>
      {
        Assert.AreEqual(0, WhalesTreachery.GetTriangularNumber(0));
        Assert.AreEqual(1, WhalesTreachery.GetTriangularNumber(1));
        Assert.AreEqual(3, WhalesTreachery.GetTriangularNumber(2));
        Assert.AreEqual(6, WhalesTreachery.GetTriangularNumber(3));

        Assert.AreEqual(10, WhalesTreachery.GetTriangularNumber(-4));
        Assert.AreEqual(15, WhalesTreachery.GetTriangularNumber(-5));
        Assert.AreEqual(21, WhalesTreachery.GetTriangularNumber(-6));
        Assert.AreEqual(28, WhalesTreachery.GetTriangularNumber(-7));

        Assert.AreEqual(8256, WhalesTreachery.GetTriangularNumber(128));
        Assert.AreEqual(8385, WhalesTreachery.GetTriangularNumber(129));
        Assert.AreEqual(8515, WhalesTreachery.GetTriangularNumber(130));
      });
    }
  }
}
