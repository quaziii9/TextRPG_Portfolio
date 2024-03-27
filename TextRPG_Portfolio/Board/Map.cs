using System.Numerics;
using TextRPG_Portfolio.Unit.Monster;


namespace TextRPG_Portfolio.Unit
{
    internal class Map
    {
        public enum BoardType
        {
            EAST,
            WEST,
            NORTH,
            SOUTH,
            NONE,
            WALL,
        }

        protected const char RECTANGLE = '■';
        protected const char PLAYER = '▲';
        protected const char MONSTER = '★';

        public BoardType[,] board { get; protected set; }

        public int Size { get; protected set; }

        public int DestY { get; protected set; }
        public int DestX { get; protected set; }

        protected uPlayer _uplayer;
        protected uMonster _umonster;

        public void Initialize(int size)
        {
            // 보드가 짝수일시 리턴
            if (size % 2 == 0)
                return;

            board = new BoardType[size, size];
            Size = size;

            SetBoardType();
        }

        public virtual void SetBoardType()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x == 0 && y == Size / 2)
                        board[y, x] = BoardType.WEST;
                    else if (x == Size - 1 && y == Size / 2)
                        board[y, x] = BoardType.EAST;
                    else if (y == 0 && x == Size / 2)
                        board[y, x] = BoardType.NORTH;
                    else if (y == Size - 1 && x == Size / 2)
                        board[y, x] = BoardType.SOUTH;
                    else if ((x == 0 || y == 0 || x == Size - 1 || y == Size - 1))
                        board[y, x] = BoardType.WALL;
                    else
                        board[y, x] = BoardType.NONE;
                }
            }
        }

        ConsoleColor MAPcolor(BoardType type)
        {
            switch (type)
            {
                case BoardType.WALL:
                    return ConsoleColor.Red;
                case BoardType.EAST:
                case BoardType.WEST:
                case BoardType.NORTH:
                case BoardType.SOUTH:
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.Green;
            }
        }

        public void Render(uPlayer player)
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                Console.SetCursorPosition(15, y + 5);
                for (int x = 0; x < Size; x++)
                {
                    if (y == player.PosY && x == player.PosX)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(PLAYER);
                    }
                    else
                    {
                        Console.ForegroundColor = MAPcolor(board[y, x]);
                        Console.Write(RECTANGLE);
                    }
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = prevColor;
        }

        public void MonsterMapRender(uPlayer player, uMonster monster1, uMonster monster2)
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            for (int y = 0; y < Size; y++)
            {
                Console.SetCursorPosition(15, y + 5);
                for (int x = 0; x < Size; x++)
                {
                    if (y == player.PosY && x == player.PosX)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(PLAYER);                        
                    }

                    else if (y == monster1.PosY && x == monster1.PosX || y == monster2.PosY && x == monster2.PosX)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(MONSTER);
                    }

                    else
                    {
                        Console.ForegroundColor = MAPcolor(board[y, x]);
                        Console.Write(RECTANGLE);
                    }
                }
                Console.WriteLine();
            }
            
            Console.ForegroundColor = prevColor;
        }
    }
}
