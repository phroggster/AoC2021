using System;

namespace AoC2021.Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hpos * VPos simple: {PositionTracker.CalculateSimple(Data.Day02Data)}");
            Console.WriteLine($"Hpos * VPos aimed:  {PositionTracker.CalculateWithAim(Data.Day02Data)}");
        }
    }
}
