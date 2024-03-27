

using TextRPG_Portfolio.Unit.Monster;

namespace TextRPG_Portfolio.Unit
{
    internal class EASTMap : Map
    {
        public void Initialize(int size, MonsterBoard monsterboard)
        {

            if (size % 2 == 0)
                return;

            board = new BoardType[size, size];
            Size = size;
            
            DestY = monsterboard._DestY;
            DestX = monsterboard._DestX;

            SetBoardType();
        }


        public override void SetBoardType()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x == 0 && y == Size / 2)
                        board[y, x] = BoardType.WEST;
                    else if (x == 0 || y == 0 || x == Size - 1 || y == Size - 1)
                        board[y, x] = BoardType.WALL;
                    else
                        board[y, x] = BoardType.NONE;
                }
            }
        }
    }
}
