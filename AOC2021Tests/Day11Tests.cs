using AoC2021.Day11;
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
  public class Day11Tests
  {
    [Test]
    public void TestDay11PartOne()
    {
      Assert.AreEqual(1656, DumboOctopus.CountFlashesDuring100Steps(Data.Day11ExampleData));
    }

    [Test]
    public void TestDay11PartTwo()
    {
      Assert.AreEqual(195, DumboOctopus.FindFirstStepAllItemsFlash(Data.Day11ExampleData));
    }
  }
}
