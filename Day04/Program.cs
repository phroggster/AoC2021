using System;

namespace AoC2021.Day04
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Best score:  {SquidGame.GetScoreOfWinningBoard(Data.ActualDataDay04)}");
      Console.WriteLine($"Worst score: {SquidGame.GetScoreOfWorstBoard(Data.ActualDataDay04)}");
    }
  }
}
