using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Portfolio.Unit
{
    internal class uPlayer : Unit
    {
        
        public uPlayer() 
        { 
            _money = 100;
            _exp = 0;
            _lv = 1;
            _maxhp = 100;
            _hp = 100;
            _atk = 10;
            _name = " ";
        }

        public void Changeplayerboard(Map board)
        {
            _board = board;
        }

        public void Victory(int exp, int money)
        {
            int temp = _lv;
            _exp += exp;
            _money += money;
            _lv = (_exp + 10) / 10;
            if (temp !=_lv)
            {
                _maxhp +=  10*(_lv-1);
                _hp = _maxhp;
                _atk += 5*(_lv-1);
            }
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public void LoseExp()
        {
            int temp = _lv;
            _exp -= 5;
            if (_exp < 0) _exp = 0;
            _lv = (_exp + 10) / 10;
            if (temp != _lv)
            {
                _maxhp -= 10;
                _hp -= 10;
                _atk -=  5;
            }
        }

        public void EquipWeapon(int plusAtk)
        {
            _atk += plusAtk;
        }

        public void EquipArmor(int plusHp)
        {
            _maxhp += plusHp;
            _hp += plusHp;
        }

        public void UsePotion(int healhp)
        {
            _hp += healhp;
            if (_maxhp < _hp) _hp = _maxhp;
        }

        public void useMoney(int money)
        {
            _money = _money - money;
        }

        public void healing()
        {
            _hp = _maxhp;
            Console.WriteLine($"{_name}의 체력이 {_hp}으로 회복되었습니다");
            Thread.Sleep(1500);
        }

        public void deadheal()
        {
            _hp = 1;
        }

        public void PlayerMove()
        {
            ConsoleKeyInfo inputkey = Console.ReadKey(true); // 키 입력을 콘솔에 표시

            switch (inputkey.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (_board.board[PosY, PosX - 1] != Map.BoardType.WALL)
                        PosX -= 1;
                    break;
                case ConsoleKey.RightArrow:
                    if (_board.board[PosY, PosX + 1] != Map.BoardType.WALL)
                        PosX += 1;
                    break;
                case ConsoleKey.UpArrow:
                    if (_board.board[PosY - 1, PosX] != Map.BoardType.WALL)
                        PosY -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    if (_board.board[PosY + 1, PosX] != Map.BoardType.WALL)
                        PosY += 1;
                    break;
            }
        }


        public void State()
        {
            Console.SetCursorPosition(55, 2);
            Console.WriteLine($"------------------ {_name} 스탯 ------------------");
            Console.SetCursorPosition(55, 3);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 4);
            Console.WriteLine($"|\t체력 : {_hp}/{_maxhp} \t\t 공격력 : {_atk}\t|");
            Console.SetCursorPosition(55, 5);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 6);
            Console.WriteLine($"|\texp : {_exp} / Lv: {_lv}\t 돈 : {_money}원");
            Console.SetCursorPosition(104, 6); Console.WriteLine("|");
            Console.SetCursorPosition(55, 7);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 8);
            Console.WriteLine("-------------------------------------------------");
        } 
    }
}
