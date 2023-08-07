using static Minesweeper.Utility;
namespace Minesweeper
{
    public class Game
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _bombs;
        public Cell[,] game;
        public Board board;
        public Game(int width, int height, int bombs)
        {
            _width = width;
            _height = height;
            _bombs = bombs;

            board = new Board(_width, _height, _bombs);
            
        }
        public static void Main(string[] args)
        {
            //Console.Write("How many columns would you like?");
            Game currGame = new Game(9, 9, 10);
            currGame.board.populateMines();
            currGame.board.printBoard();
        }
    }
}
