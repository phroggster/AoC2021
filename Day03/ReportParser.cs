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

      int nEpsilon = ~nGamma & ((1 << nBits) - 1);
      return (int)(nGamma * nEpsilon);
    }

    public static int CalculateLifeSupportRating(string[] dataset)
    {
      Debug.Assert(dataset != null);
      Debug.Assert(dataset.Any());

      int n = 0;
      var filteredSet = dataset;
      while (!FilterO2(ref filteredSet, n++)) { }
      Debug.Assert(filteredSet.Length == 1);
      int o2Rating = Convert.ToInt32(filteredSet[0], 2);

      n = 0;
      filteredSet = dataset;
      while (!FilterC02(ref filteredSet, n++)) { }
      Debug.Assert(filteredSet.Length == 1);
      int co2Rating = Convert.ToInt32(filteredSet[0], 2);

      return o2Rating * co2Rating;
    }

    private static bool FilterO2(ref string[] dataset, int index)
    {
      Debug.Assert(dataset != null);
      Debug.Assert(dataset.Length > 1);
      Debug.Assert(dataset[0].Length > index);

      int nCount = 0;
      int nRows = dataset.Length;
      int nThreshold = nRows / 2;

      // summate the characters in the index column
      foreach (var r in dataset)
      {
        if (r[index] == '1') nCount++;
      }

      // return only values from dataset that have the **MOST** common value in that column
      if (nCount >= nThreshold)
      {
        // MCV is '1'
        dataset = dataset.Where(r => r[index] == '1').ToArray();
      }
      else
      {
        // MCV is '0'
        dataset = dataset.Where(r => r[index] == '0').ToArray();
      }

      return dataset.Length == 1;
    }

    private static bool FilterC02(ref string[] dataset, int index)
    {
      Debug.Assert(dataset != null);
      Debug.Assert(dataset.Length > 1);
      Debug.Assert(dataset[0].Length > index);

      int nCount = 0;
      int nRows = dataset.Length;
      int nThreshold = nRows / 2;

      // summate the characters in the index column
      foreach (var r in dataset)
      {
        if (r[index] == '1') nCount++;
      }

      // return only values from dataset that have the **LEAST** common value in that column
      if (nCount >= nThreshold)
      {
        // LCV is '0'
        dataset = dataset.Where(r => r[index] == '0').ToArray();
      }
      else
      {
        // LCV is '1'
        dataset = dataset.Where(r => r[index] == '1').ToArray();
      }

      return dataset.Length == 1;
    }
  }
}
