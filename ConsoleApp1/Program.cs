






using System;
using System.Data.SqlTypes;

public class GameManager
{
    private Player player;
    private List<Item> inventory;

    private List<Item> storeInventory;

    private List<Monster> monsters;

    private bool turn = true;

    public GameManager()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        player = new Player("Seungjun", "Programmer", 1, 10, 5, 100, 15000);

        inventory = new List<Item>();
        
        storeInventory = new List<Item>();
        storeInventory.Add(new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", ItemType.ARMOR, 0, 5, 0, 500));
        storeInventory.Add(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", ItemType.ARMOR, 0, 9, 0, 2000));
        storeInventory.Add(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", ItemType.ARMOR, 0, 15, 0, 3500));
        storeInventory.Add(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", ItemType.WEAPON, 2, 0, 0, 600));
        storeInventory.Add(new Item("청동 도끼", "어디선가 사용됐던 것 같은 도끼입니다.", ItemType.WEAPON, 5, 0, 0, 1500));
        storeInventory.Add(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", ItemType.WEAPON, 7, 0, 0, 2500));
    }

    public void StartGame()
    {
        Console.Clear();
        ConsoleUtility.PrintGameHeader();
        MainManu();
    }

    private void MainManu()
    {


        Console.Clear();
        Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■");
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
        Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■");
        Console.WriteLine();

        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전입장");
        Console.WriteLine();

        int choice = ConsoleUtility.PromotMenuChoice(1, 4);

        switch (choice)
        {
            case 1:
                StatusMenu(); //상태보기
                break;
            case 2:
                InventoryMenu(); //인벤토리
                break;
            case 3:
                StoreMenu(); //상점
                break;
            case 4:
                DungeonMenu();
                break;
        }
        MainManu(); // 혹시나 몰라서 받아주는 부분
    }

    private void DungeonMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 던전입구 ■");
        Console.WriteLine("가고 싶은 던전을 선택해주세요..");
        Console.WriteLine();
        Console.WriteLine("★☆☆");
        Console.WriteLine("1. 깊은 숲");

        Console.WriteLine();
        Console.WriteLine("★★☆");
        Console.WriteLine("2. 설산");

        Console.WriteLine();
        Console.WriteLine("★★★");
        Console.WriteLine("3. 용암동굴");

        Console.WriteLine();
        Console.WriteLine("0. 뒤로가기");
        Console.WriteLine();

        int choice = ConsoleUtility.PromotMenuChoice(0, 3);

        switch (choice)
        {
            case 0:
                MainManu();
                break;
            case 1:
                FightStartMenu();
                break;
            case 2:
                if (player.Def > 10)
                {
                    player.Gold += 1700;
                    Console.WriteLine("설산에 입장하였습니다.");
                    Console.WriteLine("보상으로 1700골드를 획득하였습니다.");
                }
                else
                {
                    Console.WriteLine("방어력이 부족하여 설산에 입장할 수 없습니다.");
                }
                break;
            case 3:
                if (player.Def > 15)
                {
                    player.Gold += 2500;
                    Console.WriteLine("용암동굴에 입장하였습니다.");
                    Console.WriteLine("보상으로 2500골드를 획득하였습니다.");
                }
                else
                {
                    Console.WriteLine("방어력이 부족하여 용암동굴에 입장할 수 없습니다.");
                }
                break;
        }
        MainManu();
    }

    private void FightStartMenu()
    {
        Console.Clear();
        Random random = new Random();
        int Mnum = random.Next(1, 5); //1~4마리의 랜덤 몬스터;
        Monster[] TempMonster = new Monster[Mnum];
        for (int i = 0; i < Mnum; i++)
        {
            TempMonster[i] = new Monster(random.Next(0, 3)); //임시 저장
        }

        switch (FightMenu(false, random, Mnum, TempMonster))
        {
            case 1:
                BattleMenu(random, Mnum, TempMonster);
                break;
        }
    }
    int MonsterAtkNum = 0; //몬스터의 공격순서를 위한 변수
    private void BattleMenu(Random random, int Mnum, Monster[] TempMonster)
    {
        int choice = FightMenu(true, random, Mnum, TempMonster); //FightMenu를 출력하면서 choice의 값을 받아온다.
        switch (choice)
        {
            case 0:
                TempMonster[MonsterAtkNum%(Mnum-1)].MonsterPhase();
                MonsterAtkNum++;
                BattleMenu(random, Mnum, TempMonster);
                break;
            default:
                player.PlayerPhase(TempMonster[choice - 1]);
                switch (ConsoleUtility.PromotMenuChoice(0, 0))
                {
                    case 0:
                        TempMonster[MonsterAtkNum % (Mnum - 1)].MonsterPhase();
                        MonsterAtkNum++;
                        BattleMenu(random, Mnum, TempMonster);
                        break;
                }
                break;
        }
    }

    private int FightMenu(bool fightCheck, Random random, int Mnum, Monster[] TempMonster)
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights("", "Battle!");
        Console.WriteLine("\n\n");
        for (int i = 0; i < Mnum; i++)
        {
            TempMonster[i].PrintMonsterInfo(fightCheck, i + 1);
        }
        Console.WriteLine("\n\n\n");
        Console.WriteLine("[내정보]");
        ConsoleUtility.PrintTextHighlights("Lv", player.Level.ToString(), $" {player.Name} ({player.Job})");
        Console.WriteLine();
        ConsoleUtility.PrintTextHighlights("HP ", $"{player.Hp}/100");
        Console.WriteLine("\n\n");
        if (fightCheck)
        {
            ConsoleUtility.PrintTextHighlights("", "0. ", "취소");
            Console.WriteLine("\n");
            return ConsoleUtility.PromotMenuChoice(0, Mnum, TempMonster);
        }
        else
        {
            ConsoleUtility.PrintTextHighlights("", "1. ", "공격");
            Console.WriteLine("\n");
            return ConsoleUtility.PromotMenuChoice(1, 1);
        }

    }

    private void StatusMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 상태보기 ■");
        Console.WriteLine("캐릭터의 정보가 표기됩니다.");

        ConsoleUtility.PrintTextHighlights("Lv. ", player.Level.ToString("00")); //00은 두글자 제한
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"{player.Name}({player.Job})");

        //능력치 강화된 부분 추가하기
        int bonusAtk = inventory.Select(item => item.IsEquipped ? item.Atk : 0).Sum();
        int bonusDef = inventory.Select(item => item.IsEquipped ? item.Def : 0).Sum();
        int bonusHp = inventory.Select(item => item.IsEquipped ? item.Hp : 0).Sum();


        ConsoleUtility.PrintTextHighlights("공격력 :", (player.Atk + bonusAtk).ToString(), bonusAtk > 0 ? $"(+{bonusAtk})" : "");
        Console.WriteLine();
        ConsoleUtility.PrintTextHighlights("방어력 :", (player.Def + bonusDef).ToString(), bonusDef > 0 ? $"(+{bonusDef})" : "");
        Console.WriteLine();
        ConsoleUtility.PrintTextHighlights("체  력 :", (player.Hp + bonusHp).ToString(), bonusHp > 0 ? $"(+{bonusHp})" : "");
        Console.WriteLine();

        ConsoleUtility.PrintTextHighlights("Gold :", player.Gold.ToString());
        Console.WriteLine();
        Console.WriteLine();

        Console.WriteLine("0. 뒤로가기");
        Console.WriteLine();


        switch (ConsoleUtility.PromotMenuChoice(0, 0))
        {
            case 0:
                MainManu();
                break;
        }
    }

