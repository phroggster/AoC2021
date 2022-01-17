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

    [Test]
    public void TestTriangularNumberGenerator()
    {
      Assert.Multiple(() =>
      {
        Assert.AreEqual(0, Day07.Extensions.GetTriangularNumber(0));
        Assert.AreEqual(1, Day07.Extensions.GetTriangularNumber(1));
        Assert.AreEqual(3, Day07.Extensions.GetTriangularNumber(2));
        Assert.AreEqual(6, Day07.Extensions.GetTriangularNumber(3));

        Assert.AreEqual(10, Day07.Extensions.GetTriangularNumber(-4));
        Assert.AreEqual(15, Day07.Extensions.GetTriangularNumber(-5));
        Assert.AreEqual(21, Day07.Extensions.GetTriangularNumber(-6));
        Assert.AreEqual(28, Day07.Extensions.GetTriangularNumber(-7));

        Assert.AreEqual(8256, Day07.Extensions.GetTriangularNumber(128));
        Assert.AreEqual(8385, Day07.Extensions.GetTriangularNumber(129));
        Assert.AreEqual(8515, Day07.Extensions.GetTriangularNumber(130));
      });
    }
  }
}
