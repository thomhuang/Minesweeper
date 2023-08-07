using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
namespace Minesweeper
{
    public static class Utility
    {
        public struct Cell
        {
            public bool visited;
            public int type;
            public string display;
            public int mineCount;

            public override string ToString()
            {
                return display;
            }
        }
        public const int Hidden = 0;
        public const int Shown = 1;
        public const int Bomb = 2;
        public const int Flag = 3;
        public const string BombChar = "*";
        public const string HiddenChar = "X";
        public const string FlagChar = "F";
    }
}