    private void InventoryMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 인벤토리 ■");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].PrintItemStatDescription();
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine("1. 장착관리");
        Console.WriteLine();

        switch (ConsoleUtility.PromotMenuChoice(0, 1))
        {
            case 0:
                MainManu();
                break;
            case 1:
                EquipMenu();
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
                inventory[keyInput - 1].ToggleEquipStates(inventory);
                EquipMenu();
                break;
        }
    }

    private void StoreMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 상점 ■");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
        Console.WriteLine();
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
                MainManu();
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

        ConsoleUtility.ShowTitle("■ 아이템 판매 ■");
        Console.WriteLine("보유중인 아이템을 판매 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < inventory.Count; i++)
        {
            inventory[i].PrintStoreItemDescription(true, i + 1);
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
                player.Gold += inventory[keyInput - 1].Price; //돈이 올라감
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

        ConsoleUtility.ShowTitle("■ 아이템 구매 ■");
        Console.WriteLine("필요한 아이템을 구매 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < storeInventory.Count; i++)
        {
            storeInventory[i].PrintStoreItemDescription(true, i + 1);
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
                    inventory.Add(storeInventory[keyInput - 1]);
                    PurchaseMenu();
                }
                else
                {
                    PurchaseMenu("Gold가 부족합니다.");
                }
                break;
        }
    }
}

public class Program
{


    static void Main(string[] args)
    {
        GameManager gameManager = new GameManager();
        gameManager.StartGame();
    }
}

