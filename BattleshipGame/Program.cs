using System;

namespace BattleshipGame
{
    class Ship
    {
        public int Size { get; }

        public Ship(int size)
        {
            Size = size;
        }
    }

    class Player
    {
        public string Name { get; }
        public Ship[] Ships { get; }

        public Player(string name)
        {
            Name = name;
            Ships = new Ship[]
            {
                new Ship(1),
                new Ship(2),
                new Ship(3),
                new Ship(4) 
            };
        }
    }

    class Board
    {
        private char[,] _grid;
        public int Size { get; }

        public Board(int size)
        {
            Size = size;
            _grid = new char[size, size];
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _grid[i, j] = '~';
                }
            }
        }

        public void Display(bool hideShips)
        {
            Console.WriteLine("   A B C D E F G H I J");
            for (int i = 0; i < Size; i++)
            {
                Console.Write($"{i}  ");
                for (int j = 0; j < Size; j++)
                {
                    char displayChar = hideShips && _grid[i, j] == 'X' ? '~' : _grid[i, j];
                    Console.Write($"{displayChar} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void PlaceShip(int x, int y, int size)
        {
            for (int i = 0; i < size; i++)
            {
                _grid[x + i, y] = 'X';
            }
        }

        public bool IsCellFree(int x, int y)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size)
                return false;

            return _grid[x, y] == '~';
        }

        public void MarkHit(int x, int y)
        {
            _grid[x, y] = 'H';
        }

        public void MarkMiss(int x, int y)
        {
            _grid[x, y] = 'M';
        }
    }

    class Game
    {
        private Player _player;
        private Player _computer;
        private Board _playerBoard;
        private Board _computerBoard;
        private Random _random;

        public Game()
        {
            _player = new Player("Player");
            _computer = new Player("Computer");
            _playerBoard = new Board(10);
            _computerBoard = new Board(10);
            _random = new Random();

            PlaceShips(_player.Ships, _playerBoard);
            PlaceShips(_computer.Ships, _computerBoard);
        }

        private void PlaceShips(Ship[] ships, Board board)
        {
            foreach (var ship in ships)
            {
                int x, y;
                do
                {
                    x = _random.Next(0, board.Size);
                    y = _random.Next(0, board.Size);
                } while (!board.IsCellFree(x, y));

                board.PlaceShip(x, y, ship.Size);
            }
        }

        private void PlayerTurn()
        {
            Console.WriteLine("Player's turn.");
            Console.WriteLine("Enter coordinates to attack (e.g., A5):");
            string input = Console.ReadLine().ToUpper();

            if (input.Length != 2 || !char.IsLetter(input[0]) || !char.IsDigit(input[1]))
            {
                Console.WriteLine("Invalid input. Please enter a valid coordinate.");
                PlayerTurn();
                return;
            }

            int x = input[1] - '0';
            int y = input[0] - 'A';

            if (x < 0 || x >= _computerBoard.Size || y < 0 || y >= _computerBoard.Size)
            {
                Console.WriteLine("Coordinates out of bounds. Please enter valid coordinates.");
                PlayerTurn();
                return;
            }

            if (_computerBoard.IsCellFree(x, y))
            {
                Console.WriteLine("Miss!");
                _computerBoard.MarkMiss(x, y);
            }
            else
            {
                Console.WriteLine("Hit!");
                _computerBoard.MarkHit(x, y);
            }
            Console.WriteLine();
        }

        private void ComputerTurn()
        {
            Console.WriteLine("Computer's turn.");
            int x, y;
            do
            {
                x = _random.Next(0, _playerBoard.Size);
                y = _random.Next(0, _playerBoard.Size);
            } while (!_playerBoard.IsCellFree(x, y));

            if (_playerBoard.IsCellFree(x, y))
            {
                Console.WriteLine($"Computer missed at {Convert.ToChar('A' + y)}{x}!");
                _playerBoard.MarkMiss(x, y);
            }
            else
            {
                Console.WriteLine($"Computer hit at {Convert.ToChar('A' + y)}{x}!");
                _playerBoard.MarkHit(x, y);
            }
            Console.WriteLine();
        }

        private bool IsGameOver()
        {
            return false;
        }

        public void Start()
        {
            while (!IsGameOver())
            {
                Console.Clear();
                Console.WriteLine("Player's board:");
                _playerBoard.Display(hideShips: false);

                Console.WriteLine("Computer's board:");
                _computerBoard.Display(hideShips: true);

                PlayerTurn();

                if (IsGameOver())
                    break;

                ComputerTurn();
            }

            Console.WriteLine("Game over. Thank you for playing!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Battleship!");

            Game game = new Game();
            game.Start();
        }
    }
}
