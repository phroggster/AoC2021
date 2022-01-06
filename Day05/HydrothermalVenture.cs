using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AoC2021.Day05
{
  public static class HydrothermalVenture
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Oriented overlaps: {CountOrientedOverlaps(Data.ActualDataDay05)}");
      Console.WriteLine($"All overlaps:      {CountAllOverlaps(Data.ActualDataDay05)}");
    }

    /// <summary>Using only horizontal and vertical lines from <paramref name="dataSet"/>, count the
    /// number of tiles where two or more lines overlap.</summary>
    public static int CountOrientedOverlaps(IEnumerable<Line> dataSet)
    {
      int height = dataSet.Max(ln => ln.MaxY) + 1;
      int width = dataSet.Max(ln => ln.MaxX) + 1;
      var plot = new int[width, height];

      PlotLines(ref plot, dataSet.Where(ln => ln.IsHorizontal || ln.IsVertical));
      return SummateOverlaps(ref plot, width, height);
    }

    /// <summary>Count the number of tiles where two or more lines from <paramref name="dataSet"/>
    /// overlap.</summary>
    public static int CountAllOverlaps(IEnumerable<Line> dataSet)
    {
      int height = dataSet.Max(ln => ln.MaxY) + 1;
      int width = dataSet.Max(ln => ln.MaxX) + 1;
      var plot = new int[width, height];

      PlotLines(ref plot, dataSet);
      return SummateOverlaps(ref plot, width, height);
    }


    

    private static void DumpToConsole(ref int[,] plot, int width, int height)
    {
      int value = 0;
      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          value = plot[x, y];
          Console.Write(value == 0 ? "." : value);
        }
        Console.WriteLine();
      }

      Console.WriteLine();
    }

    private static void PlotLines(ref int[,] plot, IEnumerable<Line> lines)
    {
      foreach (var line in lines)
      {
        PlotLine(ref plot, line);
      }
    }

    private static void PlotLine(ref int[,] plot, Line line)
    {
      if (line.IsHorizontal)
      {
        for (int x = line.MinX; x <= line.MaxX; x++)
        {
          plot[x, line.a.y]++;
        }
      }
      else if (line.IsVertical)
      {
        for (int y = line.MinY; y <= line.MaxY; y++)
        {
          plot[line.a.x, y]++;
        }
      }
      else
      {
        var aX = line.MinX;
        var aY = aX == line.a.x ? line.a.y : line.b.y;
        var bX = line.MaxX;
        var bY = bX == line.a.x ? line.a.y : line.b.y;
        bool up = bY < aY;

        // x always increases from A to B per prior; y can go either increase or decrease. top-left origin.
        for (int x = aX, y = aY; x <= bX; x++)
        {
          plot[x, up ? y-- : y++]++;
        }
      }
    }

    private static int SummateOverlaps(ref int[,] plot, int width, int height)
    {
      int result = 0;
      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          if (plot[x, y] >= 2)
            result++;
        }
      }

      return result;
    }
  }
}
