using System;

namespace AoC2021.Day03
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Power usage:  {ReportParser.CalculatePowerConsumption(Data.Day3Data)}");
      Console.WriteLine($"Life support: {ReportParser.CalculateLifeSupportRating(Data.Day3Data)}");
    }
  }
}
