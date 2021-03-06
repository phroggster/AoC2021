using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day04
{
  public static class SquidGame
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Best score:  {SquidGame.GetScoreOfWinningBoard(Data.ActualDataDay04)}");
      Console.WriteLine($"Worst score: {SquidGame.GetScoreOfWorstBoard(Data.ActualDataDay04)}");
    }


    public static int GetScoreOfWinningBoard(GameData gameData)
    {
      var winNum = gameData.CalculateWinner(out Board winner);
      var sumUnmarked = winner.Tiles
        .Where(t => !t.Marked)
        .Sum(t => t.Value);
      return winNum * sumUnmarked;
    }

    public static int GetScoreOfWorstBoard(GameData gameData)
    {
      var loseNum = gameData.CalculateLoser(out Board loser);
      var sumUnmarked = loser.Tiles
        .Where(t => !t.Marked)
        .Sum(t => t.Value);
      return loseNum * sumUnmarked;
    }
  }
}
