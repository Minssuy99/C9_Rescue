using ConsoleApp1;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

public partial class GameManager
{
    private void StoreMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 장비 상점 ■");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < storeInventory.Count; i++)
        {
            storeInventory[i].PrintStoreItemDescription();
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine("1. 아이템 구매");
        Console.WriteLine("2. 아이템 판매");
        Console.WriteLine();

        switch (ConsoleUtility.PromotMenuChoice(0, 2))
        {
            case 0:
                player.Items = inventory;
                MainMenu();
                break;
            case 1:
                PurchaseMenu();
                break;
            case 2:
                SellMenu();
                break;
        }
    }

    private void SellMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 장비 판매 ■");
        Console.WriteLine("보유중인 아이템을 판매 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].PrintStoreItemDescription(true, true, i + 1);
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int keyInput = ConsoleUtility.PromotMenuChoice(0, inventory.Count);

        switch (keyInput)
        {
            case 0:
                StoreMenu();
                break;
            default:
                player.Gold += (int)(inventory[keyInput - 1].Price * 0.8); //돈이 올라감
                inventory[keyInput - 1].TogglePurchase(); //선택한 아이템을 보유중에서 판매중으로 바꿈
                inventory.Remove(inventory[keyInput - 1]); //인벤토리에서 그 아이템을 삭제함
                SellMenu();
                break;
        }
    }

    private void PurchaseMenu(string? prompt = null)
    {
        if (prompt != null)
        {
            Console.Clear();
            ConsoleUtility.ShowTitle(prompt);
            Thread.Sleep(1000); //1000=1초 멈추기
        }

        Console.Clear();

        ConsoleUtility.ShowTitle("■ 장비 구매 ■");
        Console.WriteLine("필요한 아이템을 구매 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < storeInventory.Count; i++)
        {
            storeInventory[i].PrintStoreItemDescription(false, true, i + 1);
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int keyInput = ConsoleUtility.PromotMenuChoice(0, storeInventory.Count);

        switch (keyInput)
        {
            case 0:
                StoreMenu();
                break;
            default:
                if (storeInventory[keyInput - 1].IsPurchased)
                {
                    PurchaseMenu("이미 구매한 아이템입니다.");
                }
                else if (player.Gold >= storeInventory[keyInput - 1].Price)
                {
                    player.Gold -= storeInventory[keyInput - 1].Price;
                    storeInventory[keyInput - 1].TogglePurchase();
                    inventory.Add(storeInventory[keyInput - 1].CloneItem());
                    PurchaseMenu();
                }
                else
                {
                    PurchaseMenu("Gold가 부족합니다.");
                }
                break;
        }
    }

    private void PotionStoreMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 물약 상점 ■");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < potion.Count; i++)
        {
            potion[i].PrintStorePotionDescription();
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine("1. 물약 구매");
        Console.WriteLine("2. 포션 판매");
        Console.WriteLine();

        switch (ConsoleUtility.PromotMenuChoice(0, 2))
        {
            case 0:
                MainMenu();
                break;
            case 1:
                PotionPurchaseMenu();
                break;
            case 2:
                PotionSellMenu();
                break;
        }
    }

    private void PotionPurchaseMenu(string? prompt = null)
    {
        if (prompt != null)
        {
            Console.Clear();
            ConsoleUtility.ShowTitle(prompt);
            Thread.Sleep(1000); //1000=1초 멈추기
        }
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 물약 구매 ■");
        Console.WriteLine("필요한 아이템을 구매 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < potion.Count; i++)
        {
            potion[i].PrintStorePotionDescription(true, i + 1);
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int keyInput = ConsoleUtility.PromotMenuChoice(0, storeInventory.Count);

        switch (keyInput)
        {
            case 0:
                PotionStoreMenu();
                break;
            default:
                if (player.Gold >= potion[keyInput - 1].Price) //사는게 가능할때
                {
                    player.Gold -= potion[keyInput - 1].Price; //돈 빼주고
                    string potionName = potion[keyInput - 1].Name; //상점에서 선택한 포션의 이름
                    if (!potioninventory.Any(p => p.Name == potionName)) //포션이 인벤토리에 없다면
                    {
                        Potion newPotion = potion[keyInput - 1].ClonePotion(); //복제해서
                        newPotion.TogglePurchase(); //복제한걸 증가시켜줌
                        potioninventory.Add(newPotion); //증가시킨 포션을 추가
                    }
                    else //이미 있는 포션이라면
                    {
                        potioninventory.First(p => p.Name == potionName).TogglePurchase(); //제일 첫번째 있는 이름같은 포션의 개수 증가
                    }
                    PotionPurchaseMenu();
                }
                else
                {
                    PotionPurchaseMenu("Gold가 부족합니다.");
                }
                break;
        }
    }

    private void PotionSellMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 물약 판매 ■");
        Console.WriteLine("보유중인 아이템을 판매 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < potioninventory.Count; i++)
        {
            potioninventory[i].PrintPotionStatDescription(true, true, i + 1);
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int keyInput = ConsoleUtility.PromotMenuChoice(0, potioninventory.Count);

        switch (keyInput)
        {
            case 0:
                PotionStoreMenu();
                break;
            default:
                player.Gold += (int)(potioninventory[keyInput - 1].Price * 0.8); //돈이 올라감
                potioninventory[keyInput - 1].DecreaseCount(); //선택한 아이템의 갯수를 줄여줌
                if (potioninventory[keyInput - 1].Count <= 0)
                    potioninventory.Remove(potioninventory[keyInput - 1]); //인벤토리에서 그 아이템을 삭제함
                PotionSellMenu();
                break;
        }
    }
}