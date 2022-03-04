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
    public void TestSlidingWindowPairsExtInt(IEnumerable<WindowPair<int>> expected, int[] testData)
    {
      Assert.That(testData.SlidingWindowPairs(), Is.EqualTo(expected.Select(exp => (exp.x, exp.y))));
    }

    [Test,
      TestCaseSource(nameof(CharPairCases)),
      TestOf(nameof(SlidingWindowExtensions.SlidingWindowPairs))]
    public void TestSlidingWindowPairsExtChar(IEnumerable<WindowPair<char>> expected, char[] testData)
    {
      Assert.That(testData.SlidingWindowPairs(), Is.EqualTo(expected.Select(exp => (exp.x, exp.y))));
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
    public void TestSlidingWindowSetsExtInt(IEnumerable<WindowPair<int>> expected, int[] testData)
    {
      Assert.That(testData.SlidingWindowSets(), Is.EqualTo(expected.Select(exp => (exp.x, exp.y))));
    }

    [Test,
      TestCaseSource(nameof(CharSetCases)),
      TestOf(nameof(SlidingWindowExtensions.SlidingWindowSets))]
    public void TestSlidingWindowSetsExtChar(IEnumerable<WindowPair<char>> expected, char[] testData)
    {
      Assert.That(testData.SlidingWindowSets(), Is.EqualTo(expected.Select(exp => (exp.x, exp.y))));
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
    public void TestReverseSlidingWindowSetsExtInt(IEnumerable<WindowPair<int>> expectedSequence, int[] testData)
    {
      // uses the same test data as TestSlidingWindowSetsExtInt
      Assert.That(testData.ReverseSlidingWindowSets(),
        Is.EqualTo(expectedSequence.Select(exp => (exp.x, exp.y)).Reverse()));
    }

    [Test,
      TestCaseSource(nameof(CharSetCases)),
      TestOf(nameof(SlidingWindowExtensions.ReverseSlidingWindowSets))]
    public void TestReverseSlidingWindowSetsExtInt(IEnumerable<WindowPair<char>> expectedSequence, char[] testData)
    {
      // uses the same test data as TestSlidingWindowSetsExtChar
      Assert.That(testData.ReverseSlidingWindowSets(),
        Is.EqualTo(expectedSequence.Select(exp => (exp.x, exp.y)).Reverse()));
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
        new WindowPair<int>[] { new(1,2), new(2,3), new(3,4), new(4,5), new(5,6) },
        new [] { 1, 2, 3, 4, 5, 6 }
      },
      new object[] {
        new WindowPair<int> [] { new(6,4), new(4,2), new(2,1), new(1,3), new(3,5) },
        new [] { 6, 4, 2, 1, 3, 5 }
      },
    };

    static readonly object[] CharPairCases = new object[]
    {
      new object[] {
        new WindowPair<char>[] { new('a','b'), new('b','C'), new('C','d'), new('d','e'), new('e','f') },
        new [] { 'a', 'b', 'C', 'd', 'e', 'f' }
      },
      new object[] {
        new WindowPair<char>[] { new('F','D'), new('D','B'), new('B','A'), new('A','c'), new('c','e') },
        new [] { 'F', 'D', 'B', 'A', 'c', 'e' }
      }
    };

    static readonly object[] IntSetCases = new object[]
    {
      new object[] {
        new WindowPair<int>[] { new(1,2), new(3,4), new(5,6) },
        new[] { 1, 2, 3, 4, 5, 6 }
      },
      new object[] {
        new WindowPair<int>[] { new(6, 4), new(2, 1), new(3, 5) },
        new [] { 6, 4, 2, 1, 3, 5 }
      }
    };

    static readonly object[] CharSetCases = new object[]
    {
      new object[] {
          new WindowPair<char>[] { new('a','b'), new('C','d'), new('e','f') },
          new [] { 'a', 'b', 'C', 'd', 'e', 'f' }
        },
      new object[] {
        new WindowPair<char>[] { new('F','D'), new('B','A'), new('c','e') },
        new [] { 'F', 'D', 'B', 'A', 'c', 'e' }
      },
    };


    // Something about nunit/runner/visual studio isn't happy with anonymous types for
    // parameters like: (int x, int y) so this is a silly wrapper for testing.
    public readonly struct WindowPair<T>
    {
      public readonly T x;
      public readonly T y;

      public WindowPair(T x, T y)
      {
        this.x = x;
        this.y = y;
      }
    }
  }
}
