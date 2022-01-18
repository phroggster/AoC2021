#if false
# define USE_CONSOLE_DUMP
#endif

using AoC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day11
{
  public static class DumboOctopus
  {
    static void Main(string[] _)
    {
      // How many total flashes are there after 100 steps?
      Console.WriteLine($"100-Step Flashes EXAMPLE:  {CountFlashesDuring100Steps(Data.Day11ExampleData)}");
      Console.WriteLine($"100-Step Flashes ACTUAL:   {CountFlashesDuring100Steps(Data.Day11ActualData)}");
      Console.WriteLine("--------------------------|----");

      // What is the first step during which all octopuses flash?
      Console.WriteLine($"Next full-flash EXAMPLE:   {FindFirstStepAllItemsFlash(Data.Day11ExampleData)}");
      Console.WriteLine($"Next full-flash ACTUAL:    {FindFirstStepAllItemsFlash(Data.Day11ActualData)}");
    }

    public static int CountFlashesDuring100Steps(byte[,] data)
    {
      return SimulateOctopi(data).nFlashesFirst100;
    }

    public static int FindFirstStepAllItemsFlash(byte[,] data)
    {
      return SimulateOctopi(data).nStepsToFullFlash;
    }


    private static (int nFlashesFirst100, int nStepsToFullFlash) SimulateOctopi(byte[,] data)
    {
      _ = data ?? throw new ArgumentNullException(nameof(data));
      var buf = data.Clone() as byte[,];
      if (buf is null || buf.Length != data.Length
        || buf.GetLength(0) != data.GetLength(0)
        || buf.GetLength(1) != data.GetLength(0))
      {
        throw new InvalidOperationException("Failed to clone dataset.");
      }

      int nFlashes = 0;
      int stepNum = 0;
      var width = buf.GetLength(0);
      var height = buf.GetLength(1);

      for (; buf.Any(b => b != 0); stepNum++)
      {
        if (stepNum < 10 || (stepNum <= 100 && stepNum % 10 == 0))
        {
          DumpToConsole(buf, stepNum);
        }

        // *** First, the energy level of each octopus increases by 1.
        for (int y = 0; y < height; y++)
        {
          for (int x = 0; x < width; x++)
          {
            buf[y, x]++;
          }
        }

        // *** Then, any octopus with an energy level greater than 9 flashes
        while (buf.Any(b => b > 9))
        {
          var flashers = new List<(int r, int c)>();
          for (int y = 0; y < height; y++)
          {
            for (int x = 0; x < width; x++)
            {
              if (buf[y, x] > 9)
              {
                flashers.Add((y, x));

                // *** Finally, any octopus that flashed during this step has its energy level set to 0
                buf[y, x] = 0;
              }
            }
          }

          // *** This increases the energy level of all adjacent [octopi] by 1, including [octopi] that are diagonally adjacent.
          foreach (var f in flashers)
          {
            // the coordinates of neighboring octopi that haven't flashed on this iteration.
            var neighbors = new (int r, int c)[]
              {
                (f.r - 1, f.c - 1),  (f.r - 1, f.c),  (f.r - 1, f.c + 1),
                (f.r,     f.c - 1),/*(f.r,     f.c),*/(f.r,     f.c + 1),
                (f.r + 1, f.c - 1),  (f.r + 1, f.c),  (f.r + 1, f.c + 1)
              }
              .Where(n => n.r >= 0 && n.r < height
                && n.c >= 0 && n.c < width
                && buf[n.r, n.c] != 0);
            foreach (var (r, c) in neighbors)
            {
              buf[r, c]++;
            }
          }
        }

        if (stepNum < 100)
          nFlashes += buf.Count(b => b == 0);
      }

      return (nFlashes, stepNum);
    }

    [Conditional("USE_CONSOLE_DUMP")]
    private static void DumpToConsole(byte[,] arr, int stepNum)
    {
      var sb = new StringBuilder();
      sb.AppendLine(stepNum > 0 ? $"After step {stepNum}:" : "Before any steps:");

      for (int r = 0; r < arr.GetLength(0); r++)
      {
        for (int c = 0; c < arr.GetLength(1); c++)
        {
          sb.Append(arr[r, c]);
        }
        sb.Append(Environment.NewLine);
      }

      Console.WriteLine(sb.ToString());
    }
  }
}
