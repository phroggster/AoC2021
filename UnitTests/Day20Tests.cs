using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2021.Day20;
using AoC2021.Tests.Extensions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using GlmSharp;

namespace AoC2021.Tests.Day20
{
  [TestFixture,
    TestOf(typeof(TrenchMap))]
  public class Day20Tests
  {
    [Test,
      TestCase(35, Source5x5),
      TestOf(nameof(TrenchMap.EnhanceTwiceAndCount))]
    public void TestTwoEnhanceCount(int nExpectedCount, string slzdAlgoAndMap)
    {
      Assert.That(TrenchMap.EnhanceTwiceAndCount(slzdAlgoAndMap), Is.EqualTo(nExpectedCount));
    }

    [Test,
      TestCase(3351, Source5x5),
      TestOf(nameof(TrenchMap.EnhanceFiftyAndCount))]
    public void TestFiftyEnhanceCount(int nExpectedCount, string slzdAlgoAndMap)
    {
      Assert.That(TrenchMap.EnhanceFiftyAndCount(slzdAlgoAndMap), Is.EqualTo(nExpectedCount));
    }


    [Test,
      TestCaseSource(nameof(EnhancementTestCases)),
      TestOf(nameof(TrenchMap.Enhance))]
    public void TestEnhancements(string slzdAlgoAndMap, int nIterations, ivec2[] expIllumPixels)
    {
      var testee = new TrenchMap(slzdAlgoAndMap)
        .Enhance(nIterations);

      Assert.That(testee, Is.EquivalentTo(expIllumPixels));
    }

    private static object[] EnhancementTestCases = new object[]
    {
      new object[] { Source15x15, 1, new ivec2[]
      {
        new(5, 4), new(6, 4), new(8, 4), new(9, 4),
        new(4, 5), new(7, 5), new(9, 5),
        new(4, 6), new(5, 6), new(7, 6), new(10, 6),
        new(4, 7), new(5, 7), new(6, 7), new(7, 7), new(10, 7),
        new(5, 8), new(8, 8), new(9, 8),
        new(6, 9), new(7, 9), new(10, 9),
        new(7, 10), new(9, 10)
      } },
      new object[] { Source15x15_Rev1, 1, new ivec2[]
      {
        new(10, 3),
        new(4, 4), new(7, 4), new(9, 4),
        new(3, 5), new(5, 5), new(9, 5), new(10, 5), new(11, 5),
        new(3, 6), new(7, 6), new(8, 6), new(10, 6),
        new(3, 7), new(9, 7), new(11, 7),
        new(4, 8), new(6, 8), new(7, 8), new(8, 8), new(9, 8), new(10, 8),
        new(5, 9), new(7, 9), new(8, 9), new(9, 9), new(10, 9), new(11, 9),
        new(6, 10), new(7, 10), new(9, 10), new(10, 10),
        new(7, 11), new(8, 11), new(9, 11)
      } },
      new object[] { Source15x15, 2, new ivec2[]
      {
        new(10, 3),
        new(4, 4), new(7, 4), new(9, 4),
        new(3, 5), new(5, 5), new(9, 5), new(10, 5), new(11, 5),
        new(3, 6), new(7, 6), new(8, 6), new(10, 6),
        new(3, 7), new(9, 7), new(11, 7),
        new(4, 8), new(6, 8), new(7, 8), new(8, 8), new(9, 8), new(10, 8),
        new(5, 9), new(7, 9), new(8, 9), new(9, 9), new(10, 9), new(11, 9),
        new(6, 10), new(7, 10), new(9, 10), new(10, 10),
        new(7, 11), new(8, 11), new(9, 11)
      } },
    };


    [Test,
      TestCaseSource(nameof(DeserializationTestCases)),
      TestOf(nameof(TrenchMap.DeserializeRange))]
    public void TestDeserialization(string slzdAlgoAndMap, int expCount, ivec2[] expIllumPixels)
    {
      Assert.AreEqual(expIllumPixels.Length, expCount);

      var testee = new TrenchMap(slzdAlgoAndMap);
      Assert.That(testee.Algorithm, Is.Not.Null);
      Assert.That(testee.Algorithm, Is.EqualTo(DefaultAlgorithm));
      Assert.That(testee.Count, Is.EqualTo(expCount));
      Assert.That(testee, Is.EquivalentTo(expIllumPixels));
    }

