using System.Drawing;
namespace Minesweeper
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
    internal class Board
    {
        private readonly int _rowMax;
        private readonly int _colMax;
        private readonly int _mines;
        public Cell[,] board;

        public Board(int rows, int columns, int mines)
        {
            _rowMax = rows;
            _colMax = columns;
            _mines = mines;

            board = new Cell[rows, columns];
            // set default cell properties
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    board[r, c].visited = false;
                    setType(r, c, Constants.HIDDEN);
                    board[r, c].mineCount = 0;
                }
            }
        }

        public void populateBoard(int firstRow, int firstCol)
        {
            // this is count of placed mines
            int mineCount = 0;
            while (mineCount < _mines)
            {
                var random = new Random();
                // Choose random tuple of indices
                int randomRow = random.Next(0, _rowMax - 1);
                int randomCol = random.Next(0, _colMax - 1);

                // Get distance 
                int rowDistance = Math.Abs(randomRow - firstRow);
                int columnDistance = Math.Abs(randomCol - firstCol);

                // We want to make sure user's first move isn't a mine, and isn't just a single numbered cell, and 
                // if chosen tuple has already been chosen as a mine, restart process
                if ((rowDistance <= 1 && columnDistance <= 1) || isMine(randomRow, randomCol))
                {
                    continue;
                }
                // otherwise change cell to a mine and increase count
                else
                {
                    setType(randomRow, randomCol, Constants.MINE);
                    mineCount += 1;
                }
            }
            // Loop through array and count each cell's neighboring mines
            for (int r = 0; r < _rowMax; r++)
            {
                for (int c = 0; c < _colMax; c++)
                {
                    countNeighboringMines(r, c);
                }
            }
        }

        public void countNeighboringMines(int r, int c)
        {
            // Loop through all surrounding cells, if there's a mine surrounding increase mine count
            int[] deltaX = new int[] { 0, 0, 1, 1, -1, -1, 1, -1 };
            int[] deltaY = new int[] { 1, -1, 0, -1, 0, -1, 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int currX = r + deltaX[i];
                int currY = c + deltaY[i];

                if (currX >= 0 && currX < _rowMax && currY >= 0 && currY < _colMax)
                {
                    board[r, c].mineCount += isMine(currX, currY) ? 1 : 0;
                }
            }
        }

        public bool isMine(int r, int c)
        {
            return board[r, c].type == Constants.MINE;
        }

        // BFS flood fill for selected cells
        public void revealCell(int r, int c)
        {
            if (board[r, c].type == Constants.SHOWN)
            {
                Console.WriteLine("You've already selected this cell! Please choose another one or do another action.");
                return;
            }
            Queue<Point> queue = new Queue<Point>();

            // Create initial point and add to visited & queue
            Point initialPoint = new Point(r, c);
            queue.Enqueue(initialPoint);

            while (queue.Count > 0)
            {
                // Pop current point and set to shown
                Point currPoint = queue.Dequeue();
                setType(currPoint.X, currPoint.Y, Constants.SHOWN);
                // Allows us to fill until first cells that have some neighboring mine
                if (board[currPoint.X, currPoint.Y].mineCount != 0)
                {
                    continue;
                }
                // Loop through all 8 adjacent points
                int[] deltaX = new int[] { 0, 0, 1, 1, -1, -1, 1, -1 };
                int[] deltaY = new int[] { 1, -1, 0, -1, 0, -1, 1, 1 };

                for (int i = 0; i < 8; i++)
                {
                    int neighborX = currPoint.X + deltaX[i];
                    int neighborY = currPoint.Y + deltaY[i];
                    // if points are in bounds and point is valid to be revealed, add to queue/set.
                    if (neighborX >= 0 && neighborX < _rowMax && neighborY >= 0 && neighborY < _colMax)
                    {
                        Point pointToAdd = new Point(neighborX, neighborY);
                        if (board[neighborX, neighborY].type == Constants.HIDDEN)
                        {
                            queue.Enqueue(pointToAdd);
                        }
                    }
                }
            }
        }

        public bool gameWon()
        {
            // if hidden count is 0 while game is going on, game should be over with 'x' remaining mines on the board.
            int hiddenCount = 0;

            foreach (var cell in board)
            {
                if (cell.type == Constants.HIDDEN) { hiddenCount++; }
            }
            return hiddenCount == 0;
        }
        public void setType(int r, int c, int type)
        {
            switch (type)
            {
                case Constants.HIDDEN:
                    board[r, c].type = type;
                    board[r, c].display = "-";
                    break;
                case Constants.SHOWN:
                    board[r, c].type = Constants.SHOWN;
                    board[r, c].display = board[r, c].mineCount.ToString();
                    break;
                case Constants.FLAG:
                    board[r, c].display = "F";
                    break;
                case Constants.MINE:
                    board[r, c].type = type;
                    board[r, c].display = "*";
                    break;
            }
        }

        public void printBoard()
        {
            string res = "\n\t";
            // index the columns
            for (int i = 1; i < _colMax + 1; i++)
            {
                res += $"{i} ";
            }
            res += "\n\n";
            for (int i = 0; i < _rowMax; i++)
            {
                // index the rows
                res += (i + 1) + "\t";
                for (int j = 0; j < _colMax; j++)
                {
                    // If cell is type shown, show its mine count
                    if (board[i, j].type == Constants.SHOWN)
                    {
                        res += $"{board[i, j].mineCount} ";
                    }
                    // If it's a mine and user hasn't flagged, then show hidden character
                    else if (board[i, j].type == Constants.MINE && board[i, j].display != Constants.FLAGCHAR)
                    {
                        res += $"{Constants.HIDDENCHAR} ";
                    }
                    // Otherwise show its default display, e.g. flags, hidden, etc.
                    else
                    {
                        res += $"{board[i, j]} ";
                    }

                    // If indices are 2 integers, increase spacing, we only allow for row/columns up to 99.
                    if (j >= 9)
                    {
                        res += " ";

                    }
                }
                res += "\n";
            }
            Console.WriteLine(res);
        }

        public void printRevealedBoard()
        {
            string res = "\n\t";
            // index the columns
            for (int i = 1; i < _colMax + 1; i++)
            {
                res += $"{i} ";
            }
            res += "\n\n";
            for (int i = 0; i < _rowMax; i++)
            {
                // index the rows
                res += (i + 1) + "\t";
                for (int j = 0; j < _colMax; j++)
                {
                    // If not a mine, show mine count, otherwise show mine symbol.
                    res += board[i, j].type != Constants.MINE
                        ? $"{board[i, j].mineCount} "
                        : $"{board[i, j]} ";
                    // If indices are 2 integers, increase spacing, we only allow for row/columns up to 99.
                    {
                        res += " ";

                    }
                }
                res += "\n";
            }
            Console.WriteLine(res);
        }
    }
}