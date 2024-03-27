using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_Portfolio.Item
{
    public enum ItemType
    {
        None,
        Equip,
        Potion
    }
    public enum EquipType
    {
        None,
        Atk,
        Hp
    }
    public enum PotionType
    {
        None,
        Red,
        Orange
    }

    class Item
    {
        protected ItemType _type;
        protected EquipType _etype;
        protected PotionType _ptype;

        public string _name;
        public int _hp;
        public int _atk;
        public int _money;
        public int _itemListCount = 0;
        public bool _equip = false;

        public Item(ItemType type)
        {
            _type = type;
        }

        public int GetHp() { return _hp; }
        public int GetAtk() { return _atk; }
        public int GetMoney() { return _money; }
        public string GetName() { return _name; }
        public bool IsEquipped() { return _equip; }
        public int ItemCount() { return _itemListCount; }
        public int removeItemCount() { return _itemListCount -= 1; } 
        public ItemType GetItemType() { return _type; }
        public EquipType GetEquipType() { return _etype; }
        public PotionType GetPotionType() {  return _ptype; }
    }
}
