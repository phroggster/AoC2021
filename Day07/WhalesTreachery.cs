using AoC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day07
{
  public static class WhalesTreachery
  {
    static void Main(string[] _)
    {
      Console.WriteLine($"Median of example data:        {CalcMedianFuelCosts(Data.Day07ExampleData)}");
      Console.WriteLine($"Median of actual data:         {CalcMedianFuelCosts(Data.Day07Data)}");
      Console.WriteLine( "------------------------------|--------------");
      Console.WriteLine($"Triangulation of example data: {CalcTriangularFuelCosts(Data.Day07ExampleData)}");
      Console.WriteLine($"Triangulation of actual data:  {CalcTriangularFuelCosts(Data.Day07Data)}");
    }


    public static int CalcMedianFuelCosts(IEnumerable<int> crabPositions)
    {
      // step 1: 1 unit  (1 unit)
      // step 2: 2 units (2 units)
      // step 3: 3 units (3 units)
      // total:           3 units

      var median = crabPositions.RoundedMedian();
      int summation = 0;

      foreach (var hpos in crabPositions)
      {
        summation += Math.Abs(hpos - median);
      }

      return summation;
    }

    public static int CalcTriangularFuelCosts(IEnumerable<int> crabPositions)
    {
      // step 1: 1 unit  (1 unit)
      // step 2: 2 units (3 units)
      // step 3: 3 units (6 units)
      // total:           6 units

      var sortedData = crabPositions.OrderBy(n => n);
      var endingValue = sortedData.Last();
      var oldCost = int.MaxValue;
      var newCost = int.MaxValue;

      for (int n = sortedData.First(); n <= endingValue && newCost <= oldCost; n++)
      {
        oldCost = newCost;
        newCost = 0;
        foreach (var val in sortedData)
        {
          newCost += Extensions.GetTriangularNumber(val - n);
        }
      }

      return oldCost;
    }
  }
}
