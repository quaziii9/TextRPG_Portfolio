using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextRPG_Portfolio;
using TextRPG_Portfolio.Unit.Monster;


namespace TextRPG_Portfolio.Unit
{
    internal class MonsterBoard : Map
    {
        public int _DestY { get; private set; }
        public int _DestX { get; private set; }

        public void Initialize(int size, int y, int x) 
        {
            if (size % 2 == 0)
                return;

            board = new BoardType[size, size];
            Size = size;

            _DestY = y;
            _DestX = x;

            GenerateBySideWinder();
        }

        void GenerateBySideWinder()
        {
            // 길을 다 막아버리는 작업
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        board[y, x] = BoardType.WALL;
                    else
                        board[y, x] = BoardType.NONE;
                }
            }

            // 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                int count = 1;
                for (int x = 0; x < Size; x++)
                {
                    // 막힌 부분
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;
                    // 가장자리 부분
                    if (y == Size - 2 && x == Size - 2) continue;
                    
                    // y축 맨끝 부분은 다 뚫어둠 (외각에서 보드를 뚫을수도 있음)
                    if (y == Size - 2)
                    {
                        board[y, x + 1] = BoardType.NONE;
                        continue;
                    }
                    // x축 맨끝부분은 다 뚫어둠
                    if (x == Size - 2)
                    {
                        board[y + 1, x] = BoardType.NONE;
                        continue;
                    }

                    // 1/2확률로 우측길을 뚫음
                    if (rand.Next(0, 2) == 0)
                    {
                        board[y, x + 1] = BoardType.NONE;
                        count++;
                    }
                    // 아래로 길을 뚫음 
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                        // 벽 빈공간 이어지기 때문에 *2
                        board[y + 1, x - randomIndex * 2] = BoardType.NONE;
                        count = 1;
                    }
                }
            }
        }

    }
}
