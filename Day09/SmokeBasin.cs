using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day09
{
  public static class SmokeBasin
  {
    static void Main(string[] _)
    {
      // What is the sum of the risk levels of all low points on your heightmap?
      Console.WriteLine($"Risk Summation Example Data:  {SummateLowPointsRisk(Data.Day09Exampledata)}");
      Console.WriteLine($"Risk Summation Actual Data:   {SummateLowPointsRisk(Data.Day09ActualData)}");
      Console.WriteLine("-----------------------------|----");

      // What do you get if you multiply together the sizes of the three largest basins?
      Console.WriteLine($"Basin Product Example Data:   {ThreeLargestBasinsSizeProduct(Data.Day09Exampledata)}");
      Console.WriteLine($"Basin Product Actual Data:    {ThreeLargestBasinsSizeProduct(Data.Day09ActualData)}");
    }

    public static int SummateLowPointsRisk(byte[,] heightMap)
    {
      return FindLowPointCoords(heightMap)
        .Select(c => heightMap[c.x, c.y] + 1)
        .Sum();
    }

    public static int ThreeLargestBasinsSizeProduct(byte[,] heightMap)
    {
      int result = 1;

      foreach (var sz in FindBasinSizes(heightMap)
        .OrderByDescending(sz => sz)
        .Take(3))
      {
        result = result * sz;
      }
      return result;
    }


    // Return an iterator of (X, Y) coordinates representing where the low points are on the height map.
    private static IEnumerable<(int x, int y)> FindLowPointCoords(byte[,] heightMap)
    {
      Debug.Assert(heightMap.Rank == 2);
      Debug.Assert(heightMap.GetLength(0) >= 3);
      Debug.Assert(heightMap.GetLength(1) >= 3);

      var width = heightMap.GetLength(0);
      var height = heightMap.GetLength(1);

      var neighbors = new List<byte>(4);
      int target = -1;
      for (int x = 0; x < width; x++)
      {
        for (int y = 0; y < height; y++, neighbors.Clear())
        {
          target = heightMap[x, y];

          if (x > 0)
            neighbors.Add(heightMap[x - 1, y]);
          if (x + 1 < width)
            neighbors.Add(heightMap[x + 1, y]);

          if (y > 0)
            neighbors.Add(heightMap[x, y - 1]);
          if (y + 1 < height)
            neighbors.Add(heightMap[x, y + 1]);

          if (neighbors.Count == neighbors.Count(n => n > target))
            yield return (x, y);
        }
      }
    }


    // Search for basins and return their sizes.
    private static IEnumerable<int> FindBasinSizes(byte[,] heightMap)
    {
      Debug.Assert(heightMap.Rank == 2);
      Debug.Assert(heightMap.GetLength(0) >= 3);
      Debug.Assert(heightMap.GetLength(1) >= 3);


      foreach (var btmCoords in FindLowPointCoords(heightMap))
      {
        if (heightMap[btmCoords.x, btmCoords.y] == 9)
          continue;

        var visited = new List<(int x, int y)>();
        yield return FindBasinSizeRecur(heightMap, btmCoords, ref visited);
      }
    }

    // Starting from a low point, iterate neighbors recursively to map out the basin.
    private static int FindBasinSizeRecur(
      byte[,] heightMap,
      (int x, int y) lowPoint,
      ref List<(int x, int y)> visitedTiles)
    {
      var lowHeight = heightMap[lowPoint.x, lowPoint.y];

      if (lowHeight < 9 && !visitedTiles.Contains(lowPoint))
      {
        // Mark lowPoint as visited
        visitedTiles.Add(lowPoint);

        // Flag the four neighbors for followup if they're viable.
        var neighbors = new List<(int x, int y)>(4);
        var width = heightMap.GetLength(0);
        var height = heightMap.GetLength(1);

        if (lowPoint.x - 1 >= 0)
        {
          (int x, int y) left = (lowPoint.x - 1, lowPoint.y);
          if (heightMap[left.x, left.y] >= lowHeight && !visitedTiles.Contains(left))
          {
            neighbors.Add(left);
          }
        }
        if (lowPoint.x + 1 < width)
        {
          (int x, int y) right = (lowPoint.x + 1, lowPoint.y);
          if (heightMap[right.x, right.y] >= lowHeight && !visitedTiles.Contains(right))
          {
            neighbors.Add(right);
          }
        }
        if (lowPoint.y - 1 >= 0)
        {
          (int x, int y) above = (lowPoint.x, lowPoint.y - 1);
          if (heightMap[above.x, above.y] >= lowHeight && !visitedTiles.Contains(above))
          {
            neighbors.Add(above);
          }
        }
        if (lowPoint.y + 1 < height)
        {
          (int x, int y) below = (lowPoint.x, lowPoint.y + 1);
          if (heightMap[below.x, below.y] >= lowHeight && !visitedTiles.Contains(below))
          {
            neighbors.Add(below);
          }
        }

        // Recurse upon the neighbors. They'll mark themselves as visited.
        foreach (var tile in neighbors)
        {
          _ = FindBasinSizeRecur(heightMap, tile, ref visitedTiles);
        }
      }

      return visitedTiles.Count;
    }
  }
}
