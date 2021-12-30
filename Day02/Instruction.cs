using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2021.Day02
{
    public enum Direction
    {
        [Browsable(false)]
        invalid = 0,
        forward,
        up,
        down
    };

    public class Instruction
    {
        public Direction Direction { get; init; }

        public int Amount { get; init; }


        public Instruction(Direction dir, int amount)
        {
            Amount = amount;
            Direction = dir;
        }

        public Instruction(string dir, int amount)
        {
            Amount = amount;
            if (Enum.TryParse(dir, out Direction d) && d != Direction.invalid)
            {
                Direction = d;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(dir));
            }
        }
    }
}
