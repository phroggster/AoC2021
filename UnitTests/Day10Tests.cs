using AoC2021.Day10;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AoC2021.Tests.Day10
{
  [TestFixture]
  public class Day10Tests
  {
    [Test]
    public void TestSyntaxScoring()
    {
      Assert.AreEqual(26397L, SyntaxScoring.ScorePartOne(Data.Day10ExampleData));
    }

    [Test]
    public void TestAutoCarrotScoring()
    {
      Assert.AreEqual(288957L, SyntaxScoring.ScorePartTwo(Data.Day10ExampleData));
    }
  }
}
