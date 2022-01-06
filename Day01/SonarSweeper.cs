using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day01
{
  public static class SonarSweeper
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Single-measurement increases: {CalculateIncreases(Data.Day01Data)}");
      Console.WriteLine($"Three-measurement increases:  {CalculateSlidingWindowIncreases(Data.Day01Data)}");
    }

    public static int CalculateIncreases(int[] depths)
    {
      depths = depths ?? throw new ArgumentNullException(nameof(depths));

      int last = 0, count = 0;
      foreach (var d in depths)
      {
        if (last != 0 && d > last)
        {
          count++;
        }

        last = d;
      }
      return count;
    }

    public static int CalculateSlidingWindowIncreases(int[] depths)
    {
      depths = depths ?? throw new ArgumentNullException(nameof(depths));

      int last = 0, count = 0;
      for (int n = 0; n + 2 < depths.Length; n++)
      {
        var result = depths[n] + depths[n + 1] + depths[n + 2];
        if (last != 0 && result > last)
        {
          count++;
        }
        last = result;
      }
      return count;
    }
  }
}
