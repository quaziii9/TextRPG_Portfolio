using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG_Portfolio;

namespace TextRPG_Portfolio.Unit.Monster
{
    internal class Ork : uMonster
    {
        public Ork()
        {
            _maxhp = 50;
            _hp = 50;
            _atk = 50;
            _money = 10;
            _exp = 15;
            _lv = 5;
            _name = "오크";
        }
    }
}
