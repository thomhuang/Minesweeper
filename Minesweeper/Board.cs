using static Minesweeper.Utility;
namespace Minesweeper
{
    public class Board
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly int _bombs;
        public Cell[,] board;

        public Board(int rows, int columns, int bombs)
        {
            _rows = rows;
            _columns = columns;
            _bombs = bombs;

            board = new Cell[rows, columns];
            // Make default mine properties
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    board[r, c].visited = false;
                    board[r, c].type = Hidden;
                    board[r, c].display = HiddenChar;
                    board[r, c].mineCount = 0;
                }
            }
        }

        public void populateMines()
        {

            int bombCount = 0;
            while (bombCount < _bombs)
            {
                var random = new Random();
                // Choose random tuple of indices
                int randomRow = random.Next(0, _rows);
                int randomCol = random.Next(0, _columns);

                // If chosen tuple has already been chosen as a bomb, restart process
                if (board[randomRow, randomCol].display == "*")
                {
                    continue;
                }
                // otherwise change cell to a mine and increase count
                else
                {
                    board[randomRow, randomCol].type = Bomb;
                    board[randomRow, randomCol].display = BombChar;
                    bombCount += 1;
                }
            }
            // Loop through array and count each cell's neighboring mines
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    countNeighboringMines(r, c);
                }
            }
        }

        public void countNeighboringMines(int r, int c)
        {
            // Loop through all surrounding cells, if there's a mine surrounding increase mine count
            for (int i = r - 1; i <= r + 1; i++)
            {
                for (int j = c - 1; j <= c + 1; j++)
                {
                    if (i >= 0 && i < _rows &&
                        j >= 0 && j < _columns)
                        board[r, c].mineCount += isBomb(i, j);
                }
            }
        }
        public int isBomb(int r, int c)
        {
            return board[r, c].type == Bomb ? 1 : 0;
        }

        public void printBoard()
        {
            Console.Write("\t");
            // index the columns
            for (int i = 1; i < _columns + 1; i++)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine("\n");
            for (int i = 0; i < _rows; i++)
            {
                // index the rows
                Console.Write((i + 1) + "\t");
                for (int j = 0; j < _columns; j++)
                {
                    // If cell isn't a mine, show mine count
                    if (board[i, j].type != Bomb)
                    {
                        Console.Write(board[i, j].mineCount + " ");
                    }
                    // Otherwise display mine
                    else
                    {
                        Console.Write(board[i, j] + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}