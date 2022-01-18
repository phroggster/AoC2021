using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day11
{
  public static class Data
  {
    public static readonly byte[,] Day11ExampleData = new byte[,]
    {
      { 5,4,8,3,1,4,3,2,2,3 },
      { 2,7,4,5,8,5,4,7,1,1 },
      { 5,2,6,4,5,5,6,1,7,3 },
      { 6,1,4,1,3,3,6,1,4,6 },
      { 6,3,5,7,3,8,5,4,7,8 },
      { 4,1,6,7,5,2,4,6,4,5 },
      { 2,1,7,6,8,4,1,7,2,1 },
      { 6,8,8,2,8,8,1,1,3,4 },
      { 4,8,4,6,8,4,8,5,5,4 },
      { 5,2,8,3,7,5,1,5,2,6 }
    };

    public static readonly byte[,] Day11ActualData = new byte[,]
    {
      { 7,2,3,2,3,7,4,3,1,4 },
      { 8,5,3,1,1,1,3,7,8,6 },
      { 3,4,1,1,7,8,7,8,2,8 },
      { 5,4,8,2,2,4,1,3,4,4 },
      { 5,8,5,6,8,2,7,7,4,2 },
      { 7,6,1,4,5,3,2,7,6,4 },
      { 5,3,1,1,3,2,1,7,5,8 },
      { 1,2,5,5,1,1,6,1,8,7 },
      { 5,8,2,1,2,7,7,7,1,4 },
      { 2,6,2,3,8,3,4,7,8,8 }
    };
  }
}
