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
    }
}
