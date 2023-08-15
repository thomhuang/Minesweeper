using System.Drawing;
using System.Text;

namespace Minesweeper
{
    internal class Board
    {
        private struct Cell
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

        // Used to grab surrounding 8 neighbors
        private static readonly int[] deltaX = { 0, 0, 1, 1, -1, -1, 1, -1 };
        private static readonly int[] deltaY = { 1, -1, 0, -1, 0, -1, 1, 1 };

        private readonly int _rowMax;
        private readonly int _colMax;
        private readonly int _mines;
        private Cell[,] _board;

        public Board(int rows, int columns, int mines)
        {
            _rowMax = rows;
            _colMax = columns;
            _mines = mines;

            _board = new Cell[rows, columns];
            // set default cell properties
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    _board[r, c].visited = false;
                    setType(r, c, Constants.HIDDEN);
                    _board[r, c].mineCount = 0;
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

        private void countNeighboringMines(int r, int c)
        {
            // Loop through all surrounding cells, if there's a mine surrounding increase mine count
            for (int i = 0; i < 8; i++)
            {
                int currX = r + deltaX[i];
                int currY = c + deltaY[i];

                if (currX >= 0 && currX < _rowMax && currY >= 0 && currY < _colMax)
                {
                    _board[r, c].mineCount += isMine(currX, currY) ? 1 : 0;
                }
            }
        }

        public bool isMine(int r, int c) => _board[r, c].type == Constants.MINE;

        // BFS flood fill for selected cells
        public void revealCell(int r, int c)
        {
            if (_board[r, c].type == Constants.SHOWN)
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
                if (_board[currPoint.X, currPoint.Y].mineCount != 0)
                {
                    continue;
                }
                // Loop through all 8 adjacent points
                for (int i = 0; i < 8; i++)
                {
                    int neighborX = currPoint.X + deltaX[i];
                    int neighborY = currPoint.Y + deltaY[i];
                    // if points are in bounds and point is valid to be revealed, add to queue/set.
                    if (neighborX >= 0 && neighborX < _rowMax && neighborY >= 0 && neighborY < _colMax)
                    {
                        Point pointToAdd = new Point(neighborX, neighborY);
                        if (_board[neighborX, neighborY].type == Constants.HIDDEN)
                        {
                            queue.Enqueue(pointToAdd);
                        }
                    }
                }
            }
        }

        public bool gameWon()
        {
            // if hidden count is 0 while game is going on, game should be over with 'x' remaining mines on the _board.
            int hiddenCount = 0;

            foreach (var cell in _board)
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
                    _board[r, c].type = type;
                    _board[r, c].display = "-";
                    break;
                case Constants.SHOWN:
                    _board[r, c].type = Constants.SHOWN;
                    _board[r, c].display = _board[r, c].mineCount.ToString();
                    break;
                case Constants.FLAG:
                    _board[r, c].display = "F";
                    break;
                case Constants.MINE:
                    _board[r, c].type = type;
                    _board[r, c].display = "*";
                    break;
            }
        }

        public void printBoard()
        {
            var sb = new StringBuilder("\n\t");
            // index the columns
            for (int i = 1; i < _colMax + 1; i++)
            {
                sb.Append($"{i} ");
            }
            sb.Append("\n\n");
            for (int i = 0; i < _rowMax; i++)
            {
                // index the rows
                sb.Append((i + 1) + "\t");
                for (int j = 0; j < _colMax; j++)
                {
                    switch (_board[i,j].type)
                    {
                        case Constants.SHOWN:
                            sb.Append($"{_board[i, j].mineCount} ");
                            break;
                        case Constants.MINE:
                            break;
                    }
                    // If cell is type shown, show its mine count
                    if (_board[i, j].type == Constants.SHOWN)
                    {
                    }
                    // If it's a mine and user hasn't flagged, then show hidden character
                    else if (_board[i, j].type == Constants.MINE && _board[i, j].display != Constants.FLAGCHAR)
                    {
                        sb.Append($"{Constants.HIDDENCHAR} ");
                    }
                    // Otherwise show its default display, e.g. flags, hidden, etc.
                    else
                    {
                        sb.Append($"{_board[i, j]} ");
                    }

                    // If indices are 2 integers, increase spacing, we only allow for row/columns up to 99.
                    if (j >= 9)
                    {
                        sb.Append(" ");

                    }
                }
                sb.Append("\n");
            }
            Console.WriteLine(sb.ToString());
        }

        public void printRevealedBoard()
        {
            var sb = new StringBuilder("\n\t");
            // index the columns
            for (int i = 1; i < _colMax + 1; i++)
            {
                sb.Append($"{i} ");
            }
            sb.Append("\n\n");
            for (int i = 0; i < _rowMax; i++)
            {
                // index the rows
                sb.Append((i + 1) + "\t");
                for (int j = 0; j < _colMax; j++)
                {
                    // If not a mine, show mine count, otherwise show mine symbol.
                    sb.Append(_board[i, j].type != Constants.MINE
                        ? $"{_board[i, j].mineCount} "
                        : $"{_board[i, j]} ");
                    // If indices are 2 integers, increase spacing, we only allow for row/columns up to 99.
                    if (j > 9)
                    {
                        sb.Append(" ");

                    }
                }
                sb.Append("\n");
            }
            Console.WriteLine(sb.ToString());
        }
    }
}