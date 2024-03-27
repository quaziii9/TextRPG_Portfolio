using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Portfolio.Unit.Monster
{
    class uMonster : Unit
    {
        Random _random = new Random();
        MonsterBoard _mboard;

        public void Initialize(int posY, int posX, MonsterBoard mboard)
        {
            PosY = posY;
            PosX = posX;
            _mboard = mboard;

            AStar();           
        }
        public void Initialize2(int posY, int posX, MonsterBoard mboard)
        {
            PosY = posY;
            PosX = posX;
            _mboard = mboard;

            AStar2();
        }
        public void Initializeex(int posY, int posX, MonsterBoard mboard)
        {
            PosY = posY;
            PosX = posX;
            _mboard = mboard;
            Stopwatch sw = Stopwatch.StartNew();
            BFS();
            sw.Stop();
            double seconds = sw.Elapsed.TotalSeconds;
            Console.WriteLine($"BFS 소요된 시간 :{seconds}초 ");

            Stopwatch sw2 = Stopwatch.StartNew();
            DFS();
            sw2.Stop();
            double seconds2 = sw.Elapsed.TotalSeconds;
            Console.WriteLine($"DFS 소요된 시간 :{seconds2}초 ");

            Stopwatch sw3 = Stopwatch.StartNew();
            AStar();
            sw3.Stop();
            double seconds3 = sw3.Elapsed.TotalSeconds;
            Console.WriteLine($"대각선을 허용하지 않은 ASTAR 소요된 시간 :{seconds3}초 ");


            Stopwatch sw4 = Stopwatch.StartNew();
            AStar2();
            sw4.Stop();
            double seconds4 = sw4.Elapsed.TotalSeconds;
            Console.WriteLine($"대각선을 허용한 ASTAR 소요된 시간 :{seconds4}초 ");
        }

        public void isDeadPos() { PosX = -1; PosY = -1; }

        public void mState()
        {
            Console.SetCursorPosition(55, 18);
            Console.WriteLine($"----------------- {_name} 스탯 -----------------");
            Console.SetCursorPosition(55, 19);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 20);
            Console.WriteLine($"|\t체력 : {_hp}/{_maxhp} \t\t 공격력 : {_atk}");
            Console.SetCursorPosition(104, 20); Console.WriteLine("|");
            Console.SetCursorPosition(55, 21);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 22);
            Console.WriteLine($"|\texp : {_exp} / Lv: {_lv}\t 돈 : {_money}원");
            Console.SetCursorPosition(104, 22); Console.WriteLine("|");
            Console.SetCursorPosition(55, 23);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 24);
            Console.WriteLine("-------------------------------------------------");
        }

        #region 몬스터 위치
        List<Pos> _points = new List<Pos>();

        public int PosY { get; set; }
        public int PosX { get; set; }


        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                    return 0;
                return F < other.F ? 1 : -1;
            }
        }

        void BFS()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };

            bool[,] found = new bool[_mboard.Size, _mboard.Size];
            Pos[,] parent = new Pos[_mboard.Size, _mboard.Size];


            Queue<Pos> q = new Queue<Pos>();
            q.Enqueue(new Pos(PosY, PosX));
            found[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (q.Count > 0)
            {
                Pos pos = q.Dequeue();
                int nowY = pos.Y;
                int nowX = pos.X;

                for (int i = 0; i < 4; i++)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];
                    if (nextX < 0 || nextX >= _mboard.Size || nextY < 0 || nextY >= _mboard.Size)
                        continue;
                    if (_mboard.board[nextY, nextX] == Map.BoardType.WALL)
                        continue;
                    if (found[nextY, nextX])
                        continue;

                    q.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX);
                }
            }

            CalcPathFromParent(parent);
        }

        void DFS()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };

            bool[,] vistied = new bool[_mboard.Size, _mboard.Size];
            Pos[,] parent = new Pos[_mboard.Size, _mboard.Size];


            Stack<Pos> s = new Stack<Pos>();
            s.Push(new Pos(PosY, PosX));
            vistied[PosY, PosX] = true;
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (s.Count > 0)
            {
                Pos pos = s.Pop();
                int nowY = pos.Y;
                int nowX = pos.X;

                for (int i = 0; i < 4; i++)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];
                    if (nextX < 0 || nextX >= _mboard.Size || nextY < 0 || nextY >= _mboard.Size)
                        continue;
                    if (_mboard.board[nextY, nextX] == Map.BoardType.WALL)
                        continue;
                    if (vistied[nextY, nextX])
                        continue;

                    s.Push(new Pos(nextY, nextX));
                    vistied[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX);
                }
            }

            CalcPathFromParent(parent);
        }

        void AStar()
        {
            // U L D R UP
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1, };

            // 점수 매기기
            // F = G + H
            // F = 최종 점수 (작을 수록 좋음, 경로에 따라 달라짐)
            // G = 시작점에서 해당 좌표까지 이동하는데 드는 비용 (작을 수록 좋음, 경로에 따라 달라짐)
            // H = 목적지에서 얼마나 가까운지 (작을 수록 좋음, 고정)

            // (y, x) 이미 방문했는지 여부 (방문 = closed 상태)
            bool[,] closed = new bool[_mboard.Size, _mboard.Size]; // CloseList

            // (y, x) 가는 길을 한 번이라도 발견했는지
            // 발견X => MaxValue(큰값을 줘서 발견한 값이랑 비용계산에 혼동이 없게)
            // 발견O => F = G + H
            int[,] open = new int[_mboard.Size, _mboard.Size]; // OpenList
            for (int y = 0; y < _mboard.Size; y++)
                for (int x = 0; x < _mboard.Size; x++)
                    open[y, x] = int.MaxValue;      // 초기값 설정

            // 내가 어떻게 이동했는지 추적
            Pos[,] parent = new Pos[_mboard.Size, _mboard.Size];

            // 오픈리스트에 있는 정보들 중에서, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구          
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();


            // 시작점 발견 (예약 진행) math.abs -> 절대값, 보드 목적지 - 현재 위치의 거리 계산
            open[PosY, PosX] = (Math.Abs(_mboard._DestY - PosY) + Math.Abs(_mboard._DestX - PosX));
            pq.Push(new PQNode() { F = (Math.Abs(_mboard._DestY - PosY) + Math.Abs(_mboard._DestX - PosX)), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (pq.Count > 0)
            {
                // 제일 좋은 후보를 찾는다 : F값이 낮은 노드 POP()
                PQNode node = pq.Pop();
                // 동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(closed)된 경우 스킵
                if (closed[node.Y, node.X])
                    continue;

                // 방문한다
                closed[node.Y, node.X] = true;
                // 목적지 도착했으면 바로 종료
                if (node.Y == _mboard._DestY && node.X == _mboard._DestX)
                    break;

                // 상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약(open)한다
                for (int i = 0; i < deltaY.Length; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    // 유효 범위를 벗어났으면 스킵
                    if (nextX < 0 || nextX >= _mboard.Size || nextY < 0 || nextY >= _mboard.Size)
                        continue;
                    // 벽으로 막혀서 갈 수 없으면 스킵
                    if (_mboard.board[nextY, nextX] == Map.BoardType.WALL)
                        continue;
                    // 이미 방문한 곳이면 스킵
                    if (closed[nextY, nextX])
                        continue;

                    // 비용 계산
                    int g = node.G;
                    int h = (Math.Abs(_mboard._DestY - nextY) + Math.Abs(_mboard._DestX - nextX));
                    // 다른 경로에서 더 빠른 길 이미 찾았으면 스킵
                    if (open[nextY, nextX] < g + h)
                        continue;

                    // 예약 진행
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);
                }
            }

            CalcPathFromParent(parent);
        }

        void AStar2()
        {
            // U L D R UL DL DR UR
            int[] deltaY = new int[] { -1, 0, 1, 0, -1, 1, 1, -1 };
            int[] deltaX = new int[] { 0, -1, 0, 1, -1, -1, 1, 1 };
            int[] cost = new int[] { 10, 10, 10, 10, 14, 14, 14, 14 };

            // 점수 매기기
            // F = G + H
            // F = 최종 점수 (작을 수록 좋음, 경로에 따라 달라짐)
            // G = 시작점에서 해당 좌표까지 이동하는데 드는 비용 (작을 수록 좋음, 경로에 따라 달라짐)
            // H = 목적지에서 얼마나 가까운지 (작을 수록 좋음, 고정)

            // (y, x) 이미 방문했는지 여부 (방문 = closed 상태)
            bool[,] closed = new bool[_mboard.Size, _mboard.Size]; // CloseList

            // (y, x) 가는 길을 한 번이라도 발견했는지
            // 발견X => MaxValue(큰값을 줘서 발견한 값이랑 비용계산에 혼동이 없게)
            // 발견O => F = G + H
            int[,] open = new int[_mboard.Size, _mboard.Size]; // OpenList
            for (int y = 0; y < _mboard.Size; y++)
                for (int x = 0; x < _mboard.Size; x++)
                    open[y, x] = int.MaxValue;      // 초기값 설정

            // 내가 어떻게 이동했는지 추적
            Pos[,] parent = new Pos[_mboard.Size, _mboard.Size];

            // 오픈리스트에 있는 정보들 중에서, 가장 좋은 후보를 빠르게 뽑아오기 위한 도구          
            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();


            // 시작점 발견 (예약 진행) math.abs -> 절대값, 보드 목적지 - 현재 위치의 거리 계산
            open[PosY, PosX] = 10 * (Math.Abs(_mboard._DestY - PosY) + Math.Abs(_mboard._DestX - PosX));
            pq.Push(new PQNode() { F = 10 *  (Math.Abs(_mboard._DestY - PosY) + Math.Abs(_mboard._DestX - PosX)), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (pq.Count > 0)
            {
                // 제일 좋은 후보를 찾는다 : F값이 낮은 노드 POP()
                PQNode node = pq.Pop();
                // 동일한 좌표를 여러 경로로 찾아서, 더 빠른 경로로 인해서 이미 방문(closed)된 경우 스킵
                if (closed[node.Y, node.X])
                    continue;

                // 방문한다
                closed[node.Y, node.X] = true;
                // 목적지 도착했으면 바로 종료
                if (node.Y == _mboard._DestY && node.X == _mboard._DestX)
                    break;

                // 상하좌우 등 이동할 수 있는 좌표인지 확인해서 예약(open)한다
                for (int i = 0; i < deltaY.Length; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    // 유효 범위를 벗어났으면 스킵
                    if (nextX < 0 || nextX >= _mboard.Size || nextY < 0 || nextY >= _mboard.Size)
                        continue;
                    // 벽으로 막혀서 갈 수 없으면 스킵
                    if (_mboard.board[nextY, nextX] == Map.BoardType.WALL)
                        continue;
                    // 이미 방문한 곳이면 스킵
                    if (closed[nextY, nextX])
                        continue;

                    // 비용 계산
                    int g = node.G + cost[i];
                    int h = 10 * (Math.Abs(_mboard._DestY - nextY) + Math.Abs(_mboard._DestX - nextX));
                    // 다른 경로에서 더 빠른 길 이미 찾았으면 스킵
                    if (open[nextY, nextX] < g + h)
                        continue;

                    // 예약 진행
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);
                }
            }

            CalcPathFromParent(parent);
        }

        void CalcPathFromParent(Pos[,] parent)
        {
            int EndY = _mboard._DestY;
            int EndX = _mboard._DestX;

            // 목적지에서 시작점까지의 경로 추적
            List<Pos> GoToStart = new List<Pos>();
            while (parent[EndY, EndX].Y != EndY || parent[EndY, EndX].X != EndX)
            {
                GoToStart.Add(new Pos(EndY, EndX));
                Pos pos = parent[EndY, EndX];
                EndY = pos.Y;
                EndX = pos.X;
            }
            GoToStart.Add(new Pos(EndY, EndX));

            // 시작점에서 목적지까지의 경로 저장
            _points.AddRange(GoToStart);

            // 목적지에서 시작점까지의 경로를 역순으로 저장
            List<Pos> Reverse = new List<Pos>();
            for (int i = GoToStart.Count - 1; i >= 0; i--)
            {
                Reverse.Add(GoToStart[i]);
            }

            // 경로를 5배로 늘려줍니다.
            for (int i = 0; i < 5; i++)
            {
                _points.AddRange(Reverse);
                _points.AddRange(GoToStart);
            }
            Random random = new Random();
            
            if(random.Next(0, 2) == 0) _points.Reverse();
            
        }

        const int MOVE_TICK = 50;
        int _sumTick = 0;
        int _lastIndex = 0;
        public void Update(int deltaTick)
        {
            if (_lastIndex >= _points.Count)
            {
                return;
            }

            _sumTick += deltaTick;
            if (_sumTick >= MOVE_TICK)
            {
                _sumTick = 0;

                PosY = _points[_lastIndex].Y;
                PosX = _points[_lastIndex].X;
                _lastIndex++;
            }
        }
        #endregion

    }
}
