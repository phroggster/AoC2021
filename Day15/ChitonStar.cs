using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day15
{
  public static class ChitonStar
  {
    static void Main(string[] _)
    {
      // What is the lowest total risk of any path from the top left to the bottom right?
      Console.WriteLine($"Least risk EXAMPLE:     {FindLeastRisk(Data.Day15ExampleData)}");
      Console.WriteLine($"Least risk ACTUAL:      {FindLeastRisk(Data.Day15ActualData)}");
      Console.WriteLine("-----------------------|----");

      // Using the **full** map, what is the lowest total risk ...?
      Console.WriteLine($"Least risk x25 EXAMPLE: {FindLeastRiskX5(Data.Day15ExampleData)}");
      Console.WriteLine($"Least risk x25 ACTUAL:  {FindLeastRiskX5(Data.Day15ActualData)}");
    }

    public static int FindLeastRisk(IEnumerable<IEnumerable<byte>> inputData)
    {
      return new AStarPathing(inputData, 1)
        .FindRoute();
    }

    public static int FindLeastRiskX5(IEnumerable<IEnumerable<byte>> inputData)
    {
      return new AStarPathing(inputData, 5)
        .FindRoute();
    }
  }


  public class AStarPathing
  {
    /// <exception cref="ArgumentOutOfRangeException"/>
    public AStarPathing(IEnumerable<IEnumerable<byte>> inputData, int scale = 1)
    {
      _width = inputData.First().Count();
      _height = inputData.Count();

      _scale = (scale == 1 || scale == 5)
        ? scale
        : throw new ArgumentOutOfRangeException(nameof(scale));
      _scaledWidth = _width * _scale;
      _scaledHeight = _height * _scale;

      _riskMap = new byte[_width * _height];
      int idx = 0;
      foreach (var row in inputData)
        foreach (var num in row)
          _riskMap[idx++] = num;
    }

    readonly int _width;
    readonly int _height;

    readonly int _scale;
    readonly int _scaledWidth;
    readonly int _scaledHeight;

    readonly byte[] _riskMap;

    public byte ElementAt(int x, int y)
    {
      // Limit bounds checks to debug builds without touching the preprocessor.
      Debug.Assert(x >= 0 && x < _scaledWidth);
      Debug.Assert(y >= 0 && y < _scaledHeight);

      int result = _riskMap[((y % _height) * _width) + (x % _width)]
        + (x / _width) + (y / _height);
      if (result > 9)
        result -= 9;
      return (byte)result;
    }

    public int FindRoute()
    {
      // goal always has the largest X and Y, so we can skip a few Math.Abs invocations here.
      static int Distance(Location loc, Location goal)
        => goal.X + goal.Y - loc.X - loc.Y;

      var openList = new List<Location>(_scaledWidth * _scaledHeight / 2);
      var closedList = new List<Location>(_scaledWidth * _scaledHeight / 2);
      var goal = new Location(_scaledWidth - 1, _scaledHeight - 1);
      var origin = new Location(0, 0, 0, _width + _height, null);

      openList.Add(origin);
      Location current = origin;

      while (openList.Any())
      {
        // 1) Get the best-scoring tile from the open list.
        var bestOpenScore = openList.Min(ol => ol.Score);
        current = openList
          // the end of the open list is most likely the best pathing
          .FindLast(ol => ol.Score <= bestOpenScore)
          ?? throw new InvalidOperationException();

        // Since we're always examining the best-scoring tile first, this *must* be the best path.
        if (goal.Equals(current))
          break;

        // 2) Remove the tile from the open list, and add it to the closed list.
        openList.Remove(current);
        closedList.Add(current);

        // 3) For each square in the adjacent tiles:
        var neighbors = new[]
        {
          // favor moving down, then right, before backtracking up or left.
          new Location(current.X, current.Y + 1),
          new Location(current.X + 1, current.Y),
          new Location(current.X, current.Y - 1),
          new Location(current.X - 1, current.Y)
        }
        // 3A) If neighbor is in the closed list: ignore it.
        .Where(l => l.X >= 0 && l.X < _scaledWidth && l.Y >= 0 && l.Y < _scaledHeight);

        foreach (var neighbor in neighbors)
        {
          if (closedList.Any(cl => neighbor.Equals(cl)))
            continue;

          int cost = current.Cost + ElementAt(neighbor.X, neighbor.Y);
          var priorVisit = openList
            .FirstOrDefault(ol => neighbor.Equals(ol));
          if (priorVisit is null)
          {
            // 3B) If neighbor is NOT in the open list, add it and compute it's score.
            neighbor.Cost = cost;
            neighbor.Heuristic = Distance(neighbor, goal);
            neighbor.Parent = current;
            openList.Add(neighbor);
          }
          else if (cost < priorVisit.Cost)
          {
            // 3C) If neighbor IS in the open list, check if this path is better, and update it if so.
            priorVisit.Cost = cost;
            priorVisit.Parent = current;
          }
        }
      }

      return current.Cost;
    }
  }

  [DebuggerDisplay("{" + nameof(DebugDisplay) + ",nq}")]
  public class Location
    : IEquatable<Location>
  {
    public Location(int x, int y, int cost, int heuristic, Location? parent)
    {
      X = x;
      Y = y;
      Cost = cost;
      Heuristic = heuristic;
      Parent = parent;
    }
    public Location() : this(0, 0, 0, 0, null) { }
    public Location(int x, int y) : this(x, y, 0, 0, null) { }

    public int X { get; set; }
    public int Y { get; set; }
    /// <summary>F - the overall score of this location:
    /// <c><see cref="Cost"/> + <see cref="Heuristic"/></c></summary>
    public int Score => Cost + Heuristic;
    /// <summary>G - the cost of traversing from the origin to this location.</summary>
    public int Cost { get; set; }
    /// <summary>H - estimated cost to traverse from this location to the destination.</summary>
    public int Heuristic { get; set; }
    public Location? Parent { get; set; }

    string DebugDisplay => $"({X}, {Y}): {Cost} + {Heuristic} = {Score}";

    // Only equate position, not cost, heuristic, score, nor parent.
    public bool Equals(Location? other)
      => other is not null && X == other.X && Y == other.Y;

    public override bool Equals(object? obj)
      => Equals(obj as Location);

    public override int GetHashCode()
      => (X, Y).GetHashCode();
  }
}
