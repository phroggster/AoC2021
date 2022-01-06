using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AoC2021.Day05
{
  [DebuggerDisplay("{DbgDisplay,nq}")]
  public readonly struct Point
  {
    public readonly ushort x;
    public readonly ushort y;

    public Point(ushort xpos, ushort ypos)
    {
      x = xpos;
      y = ypos;
    }


    /// <exception cref="OverflowException"/>
    public static Point operator +(Point lhs, Point rhs)
    {
      int dx = lhs.x + rhs.x;
      int dy = lhs.y + rhs.y;
      if (dx > ushort.MaxValue || dx < 0 || dy > ushort.MaxValue || dy < 0)
        throw new OverflowException();

      return new Point((ushort)dx, (ushort)dy);
    }

    /// <exception cref="OverflowException"/>
    public static Point operator -(Point lhs, Point rhs)
    {
      int dx = lhs.x - rhs.x;
      int dy = lhs.y - rhs.y;
      if (dx > ushort.MaxValue || dx < 0 || dy > ushort.MaxValue || dy < 0)
        throw new OverflowException();

      return new Point((ushort)dx, (ushort)dy);
    }


    internal string DbgDisplay => $"{x},{y}";

    public override string ToString()
    {
      return $"{x},{y}";
    }
  }

  [DebuggerDisplay("{DbgDisplay,nq}")]
  public readonly struct Line
  {
    public readonly Point a;
    public readonly Point b;

    public Line(ushort x1, ushort y1, ushort x2, ushort y2)
    {
      a = new Point(x1, y1);
      b = new Point(x2, y2);
    }

    public Line(Point pA, Point pB)
    {
      a = pA;
      b = pB;
    }


    public ushort X1 => a.x;
    public ushort Y1 => a.y;
    public ushort X2 => b.x;
    public ushort Y2 => b.y;

    public bool IsHorizontal => a.y == b.y;
    public bool IsVertical => a.x == b.x;

    internal string DbgDisplay => $"{a.DbgDisplay} -> {b.DbgDisplay}";

    public ushort MaxX => Math.Max(a.x, b.x);
    public ushort MaxY => Math.Max(a.y, b.y);
    public ushort MinX => Math.Min(a.x, b.x);
    public ushort MinY => Math.Min(a.y, b.y);
  }


  public static class HydrothermalVenture
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Oriented overlaps: {CountOrientedOverlaps(Data.Day05Data)}");
      Console.WriteLine($"All overlaps:      {CountAllOverlaps(Data.Day05Data)}");
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
