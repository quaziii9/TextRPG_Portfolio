using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TextRPG_Portfolio.Unit;

namespace TextRPG_Portfolio.Item
{
    internal class PlayerItemList : Item
    {
        public List<Item> playerItem = new List<Item>();
        public bool _CheckMoney;

        uPlayer player;
        List<Item> PrintPlayerItems = new List<Item> { };

        public PlayerItemList(ItemType type) : base(type)
        {
        }

        // 상점내에서 구매했을시 
        public void PrintPlayerItem()
        {
            PrintPlayerItems = playerItem;
            PrintPlayerItems = PrintPlayerItems.Distinct().ToList();
            int count = 0;
            foreach (Item item in PrintPlayerItems)
            {
                Console.Write(count + 1 + " ");
                Console.Write(item.GetName() + " : ");
                if (item.GetEquipType() == EquipType.Atk) Console.Write("공격력 : " + item.GetAtk());
                else Console.Write("체력 : " + item.GetHp());
                Console.WriteLine();
                count++;
            }
            Console.WriteLine("=====================================");
        }

        // 장비 장착 과정
        public void PrintEquipItem(uPlayer player, PlayerItemList playerInven)
        {
            bool equip = false;
            PrintPlayerItems = playerItem;

            PrintPlayerItems = PrintPlayerItems.Distinct().ToList();

            int count = 0;

            foreach (Item item in PrintPlayerItems)
            {
                if (item._equip == false && (item.GetName() == "가시갑옷" || item.GetName() == "소드"))
                {
                    equip = true;
                }
            }
            if (equip == true)
            {
                Console.WriteLine("입을 장비의 이름을 입력해주세요");
                foreach (Item item in PrintPlayerItems)
                {
                    if (item._equip == false && (item.GetName() == "가시갑옷" || item.GetName() == "소드"))
                    {
                        Console.Write(count + 1 + " ");
                        Console.Write(item.GetName() + " : ");
                        if (item.GetName() == "소드") Console.Write("공격력 : " + item.GetAtk());
                        else Console.Write("체력 : " + item.GetHp());
                        Console.WriteLine();
                        count++;
                    }
                }
                string EquipItemname = Console.ReadLine();

                playerInven.Equip(EquipItemname, player);
            }
            else if (equip == false || PrintPlayerItems.Count == 0)
            {
                Console.WriteLine("입을 장비가 없습니다");
                Thread.Sleep(2000);
            }
        }

        // 아이템 사용 과정
        public void PrintPotionItem(uPlayer player, PlayerItemList playerInven)
        {
            bool use = false;
            PrintPlayerItems = playerItem;

            PrintPlayerItems = PrintPlayerItems.Distinct().ToList();

            int count = 0;

            foreach (Item item in PrintPlayerItems)
            {
                if (item._equip == false && (item.GetName() == "빨간포션" || item.GetName() == "주황포션"))
                {
                    use = true;
                }
            }
            if (use == true)
            {
                Console.WriteLine("사용할 포션이름을 입력해주세요");
                foreach (Item item in PrintPlayerItems)
                {
                    if (item._equip == false && (item.GetName() == "빨간포션" || item.GetName() == "주황포션"))
                    {
                        Console.Write(count + 1 + " ");
                        Console.Write(item.GetName() + " : ");
                        Console.Write("체력 : " + item.GetHp());
                        Console.WriteLine();
                        count++;
                    }
                }
                string EquipItemname = Console.ReadLine();

                playerInven.UsePotion(EquipItemname, player);
            }
            else if (use == false || PrintPlayerItems.Count == 0)
            {
                Console.WriteLine("사용할 포션이 없습니다");
                Thread.Sleep(2000);
            }
        }

        // 내가 가지고 있는 아이템 갯수 확인
        public void ItemCount()
        {
            foreach (Item item in playerItem)
            {
                item._itemListCount = 0; // 한번씩 다 초기화
                
            }
            foreach (Item item in playerItem)
            {
                item._itemListCount++; 
            }
        }

        // 아이템 구매 
        public void BuyItem(string buyName, uPlayer player, List<Item> storeItem)
        {
            foreach (Item item in storeItem)
            {
                if (item.GetName() == buyName)
                {
                    if (item.GetMoney() > player.GetMoney())
                    {
                        _CheckMoney = false;
                        Console.WriteLine("금액이 부족하여 구매가 불가능합니다");
                        break;
                    }
                    else
                    {
                        int itemMoney = item.GetMoney();
                        player.useMoney(itemMoney);
                        _CheckMoney = true;
                        playerItem.Add(item);
                    }
                    break;
                }
            }
        }

