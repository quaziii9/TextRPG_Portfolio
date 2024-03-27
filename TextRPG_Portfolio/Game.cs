using System;
using System.Numerics;
using System.Threading;
using TextRPG_Portfolio;
using TextRPG_Portfolio.Item;
using TextRPG_Portfolio.Unit;
using TextRPG_Portfolio.Unit.Monster;
using static TextRPG_Portfolio.Unit.Map;


namespace TextRPG_Portfolio
{
    public enum GameMode
    {
        Lobby,
        Town,
        Store,
        Home,
        EasyField,
        HardField,      
    }

    internal class Game
    {
        private GameMode gmode = GameMode.Lobby;
       
        public void playing(uPlayer player, Map Townboard, Store store, PlayerItemList playerInven)
        {
          
            switch (gmode)
           {
               case GameMode.Lobby:
                   playingLobby(player);
                   break;
               case GameMode.Town:
                   Console.Clear();
                   playingTown(player, Townboard, playerInven);
                   break;
                case GameMode.Store:
                    Console.Clear();
                    playingStore(store, player, playerInven);
                    break;
                case GameMode.Home:
                    Console.Clear();
                    playingHome(player, playerInven, Townboard);
                    break;

                case GameMode.EasyField:
                    Console.Clear();
                    // 필드에 들어올때마다 몬스터 리셋
                    #region easyfield 생성 작업
                    WESTMap westMap = new WESTMap();
                    player.Changeplayerboard(westMap);
                    MonsterBoard wmonsterboard1 = new MonsterBoard();
                    MonsterBoard wmonsterboard2 = new MonsterBoard();
                    Goblin wmonster1 = new Goblin();
                    Goblin wmonster2 = new Goblin();
                    
                    wmonsterboard1.Initialize(15, 2, 13);
                    wmonster1.Initializeex(4, 1, wmonsterboard1);
                    westMap.Initialize(15, wmonsterboard1);

                    wmonsterboard2.Initialize(15, 13, 11);
                    wmonster2.Initialize2(1, 4, wmonsterboard2);
                    westMap.Initialize(15, wmonsterboard2);
                    #endregion
                    playingEasyField(player, wmonster1, wmonster2 , westMap, Townboard,playerInven);
                    break;

                case GameMode.HardField:
                    Console.Clear();
                    #region hardfiled 생성 작업
                    EASTMap eastMap = new EASTMap();
                    player.Changeplayerboard(eastMap);
                    MonsterBoard emonsterboard1 = new MonsterBoard();
                    MonsterBoard emonsterboard2 = new MonsterBoard();
                    Ork emonster1 = new Ork();
                    Ork emonster2 = new Ork();


                    emonsterboard1.Initialize(15, 13, 13);
                    emonster1.Initialize(2, 1, emonsterboard1);
                    eastMap.Initialize(15, emonsterboard1);

                    emonsterboard2.Initialize(15, 13, 2);
                    emonster2.Initialize(1, 4, emonsterboard2);
                    eastMap.Initialize(15, emonsterboard2);
                    #endregion
                    playingHardField(player, emonster1, emonster2, eastMap, Townboard, playerInven);
                    break;
            }
        }

        private void playingLobby(uPlayer player)
        {
            Console.WriteLine("플레이어의 이름을 입력해주세요");
            string name = Console.ReadLine();
            player.SetName(name);
            gmode = GameMode.Town;
        }

        private void playingTown(uPlayer player, Map board, PlayerItemList playerInven)
        {
            bool playingTown = true;
            Console.WriteLine("타운");

            while (playingTown)
            {
                board.Render(player);
                player.State();
                playerInven.PrintPlayerInven(player);            
                player.PlayerMove();

                #region 필드 이동
                if (board.board[player.PosY, player.PosX] == BoardType.NORTH)
                {
                    gmode = GameMode.Store;
                    playingTown = false;
                }
                else if (board.board[player.PosY, player.PosX] == BoardType.SOUTH)
                {
                    gmode = GameMode.Home;
                    playingTown = false;
                }
                else if (board.board[player.PosY, player.PosX] == BoardType.WEST)
                {
                    gmode = GameMode.EasyField;
                    playingTown = false;
                }
                else if (board.board[player.PosY, player.PosX] == BoardType.EAST)
                {
                    gmode = GameMode.HardField;
                    playingTown = false;
                }
                #endregion
            }
        }

