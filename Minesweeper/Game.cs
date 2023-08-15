namespace Minesweeper
{
    internal class Game
    {
        private int rowMax;
        private int colMax;
        private int numMoves = 0;
        public Board board;

        public Game() => newGame();

        private void newGame()
        {
            int userRows, userColumns, userMines;
            Console.Write("How many rows would you like? The range of rows is [1,99]. ");
            while (!int.TryParse(Console.ReadLine(), out userRows) || userRows < 1 || userRows >= 100)
            {
                Console.Write("That was an invalid integer. Please enter a valid value: ");
            }

            rowMax = userRows;

            Console.Write("How many columns would you like? The range of columns is [1, 99]. ");
            while (!int.TryParse(Console.ReadLine(), out userColumns) || userColumns < 1 || userColumns >= 100)
            {
                Console.Write("That was an invalid integer. Please enter a valid value: ");
            }
            colMax = userColumns;

            Console.Write("How many mines would you like total? ");
            while (!int.TryParse(Console.ReadLine(), out userMines) || (userMines >= (userRows * userColumns)))
            {
                Console.Write("That was an invalid mine number. Please enter a valid value: ");
            }

            board = new Board(userRows, userColumns, userMines);
            board.printBoard();
        }

        private void setFlag(int r, int c) => board.setType(r, c, Constants.FLAG);

        public void play()
        {
            while (true)
            {
                Console.WriteLine("What would you like to do? (s)elect, (f)lag, (p)rint, (c)heat, (r)eset, or (q)uit");
                string userAction = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
                switch (userAction.ToLower())
                {
                    case "s":
                        Console.WriteLine("Please enter your desired row: ");
                        int selectedRow;

                        while (!int.TryParse(Console.ReadLine(), out selectedRow) || (selectedRow <= 0 || selectedRow > rowMax))
                        {
                            Console.WriteLine("That was an invalid row number. Please enter a valid value: ");
                        }

                        Console.WriteLine("Please enter your desired column: ");
                        int selectedColumn;

                        while (!int.TryParse(Console.ReadLine(), out selectedColumn) || (selectedColumn <= 0 || selectedColumn > colMax))
                        {
                            Console.WriteLine("That was an invalid column number. Please enter a valid value: ");
                        }

                        if (board.isMine(selectedRow - 1, selectedColumn - 1))
                        {
                            Console.WriteLine("Game over!");
                            Environment.Exit(0);
                        }
                        else if (numMoves == 0)
                        {
                            board.populateBoard(selectedRow - 1, selectedColumn - 1);
                        }
                        board.revealCell(selectedRow - 1, selectedColumn - 1);
                        numMoves += 1;

                        board.printBoard();

                        if (board.gameWon())
                        {
                            Console.WriteLine($"You've won in {numMoves} selections, good job!");
                            board.printRevealedBoard();
                            Environment.Exit(0);
                        }

                        break;
                    case "f":
                        Console.WriteLine("Please enter your desired row: ");
                        int flaggedRow;

                        while (!int.TryParse(Console.ReadLine(), out flaggedRow) || (flaggedRow <= 0 || flaggedRow > rowMax))
                        {
                            Console.WriteLine("That was an invalid row number. Please enter a valid value: ");
                        }

                        Console.WriteLine("Please enter your desired column: ");
                        int flaggedColumn;

                        while (!int.TryParse(Console.ReadLine(), out flaggedColumn) || (flaggedColumn <= 0 || flaggedColumn > colMax))
                        {
                            Console.WriteLine("That was an invalid column number. Please enter a valid value: ");
                        }
                        setFlag(flaggedRow - 1, flaggedColumn - 1);
                        board.printBoard();
                        break;
                    case "c":
                        if (numMoves == 0)
                        {
                            Console.WriteLine("Please enter a move first before revealing the board!");
                        }
                        else
                        {
                            board.printRevealedBoard();
                        }
                        break;
                    case "p":
                        board.printBoard();
                        break;
                    case "q":
                        Environment.Exit(0);
                        break;
                    case "r":
                        Console.WriteLine("The board has reset! Please enter the new properties you'd like for the next board: ");
                        newGame();
                        break;
                    default:
                        Console.WriteLine("Please enter a valid action!");
                        break;
                }

            }
        }
    }
}
