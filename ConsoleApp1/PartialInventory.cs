using ConsoleApp1;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

public partial class GameManager
{
    private void InventoryMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 인벤토리 ■");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        Console.WriteLine();

        Console.WriteLine("[장비 아이템]");
        Console.WriteLine();
        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].PrintItemStatDescription();
        }

        Console.WriteLine("[소비 아이템]");
        Console.WriteLine();
        for (int i = 0; i < potioninventory.Count; i++)
        {
            potioninventory[i].PrintPotionStatDescription(); //나가기가 0번이라서 +1해줘서 띄워줌
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("2. 아이템 사용");
        Console.WriteLine();

        switch (ConsoleUtility.PromotMenuChoice(0, 2))
        {
            case 0:
                MainMenu();
                break;
            case 1:
                EquipMenu();
                break;
            case 2:
                ItemMenu();
                break;
        }
    }

    private void ItemMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 인벤토리 - 아이템 사용 ■");
        Console.WriteLine("보유 중인 아이템을 사용할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < potioninventory.Count; i++)
        {
            potioninventory[i].PrintPotionStatDescription(true, i + 1, true); //나가기가 0번이라서 +1해줘서 띄워줌
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int keyInput = ConsoleUtility.PromotMenuChoice(0, potioninventory.Count);

        switch (keyInput)
        {
            case 0:
                InventoryMenu();
                break;
            default:
                if (potioninventory[keyInput - 1].Count > 0) //포션이 1개 이상 있을때
                {
                    potioninventory[keyInput - 1].ToggleusedStates(); //개수 줄여주고
                    potioninventory[keyInput - 1].ApplyEffect(player); //효과 사용
                    if (potioninventory[keyInput - 1].Count == 0) //개수가 0일때
                    {
                        potioninventory.RemoveAt(keyInput - 1); //해당 포션을 리스트에서 제거
                    }
                }
                else //사용되지 않지만 예외처리로 남겨둠
                {
                    Console.WriteLine("사용 가능한 물약이 없습니다.");
                }
                ItemMenu();
                break;
        }
    }

    private void EquipMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 인벤토리 - 장착 관리 ■");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].PrintItemStatDescription(true, i + 1); //나가기가 0번이라서 +1해줘서 띄워줌
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int keyInput = ConsoleUtility.PromotMenuChoice(0, inventory.Count);

        switch (keyInput)
        {
            case 0:
                InventoryMenu();
                break;
            default:
                inventory[keyInput - 1].ToggleEquipStates(inventory, player, keyInput, quest);
                EquipMenu();
                break;
        }
    }

    private void DungeonItemMenu(int random)
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 인벤토리 - 아이템 사용 ■");
        Console.WriteLine("보유 중인 아이템을 사용할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < potioninventory.Count; i++)
        {
            potioninventory[i].PrintPotionStatDescription(true, i + 1, true); //나가기가 0번이라서 +1해줘서 띄워줌
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int keyInput = ConsoleUtility.PromotMenuChoice(0, potioninventory.Count);

        switch (keyInput)
        {
            case 0:
                StartBattle(random);
                break;
            default:
                if (potioninventory[keyInput - 1].Count > 0) //포션이 1개 이상 있을때
                {
                    potioninventory[keyInput - 1].ToggleusedStates(); //개수 줄여주고
                    potioninventory[keyInput - 1].ApplyEffect(player); //효과 사용
                    if (potioninventory[keyInput - 1].Count == 0) //개수가 0일때
                    {
                        potioninventory.RemoveAt(keyInput - 1); //해당 포션을 리스트에서 제거
                    }
                }
                else //사용되지 않지만 예외처리로 남겨둠
                {
                    Console.WriteLine("사용 가능한 물약이 없습니다.");
                }
                StartBattle(random);
                break;
        }
    }
}
