using System.Drawing;

namespace Minesweeper
{
    public static class Utility
    {
        public struct Cell
        {
            public bool visited { get; set; }
            public int type { get; set; }
            public string display { get; set; }
            public int mineCount { get; set; }

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
        public const string HiddenChar = "-";
        public const string FlagChar = "F";
    }
}
