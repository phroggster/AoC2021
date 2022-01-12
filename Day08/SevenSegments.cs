using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day08
{
  /* ┍AAA┑
   * B   C
   * ┝DDD┥
   * E   F
   * ┕GGG┙ */

  public static class SevenSegments
  {
    static void Main(string[] _)
    {
      // In the output values, how many times do digits 1, 4, 7, or 8 appear?
      Console.WriteLine($"Unique digits (example data): {SummateUniqueDigits(Data.Day08ExampleData)}");
      Console.WriteLine($"Unique digits (actual data):  {SummateUniqueDigits(Data.Day08ActualData)}");
      Console.WriteLine("-----------------------------|----");
      // What do you get if you (decode the output LEDs, then) add up all of the output values?
      Console.WriteLine($"Summation (example data):     {DecodeAndSummateOutputs(Data.Day08ExampleData)}");
      Console.WriteLine($"Summation (actual data):      {DecodeAndSummateOutputs(Data.Day08ActualData)}");
    }

    /// <summary>Count the number of unique 7-segment digits being displayed. That is, summate
    /// the number of 1's (2 segments), 4's (4 segments), 7's (3 segments), and 8's (7 segments).</summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int SummateUniqueDigits(string input)
    {
      int count = 0;
      foreach (var inputLine in input.Split("\r\n"))
      {
        Debug.Assert(inputLine.Count(c => c == '|') == 1);

        var lengths = inputLine
          .Split('|')[1]
          .ToLower()
          .Trim()
          .Split(" ")
          .Select(s => s.Trim().Length);
        Debug.Assert(lengths.Count() == 4);
        var acc = lengths.Count(c => c == 2 || c == 3 || c == 4 || c == 7);
        count += acc;
      }

      return count;
    }

    /// <summary>Decode the desired led displays, and summate their results.</summary>
    /// <param name="input">A single string containing all of the input text.</param>
    /// <returns>An <see cref="int"/> representing the summation of the displayed values.</returns>
    public static int DecodeAndSummateOutputs(string input)
    {
      int total = 0;

      foreach (var line in input.Split("\r\n"))
      {
        // Debug.Assert(line.ToCharArray().Select(c => c == '|').Count() == 1);

        var split = line.Split('|');
        var inputs = split.First().Trim().Split(' ');
        var outputs = split.Last().Trim().Split(' ');
        var map = new Dictionary<string, string>(10);

        // sort the sequences.
        for (int i = 0; i < 10; i++)
        {
          inputs[i] = string.Concat(inputs[i].OrderBy(c => c));
          if (i < 4)
          {
            outputs[i] = string.Concat(outputs[i].OrderBy(c => c));
          }
        }

        Array.Sort(inputs, (x, y) => x.Length.CompareTo(y.Length));
        map["1"] = inputs[0];
        map["7"] = inputs[1];
        map["4"] = inputs[2];
        map["8"] = inputs[9];

        // 2,3,5 are all five segments long.
        var fives = new List<string> { inputs[3], inputs[4], inputs[5] };

        // 3 is easiest, just one bit off from a 1
        foreach (var five in fives)
        {
          if (five.Contains(map["1"][0]) && five.Contains(map["1"][1]))
          {
            map["3"] = five;
            break;
          }
        }
        fives.Remove(map["3"]);

        var centerAndUpperLeft = map["4"].Except(map["1"]).ToList();

        // Grab the 5. It's got both of the center and upper-left segments active.
        foreach (var five in fives)
        {
          if (five.Contains(centerAndUpperLeft[0]) && five.Contains(centerAndUpperLeft[1]))
          {
            map["5"] = five;
            break;
          }
        }
        fives.Remove(map["5"]);

        // The 2 is all that's left out of the fives.
        Debug.Assert(fives.Count() == 1);
        map["2"] = fives.First();

        // 6,9,0 are all six segments long.
        var sixes = new List<string> { inputs[6], inputs[7], inputs[8] };

        // Grab the 6. It's got little in common with the 1.
        foreach (var six in sixes)
        {
          if (six.Except(map["1"]).Count() == 5)
          {
            map["6"] = six;
            break;
          }
        }
        sixes.Remove(map["6"]);

        // Grab the 9. it's pretty close to a 4.
        foreach (var six in sixes)
        {
          if (six.Except(map["4"]).Count() == 2)
          {
            map["9"] = six;
            break;
          }
        }
        sixes.Remove(map["9"]);

        // The 0 is all that's left of the sixes.
        Debug.Assert(sixes.Count() == 1);
        map["0"] = sixes.First();


        // Tabulation
        var result = new StringBuilder(4);
        for (var n = 0; n < 4; n++)
        {
          Debug.Assert(map.ContainsValue(outputs[n]));

          if (map.ContainsValue(outputs[n]))
          {
            result.Append(map.First(x => x.Value == outputs[n]).Key);
          }
          else
          {
            throw new Exception($"No match found for {outputs[n]}.");
          }
        }

        total += Convert.ToInt32(result.ToString());
      }

      return total;
    }
  }
}
