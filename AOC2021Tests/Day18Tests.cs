using AoC2021.Day18;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests.Day18
{
  [TestFixture,
    TestOf(typeof(SnailfishHomework))]
  public class OverviewTests
  {
    [Test,
      TestCaseSource(nameof(SumMagCases)),
      TestOf(nameof(SnailfishHomework.SummateAndGetMagnitude))]
    public void TestSummarizedMagnitude(int expectedValue, string[] lines)
    {
      Assert.AreEqual(expectedValue,
        SnailfishHomework.SummateAndGetMagnitude(lines));
    }

    [Test,
      TestCaseSource(nameof(MaxSumCases)),
      TestOf(nameof(SnailfishHomework.MaxSummationOfAnyTwo))]
    public void TestMaxSummationOfAnyTwo(int expectedResult, string[] lines)
    {
      Assert.AreEqual(expectedResult,
        SnailfishHomework.MaxSummationOfAnyTwo(lines));
    }

    static readonly object[] SumMagCases = new object[]
    {
      new object[] { TData.A.ExpMagnitude, TData.A.SrcLines },
      new object[] { TData.B.ExpMagnitude, TData.B.SrcLines },
    };

    static readonly object[] MaxSumCases = new object[]
    {
      new object[] { TData.B.ExpNonComutMagn, TData.B.SrcLines }
    };
  }

  [TestFixture,
    TestOf(typeof(SnailFish))]
  public class GranularTests
  {
    [Test,
      TestCaseSource(nameof(SummateAndReduceCases))]
    public void TestEnumerableSummateAndReduce(string expectedresult, string[] inputs)
    {
      Assert.AreEqual(expectedresult, inputs
        .Select(str => new SnailFish(str))
        .Sum()
        .Value);
    }

    [Test,
      TestCaseSource(nameof(SumStepThroughCases))]
    public void TestSumOperator(string expectedResult, string[] inputLines)
    {
      var left = new SnailFish(inputLines.First());
      var right = new SnailFish(inputLines.Last());
      var sum = left + right;

      Assert.AreEqual(expectedResult, sum.Value);
    }

    [Test,
      TestCaseSource(nameof(MagnitudeCases)),
      TestOf(nameof(SnailFish.CanReduce))]
    public void TestCannotReduce(int _, string reducedSnailfish)
    {
      var testee = new SnailFish(reducedSnailfish);

      Assert.Multiple(() =>
      {
        Assert.AreEqual(false, testee.CanExplode);
        Assert.AreEqual(false, testee.CanSplit);
        Assert.AreEqual(false, testee.CanReduce);
      });
    }

    [Test,
      TestCaseSource(nameof(MagnitudeCases)),
      TestOf(nameof(SnailFish.GetMagnitude))]
    public void TestGetMagnitude(int expectedResult, string input)
    {
      Assert.AreEqual(expectedResult, new SnailFish(input).GetMagnitude());
    }


    static readonly object[] MagnitudeCases = new object[]
    {
      new object[] { 29, "[9,1]"},
      new object[] { 21, "[1,9]"},
      new object[] { 129, "[[9,1],[1,9]]"},
      new object[] { 143, "[[1,2],[[3,4],5]]" },
      new object[] { 445, "[[[[1,1],[2,2]],[3,3]],[4,4]]" },
      new object[] { 791, "[[[[3,0],[5,3]],[4,4]],[5,5]]" },
      new object[] { 1137, "[[[[5,0],[7,4]],[5,5]],[6,6]]" },
      new object[] { 1384, "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]" },
      new object[] { TData.A.ExpMagnitude, TData.A.ExpSum },
      new object[] { TData.B.ExpMagnitude, TData.B.ExpSum },
      // ReversedSlidingWindow never used to behave with this one.
      new object[] { 2478, "[[[[0,7],[8,8]],[[0,6],9]],[[6,[6,6]],[[7,0],[8,9]]]]" },
    };

    static readonly object[] SumStepThroughCases = new object[]
    {
      new object[] { "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]",
        new[] {
          "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
          "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]"
        } },
      new object[] { "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]",
        new[] {
          "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]",
          "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]"
        } },
      new object[] { "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]",
        new[] {
          "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]",
          "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]"
        } },
      new object[] { "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]",
        new[] {
          "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]",
          "[7,[5,[[3,8],[1,4]]]]"
        } },
      new object[] { "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]",
        new[] {
          "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]",
          "[[2,[2,2]],[8,[8,1]]]"
        } },
      new object[] { "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]",
        new[] {
          "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]",
          "[2,9]"
        } },
      new object[] { "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]",
        new[] {
          "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]",
          "[1,[[[9,3],9],[[9,0],[0,7]]]]"
        } },
      new object[] { "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]",
        new[] {
          "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]",
          "[[[5,[7,4]],7],1]"
        } },
      new object[] { "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]",
        new[] {
          "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]",
          "[[[[4,2],2],6],[8,7]]"
        } }
    };

    static readonly object[] SummateAndReduceCases = new object[]
    {
      new object[] { "[[1,2],[[3,4],5]]",
        new[] { "[1,2]", "[[3,4],5]" }},
      new object[] { "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]",
        new[] { "[[[[4,3],4],4],[7,[[8,4],9]]]", "[1,1]" }},
      new object[] { "[[[[1,1],[2,2]],[3,3]],[4,4]]",
        new[] { "[1,1]", "[2,2]", "[3,3]", "[4,4]" }},
      new object[] { "[[[[3,0],[5,3]],[4,4]],[5,5]]",
        new[] { "[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]" }},
      new object[] { "[[[[5,0],[7,4]],[5,5]],[6,6]]",
        new[] { "[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]", "[6,6]" }},
      new object[] { TData.A.ExpSum, TData.A.SrcLines },
      new object[] { TData.B.ExpSum, TData.B.SrcLines },
    };
  }

  internal static class TData
  {
    internal static class A
    {
      public const int ExpMagnitude = 3488;
      public const string ExpSum = "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]";
      public static readonly string[] SrcLines = new[]
      {
      "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
      "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]",
      "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]",
      "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
      "[7,[5,[[3,8],[1,4]]]]",
      "[[2,[2,2]],[8,[8,1]]]",
      "[2,9]",
      "[1,[[[9,3],9],[[9,0],[0,7]]]]",
      "[[[5,[7,4]],7],1]",
      "[[[[4,2],2],6],[8,7]]"
    };
    }

    internal static class B
    {
      public const int ExpMagnitude = 4140;
      public const int ExpNonComutMagn = 3993;
      public const string ExpSum = "[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]";
      public static readonly string[] SrcLines = new[]
      {
      "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]",
      "[[[5,[2,8]],4],[5,[[9,9],0]]]",
      "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]",
      "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]",
      "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]",
      "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]",
      "[[[[5,4],[7,7]],8],[[8,3],8]]",
      "[[9,3],[[9,9],[6,[4,9]]]]",
      "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]",
      "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"
    };
    }
  }
}
