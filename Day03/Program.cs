using System;

namespace AoC2021.Day03
{
  class Program
  {
    static readonly string[] s_ExampleData =
    {
      "00100",
      "11110",
      "10110",
      "10111",
      "10101",
      "01111",
      "00111",
      "11100",
      "10000",
      "11001",
      "00010",
      "01010",
    };

    static void Main(string[] args)
    {
      Console.WriteLine($"Power usage:  {ReportParser.CalculatePowerConsumption(Data.Day3Data)}");
      Console.WriteLine($"Life support: {ReportParser.CalculateLifeSupportRating(s_ExampleData /*Data.Day3Data*/)}");
    }
  }
}
