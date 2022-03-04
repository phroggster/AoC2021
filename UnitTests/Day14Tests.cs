using AoC2021.Day14;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests.Day14
{
  [TestFixture,
    TestOf(typeof(ExtendedPolymer))]
  public class Day14Tests
  {
    [Test]
    public void TestTenCycles()
    {
      Assert.AreEqual(1588L, ExtendedPolymer.InspectPolymerAfterTenCycles(Data.Day14ExampleData));
    }

    [Test]
    public void TestFortyCycles()
    {
      Assert.AreEqual(2188189693529L, ExtendedPolymer.InspectPolymerAfterFortyCycles(Data.Day14ExampleData));
    }
  }
}
