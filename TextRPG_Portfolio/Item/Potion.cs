using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Portfolio.Item
{

    class Potion : Item
    {
        public Potion(ItemType itemType, PotionType potionType) : base(itemType)
        {
           base._ptype = potionType;
        }   
    }

    class RedPotion : Potion
    {
        public RedPotion() : base(ItemType.Potion, PotionType.Red)
        {
            _name = "빨간포션";
            _atk = 0;
            _hp = 30;
            _money = 5;
            _itemListCount = 0;
        }
    }

    class OrangePotion : Potion
    {
        public OrangePotion() : base(ItemType.Potion, PotionType.Orange)
        {
            _name = "주황포션";
            _atk = 0;
            _hp = 50;
            _money = 10;
            _itemListCount = 0;
        }

    }
}
