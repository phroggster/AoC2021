using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day03
{
  public static class ReportParser
  {
    public static int CalculatePowerConsumption(string[] dataset)
    {
      Debug.Assert(dataset != null);
      Debug.Assert(dataset.Any());

      int nGamma = 0;
      int nBits = dataset.ElementAt(0).Length;
      int nThreshold = dataset.Length / 2;
      // init a list of ints to zero, each storing the number of '1' values in a column
      List<int> mcbs = new(Enumerable.Repeat(0, nBits));

      // iterate all rows and columns of the dataset, incrementing the mcbs for any '1' values in the dataset.
      for (int row = 0; row < dataset.Length; row++)
      {
        Debug.Assert(dataset[row].Length == nBits);

        for (int col = 0; col < nBits; col++)
        {
          if (dataset[row][col] == '1')
          {
            mcbs[col]++;
          }
          else
          {
            Debug.Assert(dataset[row][col] == '0');
          }
        }
      }

      for (int n = 1; n <= nBits; n++)
      {
        if (mcbs[n-1] >= nThreshold)
        {
          nGamma |= (1 << (nBits - n));
        }
      }

      // epsilon is the bitwise compliment of gamma, but gamma has a significant number of leading zeros
      // that we don't care about, and the compliment of those must be ignored.
      int nEpsilon = ~nGamma & ((1 << nBits) - 1);
      return (int)(nGamma * nEpsilon);
    }

    public static int CalculateLifeSupportRating(string[] dataset)
    {
      Debug.Assert(dataset != null);
      Debug.Assert(dataset.Any());

      int numBits = dataset[0].Length;

      var filteredSet = dataset;
      for (int n = 0; n < numBits && filteredSet.Length > 1; n++)
      {
        int nRecords = filteredSet.Length;
        int nBitsActive = 0;
        int nThreshold = nRecords % 2 == 0 ? nRecords / 2 : (nRecords + 1) / 2;

        foreach (var row in filteredSet)
        {
          if (row[n] == '1')
            nBitsActive++;
        }

        if (nBitsActive >= nThreshold)
        {
          filteredSet = filteredSet.Where(r => r[n] == '1').ToArray();
        }
        else
        {
          filteredSet = filteredSet.Where(r => r[n] == '0').ToArray();
        }
      }
      Debug.Assert(filteredSet.Length == 1);
      int o2Rating = Convert.ToInt32(filteredSet[0], 2);


      filteredSet = dataset;
      for (int n = 0; n < numBits && filteredSet.Length > 1; n++)
      {
        int nRecords = filteredSet.Length;
        int nBitsActive = 0;
        int nThreshold = nRecords % 2 == 0 ? nRecords / 2 : (nRecords + 1) / 2;

        foreach (var row in filteredSet)
        {
          if (row[n] == '1')
            nBitsActive++;
        }

        if (nBitsActive >= nThreshold)
        {
          filteredSet = filteredSet.Where(r => r[n] == '0').ToArray();
        }
        else
        {
          filteredSet = filteredSet.Where(r => r[n] == '1').ToArray();
        }
      }
      Debug.Assert(filteredSet.Length == 1);
      int co2Rating = Convert.ToInt32(filteredSet[0], 2);

      return o2Rating * co2Rating;
    }
  }
}
