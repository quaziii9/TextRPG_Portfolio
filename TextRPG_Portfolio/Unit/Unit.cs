using System;

namespace TextRPG_Portfolio.Unit
{
    internal class Pos
    {
        public Pos(int y, int x) { Y = y; X = x; }
        public int Y;
        public int X;
    }

    class Unit
    {
        protected Map _board;

        public int PosY { get; set; }
        public int PosX { get; set; }

       
        protected int _maxhp;
        protected int _hp;
        protected int _atk;
        protected int _exp;
        protected int _money;
        protected int _lv;
        protected string _name;

        public int GetMaxHp() { return _maxhp; }
        public int GetHp() { return _hp; }
        public int GetAttack() { return _atk; }
        public int GetExp() { return _exp; }
        public int GetMoney() { return _money; }
        public string GetName() { return _name; }

        public bool isDead() { return _hp <= 0; }

        public void onDamaged(int damage)
        {
            _hp -= damage;
            if (_hp <= 0) _hp = 0;
        }

        public void Initialize(int posY, int posX, Map board)
        {
            PosY = posY;
            PosX = posX;

            _board = board;
        }

        public void SetPos(int PlayerY, int PlayerX)
        {
            PosY = PlayerY;
            PosX = PlayerX;
        }
    }
}