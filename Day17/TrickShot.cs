using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day17
{
  public static class TrickShot
  {
    static void Main(string[] _)
    {
      const string target = "target area: x=14..50, y=-267..-225";

      // What is the highest y position it reaches on this trajectory?
      Console.WriteLine($"Highest Altitude:    {MaxAltitude(target)}");
      // How many distinct initial velocity values cause the probe to be within the target area after any step?
      Console.WriteLine($"Available Solutions: {CountSolutions(target)}");
    }

    public static int MaxAltitude(string tgtData)
    {
      return new FireController(tgtData)
        .Solutions()
        .Max(ts => ts.maxY);
    }

    public static int CountSolutions(string tgtData)
    {
      return new FireController(tgtData)
        .Solutions()
        .Count();
    }
  }

  public class FireController
  {
    public FireController(string targetString)
    {
      // "target area: x=20..30, y=-10..-5"
      const string preamble = "target area: x=";
      const string a2b = ", y=";
      const string x2y = "..";
      Debug.Assert(targetString.StartsWith(preamble));
      Debug.Assert(targetString.Split(x2y).Count() == 3);
      Debug.Assert(targetString.Split(a2b).Count() == 2);

      var nums = targetString
        .Substring(preamble.Length)     // "20..30, y=-10..-5"
        .Split(a2b)                     // new string[] { "20..30", "-10..-5" }
        .SelectMany(s => s.Split(x2y))  // new string[] { "20", "30", "-10", "-5" }
        .Select(s => int.Parse(s))      // new int[] { 20, 30, -10, -5 }
        .ToArray();
      Debug.Assert(nums.Length == 4);
      Debug.Assert(nums[0] < nums[1]);
      Debug.Assert(nums[2] < nums[3]);

      // ( 20, -5, 10, 5)
      Target = new Rectangle(nums[0], nums[3], nums[1] - nums[0], nums[3] - nums[2]);
    }

    public Rectangle Target { get; init; }

    public IEnumerable<(int xVel, int yVel, int maxY)> Solutions()
    {
      const int MaxStartingY = 512;

      for (int sX = 0; sX < Target.Right + 1; ++sX)
      {
        for (int sY = Target.Bottom - 1; sY < MaxStartingY; ++sY)
        {
          int maxY = 0,
            xpos = 0,
            ypos = 0,
            dX = sX,
            dY = sY;

          while (xpos <= Target.Right && Target.Bottom <= ypos)
          {
            xpos += dX;
            ypos += dY;
            maxY = Math.Max(ypos, maxY);

            if (dX > 0)
              dX--;
            else if (dX < 0)
              dX++;
            dY--;

            if (Target.Contains(xpos, ypos))
            {
              yield return (sX, sY, maxY);
              break;
            } 
          }
        }
      }
    }
  }

  [DebuggerDisplay("{" + nameof(DbgDisplay) + ",nq}")]
  public readonly struct Rectangle
  {
    public readonly int X;
    public readonly int Y;
    public readonly int Width;
    public readonly int Height;

    public int Left => X;
    public int Top => Y;
    public int Right => X + Width;
    public int Bottom => Y - Height;

    public Rectangle(int x, int y, int width, int height)
    {
      X = x;
      Y = y;
      Width = width;
      Height = height;
    }

    public bool Contains(int x, int y)
      => Left <= x && x <= Right && Bottom <= y && y <= Top;

    public override string ToString() => DbgDisplay;

    private string DbgDisplay
      => $"({Left}, {Top})—({Right}, {Bottom}); Width: {Width}, Height: {Height}";
  }
}
