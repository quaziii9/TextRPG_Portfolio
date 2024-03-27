using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TextRPG_Portfolio.Unit;

namespace TextRPG_Portfolio.Item
{
    internal class Store : Item
    {
        public List<Item> storeItem = new List<Item>();

        List<Item> PrintStoreItems = new List<Item> { };

        public Store(ItemType type) : base(type)
        {
            
        }

        // 게임 실행시 아이템 설정된 값을 넣어줌
        public void SetStoreItem()
        {
            Equip sword = new Weapon();
            Equip armor = new Armor();
            Potion redPotion = new RedPotion();
            Potion orangePotion = new OrangePotion();
            storeItem.Add(sword);
            storeItem.Add(armor);
            storeItem.Add(redPotion);
            storeItem.Add(redPotion);
            //storeItem.Add(redPotion);
            //storeItem.Add(orangePotion);
            storeItem.Add(orangePotion);

        }

        // 상점에서 가지고 있는 아이템 프린트
        public void PrintItem()
        {
            PrintStoreItems = storeItem.Distinct().ToList(); // 중복값 제거
            int count = 0;
            foreach (Item item in PrintStoreItems)
            {
                Console.Write(count + 1 + " ");
                Console.Write(item.GetName() + " : ");

                if (item.GetEquipType() == EquipType.Atk) Console.Write("공격력 : " + item.GetAtk() + " , ");
                else Console.Write("체력 : " + item.GetHp() + " , ");

                Console.Write("금액 : " + item.GetMoney());
                Console.WriteLine();
                count++;
            }
        }

        // 상점 진행 
        public void StorePlaying(Store store, uPlayer player, PlayerItemList playerItem, GameMode mode)
        {
            bool playing = true;

            while (playing)
            {
                Console.WriteLine("상점에 오신걸 환영 합니다 원하시는 물건의 이름을 입력해주세요(상점 나가기는 0)");
                Console.WriteLine($"{player.GetName()}의 소지금 {player.GetMoney()}");
                Console.WriteLine();
                store.PrintItem();
                string buyItem = Console.ReadLine();
                bool buy = false;

                if (buyItem =="0")
                {
                    playing = false;
                    break;
                }

                foreach (Item item in PrintStoreItems)
                {
                    if (item.GetName() == buyItem)
                    {
                        buy = true;
                    }
                }
                if (buy)
                {
                    Buy(store, player, playerItem, buyItem);
                }
                else
                {
                    Console.WriteLine("다시 입력주세요");
                    Thread.Sleep(800);
                }

                if (store.storeItem.Count == 0)
                {
                    StoreListEmtpy(store, mode);
                    playing = false;
                }
                Console.Clear();
            }
        }

        // 상점에서 아이템 삭제
        public void StoreRemoveItem(string removeName)
        {
            foreach (Item item in storeItem)
            {
                if (item.GetName() == removeName)
                {
                    Console.WriteLine($"{removeName}을 구매하셨습니다. ");
                    storeItem.Remove(item);
                    break;
                }
            }
        }

        // 상품이 구매되었을때 
        public void Buy(Store store, uPlayer player, PlayerItemList playerItem, string buyItem)
        {
            player.GetMoney();
            playerItem.BuyItem(buyItem, player, storeItem);
            playerItem.ItemCount();

            if (playerItem._CheckMoney == true) store.StoreRemoveItem(buyItem);

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"{player.GetName()}의 아이템 목록 ");

            playerItem.PrintPlayerItem();

            Thread.Sleep(800);
        }

        // 상점에서 물건을 다 구매했을 때
        public void StoreListEmtpy(Store store, GameMode mode)
        {
            {
                //Console.Clear();
                //Console.WriteLine("물건을 다 구매하시다니 당신은 부자!");
                //Thread.Sleep(1000);

                while (true)
                {
                    Console.Clear();                   
                    Console.WriteLine("더 이상 아이템이 남아 있지 않습니다 상점을 나가려면 0을 입력해주세요");
                    string outStore = Console.ReadLine();
                    if (outStore == "0")
                    {
                        mode = GameMode.Town;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("물건은 이제 안팔거야 부자다 부자!");
                        Thread.Sleep(1000);
                    }

                }
            }
        }
    }
}
