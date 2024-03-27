using System.Linq.Expressions;
using System.Xml.Linq;
using TextRPG_Portfolio;
using TextRPG_Portfolio.Unit.Monster;
using static System.Formats.Asn1.AsnWriter;
using TextRPG_Portfolio.Unit;
using TextRPG_Portfolio.Item;


namespace TextRPG_Portfolio
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Game game = new Game();
            TownMap townMap = new TownMap();
            uPlayer player = new uPlayer();
            player.Initialize(7, 7, townMap);
            townMap.Initialize();

            Store store = new Store(ItemType.None);
            PlayerItemList playerInven = new PlayerItemList(ItemType.None);
            store.SetStoreItem();

            while (true)
            {
                game.playing(player, townMap, store, playerInven);
            }
        }
    }
}
