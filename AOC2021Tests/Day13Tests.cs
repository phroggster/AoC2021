using AoC2021.Day13;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Tests
{
  [TestFixture,
    TestOf(typeof(TransparentOrigami))]
  public class Day13Tests
  {
    [Test]
    public void TestSingleFoldOrigami()
    {
      Assert.AreEqual(17, TransparentOrigami.PartOne(Data.Day13ExampleData));
    }

    [Test]
    public void TestOrigamiPart2()
    {
      Assert.AreEqual(foldedStrings[2], TransparentOrigami.PartTwo(Data.Day13ExampleData));
    }

    [TestCaseSource(nameof(GraphTestData))]
    public void TestDebuggingOutput(string expectedResult, int nFolds, IEnumerable<string> dataSet)
    {
      Assert.AreEqual(expectedResult, new Graph(dataSet).Fold(nFolds).DebugDisplay);
    }

    static readonly string[] foldedStrings = new[]
    {
      @"...#..#..#.
....#......
...........
#..........
...#....#.#
...........
...........
...........
...........
...........
.#....#.##.
....#......
......#...#
#..........
#.#........
",
      @"#.##..#..#.
#...#......
......#...#
#...#......
.#.#..#.###
...........
...........
",
      @"#####
#...#
#...#
#...#
#####
.....
.....
"
    };

    static readonly object[] GraphTestData = new[]
    {
      new object[] { foldedStrings[0], 0, Data.Day13ExampleData },
      new object[] { foldedStrings[1], 1, Data.Day13ExampleData },
      new object[] { foldedStrings[2], 2, Data.Day13ExampleData },
    };
  }
}