        private void playingStore(Store store, uPlayer player, PlayerItemList playerInven)
        {
            // 상점 아이템 갯수가 0이 아닐때 (게임 시작 전 세팅 -> 갯수가 정해지게끔)
            if (store.storeItem.Count != 0) store.StorePlaying(store, player, playerInven, gmode);
            // 상점 아이템 갯수가 0일때 
            else store.StoreListEmtpy(store, gmode);

            player.SetPos(1, 7);
            gmode = GameMode.Town;
        }

        private void playingEasyField(uPlayer player,uMonster umonster1, uMonster umonster2, Map westmap,Map townMap, PlayerItemList playerInven)
        {
           
            bool playingField = true;

            player.SetPos(7, 13);
            int lastTick = 0;

            while (playingField)
            {
                //Console.SetCursorPosition(0, 0);
                //Console.WriteLine("EasyField");
                #region 프레임 관리
                // FPS 프레임(60프레임 OK, 30프레임 이하 NO)
                const int WAIT_TICK = 1000 / 30;

                int currentTick = System.Environment.TickCount;
                int elapsedTick = currentTick - lastTick;

                // 만약에 경과한 시간이 1 * 1000/30초보다 작다면 (밀리 새컨드 단위라서 * 1000)
                if (elapsedTick < WAIT_TICK) continue;

                // 현재 시간을 lastTick에 업데이트
                lastTick = currentTick;
                #endregion

                umonster1.Update(elapsedTick);
                umonster2.Update(elapsedTick);
                
                if (umonster1.isDead()) umonster1.isDeadPos();
                if (umonster2.isDead()) umonster2.isDeadPos();

                player.State();
                playerInven.PrintPlayerInven(player);
                player.PlayerMove();

                #region 몬스터 & 플레이어 충돌 
                if ((player.PosY == umonster1.PosY && player.PosX == umonster1.PosX))
                {
                    Console.SetCursorPosition(0, 23);
                    Console.WriteLine($"{umonster1.GetName()}과 부딪혔다!!");
                    westmap.MonsterMapRender(player, umonster1, umonster2);
                    Thread.Sleep(1000);
                    while (true)
                    {
                        Battle(player, playerInven, umonster1);
                        break;
                    }

                    Console.Clear();
                }
                if ((player.PosY == umonster2.PosY && player.PosX == umonster2.PosX))
                {
                    Console.SetCursorPosition(0, 23);
                    Console.WriteLine($"{umonster2.GetName()}과 부딪혔다!!");
                    westmap.MonsterMapRender(player ,umonster1, umonster2);
                    Thread.Sleep(1000);
                    while (true)
                    {
                        Battle(player, playerInven, umonster2);
                        break;
                    }

                    Console.Clear();
                }
                #endregion

                if (player.isDead())
                {
                    player.deadheal();
                    gmode = GameMode.Home;
                    break;
                }

                westmap.MonsterMapRender(player, umonster1, umonster2);

                if (westmap.board[player.PosY, player.PosX] == BoardType.EAST)
                {
                    player.Changeplayerboard(townMap);
                    player.SetPos(7, 1);
                    gmode = GameMode.Town;
                    playingField = false;
                }
            }

        }

        private void playingHardField(uPlayer player, uMonster umonster1, uMonster umonster2, Map eastmap, Map townMap, PlayerItemList playerInven)
        {
            bool playingField = true;
            
            player.SetPos(7, 1);
            int lastTick = 0;

            while (playingField)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("HardField");
                #region 프레임 관리
                // FPS 프레임(60프레임 OK, 30프레임 이하 NO)
                const int WAIT_TICK = 1000 / 30;

                int currentTick = System.Environment.TickCount;
                int elapsedTick = currentTick - lastTick;

                // 만약에 경과한 시간이 1 * 1000/30초보다 작다면 (밀리 새컨드 단위라서 * 1000)
                if (elapsedTick < WAIT_TICK) continue;

                // 현재 시간을 lastTick에 업데이트
                lastTick = currentTick;
                #endregion

                umonster1.Update(elapsedTick);
                umonster2.Update(elapsedTick);
                if (umonster1.isDead())
                {
                    umonster1.isDeadPos();
                }
                if (umonster2.isDead())
                {
                    umonster2.isDeadPos();
                }

                player.State();
                playerInven.PrintPlayerInven(player);
                player.PlayerMove();

                if ((player.PosY == umonster1.PosY && player.PosX == umonster1.PosX))
                {
                    Console.SetCursorPosition(0, 23);
                    Console.WriteLine($"{umonster1.GetName()}와 부딪혔다!!");
                    eastmap.MonsterMapRender(player, umonster1, umonster2);
                    Thread.Sleep(1000);
                    while (true)
                    {
                        Battle(player, playerInven, umonster1);
                        break;
                    }

                    Console.Clear();
                }
                if (player.isDead())
                {
                    player.deadheal();
                    gmode = GameMode.Home;
                    break;
                }
                if ((player.PosY == umonster2.PosY && player.PosX == umonster2.PosX))
                {
                    Console.SetCursorPosition(0, 23);
                    Console.WriteLine($"{umonster2.GetName()}와 부딪혔다!!");
                    eastmap.MonsterMapRender(player, umonster1, umonster2);
                    Thread.Sleep(1000);
                    while (true)
                    {
                        Battle(player, playerInven, umonster2);
                        break;
                    }

                    Console.Clear();
                }

                if (player.isDead()) 
                {
                    player.deadheal();                  
                    gmode = GameMode.Home; 
                    break; 
                }

                eastmap.MonsterMapRender(player, umonster1, umonster2);

                if (eastmap.board[player.PosY, player.PosX] == BoardType.EAST)
                {
                    player.Changeplayerboard(townMap);
                    player.SetPos(7, 1);
                    gmode = GameMode.Town;
                    playingField = false;
                }

                if (eastmap.board[player.PosY, player.PosX] == BoardType.WEST)
                {
                    player.Changeplayerboard(townMap);
                    player.SetPos(7, 13);
                    gmode = GameMode.Town;
                    playingField = false;
                }
            }
        }

