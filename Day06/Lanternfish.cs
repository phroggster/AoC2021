using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day06
{
  public static class Lanternfish
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Lanternfish ( 80 days):  {SimulateDays(Data.Day06Data, 80)}");
      Console.WriteLine($"Lanternfish (256 days):  {SimulateDays(Data.Day06Data, 256)}");
    }

    /// <summary>Simulate <paramref name="nDays"/> in the Lanternfish ecosystem.</summary>
    /// <param name="dataSet">The ages of the nearby lanterfish population.</param>
    /// <param name="nDays">The number of days to simulate.</param>
    /// <returns>The lanternfish population count after <paramref name="nDays"/> have elapsed.</returns>
    /// <exception cref="OverflowException"/>
    public static long SimulateDays(IReadOnlyCollection<byte> dataSet, int nDays)
    {
      // My first (naive) approach modified the dataSet ref to track any new fish. This quickly resulted in
      // massive thrashing, decrementing 1,574,445,493,136 memory addresses just to tick off another day.
      // This method is quite a bit faster. Could probably optimize it a bit more, but meh.

      Debug.Assert(!dataSet.Any(f => f > 8 || f < 0));
      // No data seems to have been provided such that (n < 1 || n >= 6), but let's include it anyways.
      long nBirths = 0,
        n0 = dataSet.Count(f => f == 0),
        n1 = dataSet.Count(f => f == 1),
        n2 = dataSet.Count(f => f == 2),
        n3 = dataSet.Count(f => f == 3),
        n4 = dataSet.Count(f => f == 4),
        n5 = dataSet.Count(f => f == 5),
        n6 = dataSet.Count(f => f == 6),
        n7 = dataSet.Count(f => f == 7),
        n8 = dataSet.Count(f => f == 8);

      for (int day = 0; day < nDays; day++)
      {
        nBirths = n0;

        n0 = n1;
        n1 = n2;
        n2 = n3;
        n3 = n4;
        n4 = n5;
        n5 = n6;
        n6 = checked(n7 + nBirths);
        n7 = n8;
        n8 = nBirths;
      }

      return checked(n0 + n1 + n2 + n3 + n4 + n5 + n6 + n7 + n8);
    }
  }
}
