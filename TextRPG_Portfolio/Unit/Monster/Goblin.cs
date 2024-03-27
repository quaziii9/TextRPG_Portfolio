using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG_Portfolio;

namespace TextRPG_Portfolio.Unit.Monster
{
    internal class Goblin : uMonster
    {
        public Goblin()
        {
            _maxhp = 30;
            _hp = 30;
            _atk = 30;
            _money = 5;
            _exp = 5;
            _lv = 1;
            _name = "고블린";
        }
    }
}
