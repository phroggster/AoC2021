using AoC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day13
{
  public static class TransparentOrigami
  {
    static void Main(string[] _)
    {
      // How many dots are visible after completing just
      // the first fold instruction on your transparent paper?
      Console.WriteLine($"Visible Dots EXAMPLE:    {PartOne(Data.Day13ExampleData)}");
      Console.WriteLine($"Visible Dots Actual:     {PartOne(Data.Day13ActualData)}");
      Console.WriteLine("-------------------------|----");

      // What code do you use to activate the infrared thermal imaging camera system?
      Console.WriteLine($"Activation Code EXAMPLE: {Environment.NewLine}{PartTwo(Data.Day13ExampleData)}");
      Console.WriteLine($"Activation Code Actual:  {Environment.NewLine}{PartTwo(Data.Day13ActualData)}");
    }

    public static int PartOne(string[] data)
    {
      return new Graph(data)
        .Fold(1)
        .Marks.Count();
    }

    public static string PartTwo(string[] data)
    {
      return new Graph(data)
        .Fold(int.MaxValue)
        .DebugDisplay;
    }
  }

  [DebuggerDisplay("{DebugDisplay,nq}")]
  public readonly struct Fold
  {
    public Fold(bool vertical, int index)
    {
      bVertical = vertical;
      nIndex = index;
    }

    public Fold(string source /* "fold along y=7" */)
    {
      const string prefix = "fold along ";
      Debug.Assert(source.StartsWith(prefix));

      var split = source.Split(' ').Last().Split('=');
      bVertical = split.First().First() == 'x';
      nIndex = int.Parse(split.Last());

      Debug.Assert(bVertical || split.First().First() == 'y');
    }

    private readonly bool bVertical;
    public readonly int nIndex;

    public bool IsHorizontal => !bVertical;
    public bool IsVertical => bVertical;

    public int X => bVertical ? nIndex : throw new InvalidOperationException();

    public int Y => !bVertical ? nIndex : throw new InvalidOperationException();

    internal string DebugDisplay => $"{(bVertical ? "Vertical" : "Horizontal")} fold at {(bVertical ? "x" : "y")} index {nIndex}";
  }

  [DebuggerDisplay("{DebugDisplay,nq}")]
  public readonly struct Point
  {
    public readonly int x;

    public readonly int y;

    public Point(int xparm, int yparm)
    {
      x = xparm;
      y = yparm;
    }

    public Point(string source /* "6,10" */)
    {
      var split = source.Split(',');
      x = int.Parse(split.First());
      y = int.Parse(split.Last());
    }

    internal string DebugDisplay => $"({x}, {y})";
  }

  [DebuggerDisplay("{DebugDisplay,nq}")]
  public class Graph
  {
    Graph(IEnumerable<Point> marks, IEnumerable<Fold> folds, int width, int height)
    {
      _marks = new(marks);
      _folds = new(folds);
      Width = width;
      Height = height;
    }

    public Graph(IEnumerable<string> data)
    {
      // most lines are dot locations: "6,10"
      var dotTxts = data.Where(d => d.Contains(','));
      _marks = new(dotTxts.Count());
      foreach (var dotTxt in dotTxts)
      {
        _marks.Add(new(dotTxt));
      }

      // then an empty line, then folding directions: "fold along y=7"
      var foldTxts = data.Where(d => d.Contains("fold along x=") || d.Contains("fold along y="));
      _folds = new(foldTxts.Count());
      foreach (var foldTxt in foldTxts)
      {
        _folds.Add(new(foldTxt));
      }

      Width = Math.Max(Folds.Where(f => f.IsVertical).Max(f => f.X), Marks.Max(m => m.x) + 1);
      Height = Math.Max(Folds.Where(f => f.IsHorizontal).Max(f => f.Y), Marks.Max(m => m.y) + 1);
    }


    public IEnumerable<Point> Marks => _marks;
    private readonly List<Point> _marks;

    public IEnumerable<Fold> Folds => _folds;
    private readonly List<Fold> _folds;

    public int Width { get; init; }

    public int Height { get; init; }



    public Graph Fold(int nFolds = 1)
    {
      if (!_folds.Any() || nFolds <= 0)
        return this;
      else
        nFolds = Math.Min(nFolds, _folds.Count());

      var fold = _folds.First();
      int foldWid = fold.IsVertical ? fold.X : Width;
      int foldHei = fold.IsHorizontal ? fold.Y : Height;

      var foldedMarks = new HashSet<Point>(_marks.Count);
      foreach (var mark in _marks)
      {
        var fMark = mark;
        if (fold.IsVertical && mark.x > foldWid)
          // mirror it left
          fMark = new(foldWid - (mark.x - foldWid), mark.y);
        else if (fold.IsHorizontal && mark.y > foldHei)
          // mirror it up
          fMark = new(mark.x, foldHei - (mark.y - foldHei));

        foldedMarks.Add(fMark);
      }

      var result = new Graph(foldedMarks, Folds.Skip(1), foldWid, foldHei);
      if (nFolds > 1)
        return result.Fold(nFolds - 1);
      return result;
    }

    public string DebugDisplay
    {
      get
      {
        {
          var nRows = Height;
          var nCols = Width;
          var buf = new char[nRows, nCols];
          for (int row = 0; row < nRows; row++)
          {
            for (int col = 0; col < nCols; col++)
            {
              buf[row, col] = '.';
            }
          }

          foreach (var mark in Marks)
          {
            buf[mark.y, mark.x] = '#';
          }

          var sb = new StringBuilder((nCols + Environment.NewLine.Length) * nRows);
          for (int row = 0; row < nRows; row++)
          {
            sb.AppendLine(buf.RowToString(row));
          }

          return sb.ToString();
        }
      }
    }
  }
}