        private void playingHome(uPlayer player, PlayerItemList playerInven, Map townmap)
        {
            bool plaingHome = true;
            Console.WriteLine("포근한 나의 집");
            while (plaingHome)
            {
                Console.WriteLine("1.체력 회복 , 2.장비장착, 3.장비해제 0.나가기 : ");
                Console.WriteLine($"현재 체력 : {player.GetHp()} , 최대체력 : {player.GetMaxHp()}");
                Console.WriteLine($"현재 공격력 : {player.GetAttack()}");
                string homeAct;
                homeAct = Console.ReadLine();

                if (homeAct == "1")
                {
                    player.healing();

                }
                else if (homeAct == "2")
                {
                    Console.Clear();
                    playerInven.PrintEquipItem(player, playerInven);

                }
                else if (homeAct == "3")
                {
                    Console.Clear();
                    playerInven.PrintfEquipItem(player, playerInven);
                }
                else if (homeAct == "0")
                {
                    player.Changeplayerboard(townmap);                
                    gmode = GameMode.Town;
                    player.SetPos(13, 7);
                    plaingHome = false;
                    break;
                }       
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Thread.Sleep(1000);
                }
                Console.Clear();
            }
        }

        public void Battle(uPlayer player, PlayerItemList playerInven, uMonster monster)
        {
            Console.Clear();
            if (!monster.isDead())
            {
                Console.WriteLine($"{monster.GetName()}와 마주쳤다!");
                bool battle = true;
                while (battle)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("1.공격, 2.아이템사용, 3.도망간다");

                    player.State();
                    playerInven.PrintPlayerInven(player);
                    monster.mState();
                    Console.SetCursorPosition(0, 1);

                    string act = Console.ReadLine();

                    switch (act)
                    {
                        case "1":
                            monster.onDamaged(player.GetAttack());

                            if (monster.isDead())
                            {
                                Console.WriteLine($"{monster.GetName()} 사망했습니다!");
                                player.Victory(monster.GetExp(), monster.GetMoney() );
                                Console.WriteLine($"획득한 exp : {monster.GetExp()}, 획득한 머니{monster.GetMoney()}");
                                Console.WriteLine($"{player.GetName()}의 남은체력 : {player.GetHp()}");
                                Thread.Sleep(2000);
                                battle = false;
                                break;
                            }
                            player.onDamaged(monster.GetAttack());
                            if (player.isDead())
                            {
                                player.LoseExp();
                                Console.WriteLine($"{player.GetName()}가 사망했습니다 exp를 잃어 {player.GetExp()}가 되었습니다");
                                Console.WriteLine("집으로 귀환합니다");
                                Thread.Sleep(2000);
                                battle = false;
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"{player.GetName()}의 남은체력 : {player.GetHp()}");
                                Console.WriteLine($"{monster.GetName()}의 남은체력 : {monster.GetHp()}");
                                Thread.Sleep(2000);
                                Console.Clear();
                            }
                            break;
                        case "2":
                            playerInven.PrintPotionItem(player, playerInven);

                            break;
                        case "3":
                            battle = false;
                            break;
                    }
                }
            }
        }
    }
}

