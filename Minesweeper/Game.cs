using System.Drawing;
using System.Text.RegularExpressions;
using static Minesweeper.Utility;
namespace Minesweeper
{
    public class Game
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _bombs;
        private readonly int _numMoves = 0;
        public Cell[,] game;
        public Board board;
        public Game(int width, int height, int bombs)
        {
            _width = width;
            _height = height;
            _bombs = bombs;

            board = new Board(_width, _height, _bombs);

        }

        public void setFlag(int r, int c)
        {
            board.board[r, c].display = FlagChar;
        }

        public void play()
        {
            Console.WriteLine("What would you like to do? (s)elect, (f)lag, (p)rint, (c)heat, or (q)uit");
            while (true)
            {
                char userAction = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (userAction)
                {
                    case 's':
                        Console.WriteLine("Please enter your desired row: ");

                        int selectedRow;
                        while(!int.TryParse(Console.ReadLine(), out selectedRow) || (selectedRow < 0 || selectedRow > _width))
                        {
                            Console.WriteLine("That was an invalid row number. Please enter a valid value.");
                        }

                        Console.WriteLine("Please enter your desired column: ");
                        int selectedColumn;
                        while (!int.TryParse(Console.ReadLine(), out selectedColumn) || (selectedColumn < 0 || selectedColumn > _height))
                        {
                            Console.WriteLine("That was an invalid column number. Please enter a valid value.");
                        }

                        if (board.isBomb(selectedRow - 1, selectedColumn - 1) == 1)
                        {
                            board.board[selectedRow - 1, selectedColumn - 1].type = Bomb;
                            Console.WriteLine("Game over!");
                            Environment.Exit(0);
                        }
                        else {

                            board.revealCell(selectedRow - 1, selectedColumn - 1);
                        }

                        break;
                    case 'f':
                        Console.WriteLine("Please enter your desired row: ");
                        int flaggedRow;
                        while (!int.TryParse(Console.ReadLine(), out flaggedRow) || (flaggedRow < 0 || flaggedRow > _width))
                        {
                            Console.WriteLine("That was an invalid row number. Please enter a valid value.");
                        }
                        Console.WriteLine("Please enter your desired column: ");
                        int flaggedColumn;
                        while (!int.TryParse(Console.ReadLine(), out flaggedColumn) || (flaggedColumn < 0 || flaggedColumn > _height))
                        {
                            Console.WriteLine("That was an invalid column number. Please enter a valid value.");
                        }
                        setFlag(flaggedRow - 1, flaggedColumn - 1);
                        break;
                    case 'c':
                        board.printRevealedBoard();
                        break;
                    case 'p':
                        board.printBoard();
                        break;
                    case 'q':
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please enter a valid action, would you like to (s)elect, (f)lag, (p)rint, (c)heat, or (q)uit");
                        Console.WriteLine("");
                        break;
                }
                board.printBoard();

            }
        }

        public static void Main(string[] args)
        {
            Console.Write("How many rows would you like? ");
            var userRows = Convert.ToInt32(Console.ReadLine());
            Console.Write("How many columns would you like? ");
            var userColumns = Convert.ToInt32(Console.ReadLine());
            Console.Write("How many mines would you like total? ");
            var userMines = Convert.ToInt32(Console.ReadLine());
            Game currGame = new Game(userRows, userColumns, userMines);
            currGame.board.populateMines();
            currGame.board.printBoard();
            currGame.play();
        }
    }
}
