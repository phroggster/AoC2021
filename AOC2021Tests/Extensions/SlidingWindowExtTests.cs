using AoC2021.Extensions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests.Extensions
{
  [TestFixture,
    TestOf(typeof(SlidingWindowExtensions))]
  public class SlidingWindowExtTests
  {
    #region SlidingWindowPairs
    [Test,
      TestCaseSource(nameof(IntPairCases)),
      TestOf(nameof(SlidingWindowExtensions.SlidingWindowPairs))]
    public void TestSlidingWindowPairsExtInt((int, int)[] expected, int[] testData)
    {
      Utility.SequenceEquals(expected, testData.SlidingWindowPairs());
    }

    [Test,
      TestCaseSource(nameof(CharPairCases)),
      TestOf(nameof(SlidingWindowExtensions.SlidingWindowPairs))]
    public void TestSlidingWindowPairsExtChar((char, char)[] expected, char[] testData)
    {
      Utility.SequenceEquals(expected, testData.SlidingWindowPairs());
    }

    [Test,
      TestOf(nameof(SlidingWindowExtensions.SlidingWindowPairs))]
    public void TestSlidingWindowPairsExtExceptions()
    {
      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.SlidingWindowPairs<int>(null));
      Assert.IsEmpty(SlidingWindowExtensions.SlidingWindowPairs(Enumerable.Empty<int>()));

      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.SlidingWindowPairs<char>(null));
      Assert.IsEmpty(SlidingWindowExtensions.SlidingWindowPairs(Enumerable.Empty<char>()));

      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.SlidingWindowPairs<object>(null));
      Assert.IsEmpty(SlidingWindowExtensions.SlidingWindowPairs(Enumerable.Empty<object>()));
    }
    #endregion

    #region SlidingWindowSets
    [Test,
      TestCaseSource(nameof(IntSetCases)),
      TestOf(nameof(SlidingWindowExtensions.SlidingWindowSets))]
    public void TestSlidingWindowSetsExtInt((int, int)[] expected, int[] testData)
    {
      Utility.SequenceEquals(expected, testData.SlidingWindowSets());
    }

    [Test,
      TestCaseSource(nameof(CharSetCases)),
      TestOf(nameof(SlidingWindowExtensions.SlidingWindowSets))]
    public void TestSlidingWindowSetsExtChar((char, char)[] expected, char[] testData)
    {
      Utility.SequenceEquals(expected, testData.SlidingWindowSets());
    }

    [Test,
      TestOf(nameof(SlidingWindowExtensions.SlidingWindowSets))]
    public void TestSlidingWindowSetsExtExceptions()
    {
      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.SlidingWindowSets<int>(null));
      Assert.IsEmpty(SlidingWindowExtensions.SlidingWindowSets(Enumerable.Empty<int>()));

      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.SlidingWindowSets<int>(null));
      Assert.IsEmpty(SlidingWindowExtensions.SlidingWindowSets(Enumerable.Empty<char>()));

      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.SlidingWindowSets<object>(null));
      Assert.IsEmpty(SlidingWindowExtensions.SlidingWindowSets(Enumerable.Empty<object>()));
    }
    #endregion

    #region ReverseSlidingWindowSets
    [Test,
      TestCaseSource(nameof(IntSetCases)),
      TestOf(nameof(SlidingWindowExtensions.ReverseSlidingWindowSets))]
    public void TestReverseSlidingWindowSetsExtInt((int, int)[] expected, int[] testData)
    {
      // uses the same test data as TestSlidingWindowSetsExtInt
      Utility.SequenceEquals(expected.Reverse(),
        testData.ReverseSlidingWindowSets());
    }

    [Test,
      TestCaseSource(nameof(CharSetCases)),
      TestOf(nameof(SlidingWindowExtensions.ReverseSlidingWindowSets))]
    public void TestReverseSlidingWindowSetsExtInt((char, char)[] expected, char[] testData)
    {
      // uses the same test data as TestSlidingWindowSetsExtChar
      Utility.SequenceEquals(expected.Reverse(),
        testData.ReverseSlidingWindowSets());
    }

    [Test,
      TestOf(nameof(SlidingWindowExtensions.ReverseSlidingWindowSets))]
    public void TestReverseSlidingWindowSetsExtExceptions()
    {
      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.ReverseSlidingWindowSets<int>(null));
      Assert.IsEmpty(SlidingWindowExtensions.ReverseSlidingWindowSets(Enumerable.Empty<int>()));

      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.ReverseSlidingWindowSets<int>(null));
      Assert.IsEmpty(SlidingWindowExtensions.ReverseSlidingWindowSets(Enumerable.Empty<char>()));

      Assert.Throws<ArgumentNullException>(()
        => SlidingWindowExtensions.ReverseSlidingWindowSets<object>(null));
      Assert.IsEmpty(SlidingWindowExtensions.ReverseSlidingWindowSets(Enumerable.Empty<object>()));
    }
    #endregion

    static readonly object[] IntPairCases = new object[]
    {
      new object[] {
        new [] { (1,2), (2,3), (3,4), (4,5), (5,6) },
        new [] { 1, 2, 3, 4, 5, 6 }
      },
      new object[] {
        new [] { (6,4), (4,2), (2,1), (1,3), (3,5) },
        new [] { 6, 4, 2, 1, 3, 5 }
      },
    };

    static readonly object[] CharPairCases = new object[]
    {
      new object[] {
        new [] { ('a','b'), ('b','C'), ('C','d'), ('d','e'), ('e','f') },
        new [] { 'a', 'b', 'C', 'd', 'e', 'f' }
      },
      new object[] {
        new [] { ('F','D'), ('D','B'), ('B','A'), ('A','c'), ('c','e') },
        new [] { 'F', 'D', 'B', 'A', 'c', 'e' }
      }
    };

    static readonly object[] IntSetCases = new object[]
    {
      new object[] {
        new[] { (1,2), (3,4), (5,6) },
        new[] { 1, 2, 3, 4, 5, 6 }
      },
      new object[] {
        new [] { (6, 4), (2, 1), (3, 5) },
        new [] { 6, 4, 2, 1, 3, 5 }
      }
    };

    static readonly object[] CharSetCases = new object[]
    {
      new object[] {
          new [] { ('a','b'), ('C','d'), ('e','f') },
          new [] { 'a', 'b', 'C', 'd', 'e', 'f' }
        },
      new object[] {
        new [] { ('F','D'), ('B','A'), ('c','e') },
        new [] { 'F', 'D', 'B', 'A', 'c', 'e' }
      },
    };
  }
}
