using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Portfolio.Item
{
    internal class Equip : Item
    {
        public Equip(ItemType itemType, EquipType equipType) : base(itemType)
        {
            base._etype = equipType;
        }
    }

    class Weapon : Equip
    {
        public Weapon() : base(ItemType.Equip, EquipType.Atk)
        {
            _name = "소드";
            _atk = 15;
            _hp = 0;
            _money = 15;
            _equip = false;
            _itemListCount = 0;
        }
    }

    class Armor : Equip
    {
        public Armor() : base(ItemType.Equip,EquipType.Hp)
        {
            _name = "가시갑옷";
            _atk = 0;
            _hp = 20;
            _money = 20;
            _equip = false;
            _itemListCount = 0;
        }
    }
}