        // 인벤토리 
        public void PrintPlayerInven(uPlayer player)
        {
            string swordItem = " ";
            string armorItem = " ";
            string smallPotion = " ";
            string bigPotion = " ";
            PrintPlayerItems = playerItem.Distinct().ToList(); // 중복값 제거

            foreach (Item item in PrintPlayerItems)
            {
                if (item.GetName() == "소드")
                {
                    if (item.IsEquipped() == true)
                        swordItem = (string)(item.GetName() + "(atk+" + item.GetAtk() + ")" + " x" + item.ItemCount() + "장착");
                    else
                        swordItem = (string)(item.GetName() + "(atk+" + item.GetAtk() + ")" + " x" + item.ItemCount());
                }
                if (item.GetName() == "가시갑옷")
                {
                    if (item.IsEquipped() == true)
                        armorItem = (string)(item.GetName() + "(maxhp+" + item.GetHp() + ")" + " x" + item.ItemCount() + "장착");
                    else
                        armorItem = (string)(item.GetName() + "(maxhp+" + item.GetHp() + ")" + " x" + item.ItemCount());
                }
                if (item.GetName() == "빨간포션")
                {
                    smallPotion = (string)(item.GetName() + "(hp+" + item.GetHp() + ")" + " x" + item.ItemCount());
                }
                if (item.GetName() == "주황포션")
                {
                    bigPotion = (string)(item.GetName() + "(hp+" + item.GetHp() + ")" + " x" + item.ItemCount());
                }
            }


            Console.SetCursorPosition(55, 9);
            Console.WriteLine($"------------------ {player.GetName()} 인벤 ------------------");
            Console.SetCursorPosition(55, 10);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 11);
            Console.WriteLine("|\t무기 : " + swordItem);
            Console.SetCursorPosition(104, 11); Console.WriteLine("|");
            Console.SetCursorPosition(55, 12);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 13);
            Console.WriteLine("|\t방어구 : " + armorItem);
            Console.SetCursorPosition(104, 13); Console.WriteLine("|");
            Console.SetCursorPosition(55, 14);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 15);
            Console.WriteLine("| 포션 : " + smallPotion + "   " + bigPotion);
            Console.SetCursorPosition(104, 15); Console.WriteLine("|");
            Console.SetCursorPosition(55, 16);
            Console.WriteLine("|                                                |");
            Console.SetCursorPosition(55, 17);
            Console.WriteLine("-------------------------------------------------");
        }

        // 장비 장착시
        public void Equip(string Equip, uPlayer player)
        {
            foreach (Item item in playerItem)
            {
                if (item._equip == false && item._name == Equip && item.GetItemType() == ItemType.Equip)
                {
                    if (item.GetEquipType() == EquipType.Atk)
                    {
                        int plusAtk = item._atk;
                        item._equip = true;
                        player.EquipWeapon(plusAtk);
                        Console.WriteLine($"{item._name} 장착");
                        Thread.Sleep(1000);
                    }
                    else if (item.GetEquipType() == EquipType.Hp)
                    {
                        int plusHp = item._hp;
                        item._equip = true;
                        player.EquipArmor(plusHp);
                        Console.WriteLine($"{item._name} 장착");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        // 포션 사용시
        public void UsePotion(string Use, uPlayer player)
        {
            foreach (Item item in playerItem)
            {
                if (item.GetName() == Use && item.GetItemType() == ItemType.Potion)
                {
                    int plusHp = item._hp;
                    player.UsePotion(plusHp);
                    playerItem.Remove(item);
                    item.removeItemCount();
                    Console.WriteLine($"{item._name} 사용 {item._hp}만큼 체력이 회복되었습니다");
                    Thread.Sleep(1000);
                    break;
                }
            }
        }







        public void PrintfEquipItem(uPlayer player, PlayerItemList playerInven)
        {
            bool equip = true;
            PrintPlayerItems = playerItem;

            PrintPlayerItems = PrintPlayerItems.Distinct().ToList();

            int count = 0;

            foreach (Item item in PrintPlayerItems)
            {
                if (item._equip == true && (item.GetName() == "가시갑옷" || item.GetName() == "소드"))
                {
                    equip = false;
                }
            }
            if (equip == false)
            {
                Console.WriteLine("벗을 장비의 이름을 입력해주세요");
                foreach (Item item in PrintPlayerItems)
                {
                    if (item._equip == true && (item.GetName() == "가시갑옷" || item.GetName() == "소드"))
                    {
                        Console.Write(count + 1 + " ");
                        Console.Write(item.GetName() + " : ");
                        if (item.GetName() == "소드") Console.Write("공격력 : " + item.GetAtk());
                        else Console.Write("체력 : " + item.GetHp());
                        Console.WriteLine();
                        count++;
                    }
                }
                string EquipItemname = Console.ReadLine();

                playerInven.fEquip(EquipItemname, player);
            }
            else if (equip == true || PrintPlayerItems.Count == 0)
            {
                Console.WriteLine("벗을 장비가 없습니다");
                Thread.Sleep(2000);
            }
        }


        public void fEquip(string Equip, uPlayer player)
        {
            foreach (Item item in playerItem)
            {
                if (item._equip == true && item._name == Equip && item.GetItemType() == ItemType.Equip)
                {
                    if (item.GetEquipType() == EquipType.Atk)
                    {
                        int plusAtk = item._atk;
                        item._equip = false;
                        player.EquipWeapon(-plusAtk);
                        Console.WriteLine($"{item._name} 장착해제");
                        Thread.Sleep(1000);
                    }
                    else if (item.GetEquipType() == EquipType.Hp)
                    {
                        int plusHp = item._hp;
                        item._equip = false;
                        player.EquipArmor(-plusHp);
                        if (player.GetHp() <= 0) player.deadheal();
                        Console.WriteLine($"{item._name} 장착해제");
                        Thread.Sleep(1000);
                    }
                }
            }
        }
    }
}
