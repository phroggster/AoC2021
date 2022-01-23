using AoC2021.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day14
{
  public static class ExtendedPolymer
  {
    static void Main(string[] _)
    {
      // What do you get if you take the quantity of the most common
      // element and subtract the quantity of the least common element ...

      // ... after 10 steps?
      Console.WriteLine($"10-step EXAMPLE: {InspectPolymerAfterTenCycles(Data.Day14ExampleData)}");
      Console.WriteLine($"10-step ACTUAL:  {InspectPolymerAfterTenCycles(Data.Day14ActualData)}");
      Console.WriteLine("-----------------|----");

      // ... after 40 steps?
      Console.WriteLine($"40-step EXAMPLE: {InspectPolymerAfterFortyCycles(Data.Day14ExampleData)}");
      Console.WriteLine($"40-step ACTUAL:  {InspectPolymerAfterFortyCycles(Data.Day14ActualData)}");
    }

    public static long InspectPolymerAfterTenCycles(IEnumerable<string> data)
    {
      return new PolymerCycler(data)
        .React(10);
    }

    public static long InspectPolymerAfterFortyCycles(IEnumerable<string> data)
    {
      return new PolymerCycler(data)
        .React(40);
    }
  }

  internal class PolymerCycler
  {
    public PolymerCycler(IEnumerable<string> input)
    {
      Template = input.First();

      Rules = input
        .Skip(2)
        .Select(line =>
        {
          var split = line.Split(" -> ");
          return ((split[0][0], split[0][1]), split[1][0]);
        })
        .ToDictionary(p => p.Item1, p => p.Item2);

      _elements = Rules.Select(kvp => kvp.Value)
        .Concat(Template.ToCharArray())
        .Distinct()
        .OrderBy(c => c)
        .ToArray();
    }

    /// <summary>The pair insertion rules for the reaction.</summary>
    public Dictionary<(char, char), char> Rules { get; init; }

    /// <summary>The starting point of the reaction.</summary>
    public string Template { get; init; }

    /// <summary>A collection of all the elements that may be used during a reaction.</summary>
    public IEnumerable<char> Elements => _elements;
    private readonly char[] _elements;


    public long React(int nCycles = 10)
    {
      var elemCounts = Elements.ToDictionary(l => l, n => 0L);
      foreach (var c in Template.ToCharArray())
        elemCounts[c]++;

      var rxCounter = new ReactionCounter(Elements);
      foreach (var bond in Template.SlidingPairs())
        rxCounter[bond]++;

      for (int n = 0; n < nCycles; n++)
      {
        var stepCnts = new ReactionCounter(Elements);

        foreach (var pair in rxCounter
          .Where(kvp => kvp.Value > 0)
          .Select(kvp => kvp.Key))
        {
          long pairCnt = rxCounter[pair];
          char newElem = Rules[pair];

          // bond new to left
          stepCnts[(pair.left, newElem)] += pairCnt;
          // bond new to right
          stepCnts[(newElem, pair.right)] += pairCnt;
          // and trim the existing bond between them
          stepCnts[(pair.left, pair.right)] -= pairCnt;

          elemCounts[newElem] += pairCnt;
        }

        rxCounter += stepCnts;
      }

      return elemCounts.Values.Max() - elemCounts.Values.Min();
    }
  }

  internal class ReactionCounter
    : Dictionary<(char left, char right), long>
  {
    public ReactionCounter(IEnumerable<char> elementsToMonitor)
      : base(elementsToMonitor.Count() * elementsToMonitor.Count())
    {
      foreach (var a in elementsToMonitor)
        foreach (var b in elementsToMonitor)
          Add((a, b), 0L);
    }

    public static ReactionCounter operator +(ReactionCounter a, ReactionCounter b)
    {
      var charSet = a.Keys.Select(k => k.left).Distinct().OrderBy(c => c);
      var result = new ReactionCounter(charSet);
      foreach (var idx in a.Keys)
      {
        result[idx] = a[idx] + b[idx];
      }
      return result;
    }
  }
}
