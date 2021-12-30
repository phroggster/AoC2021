using System;

namespace AoC2021.Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Single-measurement increases: {DepthTest.CalculateIncreases(Data.Day01Data)}");
            Console.WriteLine($"Three-measurement increases:  {DepthTest.CalculateSlidingWindowIncreases(Data.Day01Data)}");
        }
    }
}