    static readonly object[] DeserializationTestCases = new object[]
    {
      new object[] { Source5x5, 10, new ivec2[]
      {
        new(0, 0), new(3, 0),
        new(0, 1),
        new(0, 2), new(1, 2), new(4, 2),
        new(2, 3),
        new(2, 4), new(3, 4), new(4, 4),
      } },
      new object[] { Source15x15, 10, new ivec2[]
      {
        new(5, 5), new(8, 5),
        new(5, 6),
        new(5, 7), new(6, 7), new(9, 7),
        new(7, 8),
        new(7, 9), new(8, 9), new(9, 9),
      } },
      new object[] { Source15x15_Rev1, 24, new ivec2[]
      {
        new(5, 4), new(6, 4), new(8, 4), new(9, 4),
        new(4, 5), new(7, 5), new(9, 5),
        new(4, 6), new(5, 6), new(7, 6), new(10, 6),
        new(4, 7), new(5, 7), new(6, 7), new(7, 7), new(10, 7),
        new(5, 8), new(8, 8), new(9, 8),
        new(6, 9), new(7, 9), new(10, 9),
        new(7, 10), new(9, 10)
      } },
      new object[] { Source15x15_Rev2, 35, new ivec2[]
      {
        new(10, 3),
        new(4, 4), new(7, 4), new(9, 4),
        new(3, 5), new(5, 5), new(9, 5), new(10, 5), new(11, 5),
        new(3, 6), new(7, 6), new(8, 6), new(10, 6),
        new(3, 7), new(9, 7), new(11, 7),
        new(4, 8), new(6, 8), new(7, 8), new(8, 8), new(9, 8), new(10, 8),
        new(5, 9), new(7, 9), new(8, 9), new(9, 9), new(10, 9), new(11, 9),
        new(6, 10), new(7, 10), new(9, 10), new(10, 10),
        new(7, 11), new(8, 11), new(9, 11)
      } },
    };



    static readonly bool[] DefaultAlgorithm = new bool[]
    {
      false, false, true, false, true, false, false, true, true, true,
      true, true, false, true, false, true, false, true, false, true,
      true, true, false, true, true, false, false, false, false, false,
      true, true, true, false, true, true, false, true, false, false,
      true, true, true, false, true, true, true, true, false, false,
      true, true, true, true, true, false, false, true, false, false,
      false, false, true, false, false, true, false, false, true, true,
      false, false, true, true, true, false, false, true, true, true,
      true, true, true, false, true, true, true, false, false, false,
      true, true, true, true, false, false, true, false, false, true,
      true, true, true, true, false, false, true, true, false, false,
      true, false, true, true, true, true, true, false, false, false,
      true, true, false, true, false, true, false, false, true, false,
      true, true, false, false, true, false, true, false, false, false,
      false, false, false, true, false, true, true, true, false, true,
      true, true, true, true, true, false, true, true, true, false,
      true, true, true, true, false, false, false, true, false, true,
      true, false, true, true, false, false, true, false, false, true,
      false, false, true, true, true, true, true, false, false, false,
      false, false, true, false, true, false, false, false, false, true,
      true, true, false, false, true, false, true, true, false, false,
      false, false, false, false, true, false, false, false, false, false,
      true, false, false, true, false, false, true, false, false, true,
      true, false, false, true, false, false, false, true, true, false,
      true, true, true, true, true, true, false, true, true, true,
      true, false, true, true, true, true, false, true, false, true,
      false, false, false, true, false, false, false, false, false, false,
      false, true, false, false, true, false, true, false, true, false,
      false, false, true, true, true, true, false, true, true, false,
      true, false, false, false, false, false, false, true, false, false,
      true, false, false, false, true, true, false, true, false, true,
      true, false, false, true, false, false, false, true, true, false,
      true, false, true, true, false, false, true, true, true, false,
      true, false, false, false, false, false, false, true, false, true,
      false, false, false, false, false, false, false, true, false, true,
      false, true, false, true, true, true, true, false, true, true,
      true, false, true, true, false, false, false, true, false, false,
      false, false, false, true, true, true, true, false, true, false,
      false, true, false, false, true, false, true, true, false, true,
      false, false, false, false, true, true, false, false, true, false,
      true, true, true, true, false, false, false, false, true, true,
      false, false, false, true, true, false, false, true, false, false,
      false, true, false, false, false, false, false, false, true, false,
      true, false, false, false, false, false, false, false, true, false,
      false, false, false, false, false, false, true, true, false, false,
      true, true, true, true, false, false, true, false, false, false,
      true, false, true, false, true, false, false, false, true, true,
      false, false, true, false, true, false, false, true, true, true,
      false, false, true, true, true, true, true, false, false, false,
      false, false, false, false, false, true, false, false, true, true,
      true, true, false, false, false, false, false, false, true, false,
      false, true
    };

    const string Source5x5 = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

#..#.
#....
##..#
..#..
..###";

    const string Source15x15 = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

...............
...............
...............
...............
...............
.....#..#......
.....#.........
.....##..#.....
.......#.......
.......###.....
...............
...............
...............
...............
...............";

    const string Source15x15_Rev1 = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

...............
...............
...............
...............
.....##.##.....
....#..#.#.....
....##.#..#....
....####..#....
.....#..##.....
......##..#....
.......#.#.....
...............
...............
...............
...............";

    const string Source15x15_Rev2 = @"..#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#..#

...............
...............
...............
..........#....
....#..#.#.....
...#.#...###...
...#...##.#....
...#.....#.#...
....#.#####....
.....#.#####...
......##.##....
.......###.....
...............
...............
...............";
  }
}
