using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day10
{
  public readonly struct ScoreValue
  {
    public readonly char OpeningParen;

    public readonly char ClosingParen;

    public readonly int ScorePtOne;

    public readonly int ScorePtTwo;

    public ScoreValue(char open, char close, int scorePtA, int scorePtB)
    {
      OpeningParen = open;
      ClosingParen = close;
      ScorePtOne = scorePtA;
      ScorePtTwo = scorePtB;
    }
  }

  public static class SyntaxScoring
  {
    static void Main(string[] _)
    {
      // What is the total syntax error score for those errors?
      Console.WriteLine($"Syntax score EXAMPLE:      {ScorePartOne(Data.Day10ExampleData)}");
      Console.WriteLine($"Syntax score ACTUAL:       {ScorePartOne(Data.Day10ActualData)}");
      Console.WriteLine("---------------------------|----");

      // What is the middle score?
      Console.WriteLine($"Autocarrot score EXAMPLE:  {ScorePartTwo(Data.Day10ExampleData)}");
      Console.WriteLine($"Autocarrot score ACTUAL:   {ScorePartTwo(Data.Day10ActualData)}");
    }

    public static long ScorePartOne(IEnumerable<string> navData)
    {
      return Compute(navData).a;
    }

    public static long ScorePartTwo(IEnumerable<string> navData)
    {
      return Compute(navData).b;
    }

    private static (long a, long b) Compute(IEnumerable<string> navData)
    {
      var syntaxScore = 0L;
      var autoCarrotScore = new List<long>();

      int lineIdx = -1;
      var scoreValues = new ScoreValue[]
        {
          new ('(', ')',     3, 1),
          new ('[', ']',    57, 2),
          new ('{', '}',  1197, 3),
          new ('<', '>', 25137, 4)
        };

      foreach (var line in navData)
      {
        lineIdx++;
        Debug.Assert(!string.IsNullOrWhiteSpace(line));
        Debug.Assert(line.Length >= 2);

        var openStack = new Stack<char>();
        int pos = -1;
        foreach (var c in line)
        {
          pos++;
          if (scoreValues.Any(v => v.OpeningParen == c))
          {
            openStack.Push(c);
          }
          else if (scoreValues.Any(v => v.ClosingParen == c))
          {
            var closeValue = scoreValues
              .Where(v => v.ClosingParen == c)
              .First();
            if (closeValue.OpeningParen == openStack.Peek())
            {
              // syntactically valid block to be otherwise ignored
              _ = openStack.Pop();
            }
            else
            {
              // corrupted block. increment part one scoring.
              syntaxScore += closeValue.ScorePtOne;
              openStack.Clear();
              break;
            }
          }
          else
          {
            throw new Exception($"Unknown character '{c}' in line {lineIdx} at character index {pos}.");
          }
        }

        if (openStack.Count > 0)
        {
          // incomplete line
          long lineScore = 0L;
          foreach (var op in openStack)
          {
            lineScore *= 5;
            lineScore += scoreValues.Where(v => v.OpeningParen == op).First().ScorePtTwo;
          }
          autoCarrotScore.Add(lineScore);
        }
      }

      autoCarrotScore.Sort();
      return (syntaxScore, autoCarrotScore[autoCarrotScore.Count / 2]);
    }
  }
}
