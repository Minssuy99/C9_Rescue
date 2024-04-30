using ConsoleApp1;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

public class GameManager
{
    private Player player;
    private List<Item> inventory;
    private List<Item> storeInventory;
    private List<Monster> monster;
    private List<Monster> battleMonster;
    Random battleRandom = new Random();
    private List<Dungeon> dungeons;
    public GameManager()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        player = new Player("Seungjun", "Programmer", 1, 10, 5, 100, 100, 15000);

        battleMonster = new List<Monster>();

        monster = new List<Monster>();
        monster.Add(new Monster("미니언", 2, 5, 2, 15, 15));
        monster.Add(new Monster("대포미니언", 5, 9, 5, 25, 25));
        monster.Add(new Monster("공허충", 3, 8, 3, 10, 10));
        monster.Add(new Monster("칼날부리새끼", 1, 3, 1, 8, 8));
        monster.Add(new Monster("어스름 늑대", 5, 10, 5, 8, 8));

        inventory = new List<Item>();

        storeInventory = new List<Item>();
        storeInventory.Add(new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", ItemType.ARMOR, 0, 5, 0, 500));
        storeInventory.Add(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", ItemType.ARMOR, 0, 9, 0, 2000));
        storeInventory.Add(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", ItemType.ARMOR, 0, 15, 0, 3500));
        storeInventory.Add(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", ItemType.WEAPON, 2, 0, 0, 600));
        storeInventory.Add(new Item("청동 도끼", "어디선가 사용됐던 것 같은 도끼입니다.", ItemType.WEAPON, 5, 0, 0, 1500));
        storeInventory.Add(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", ItemType.WEAPON, 7, 0, 0, 2500));

        dungeons = new List<Dungeon>();
        dungeons.Add(new Dungeon(1, "초급던전", 5, 3, 1000));
        dungeons.Add(new Dungeon(2, "중급던전", 15, 5, 3000));
        dungeons.Add(new Dungeon(3, "고급던전", 25, 7, 5000));
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
        Console.WriteLine("5. 여관");
        Console.WriteLine();

        int choice = ConsoleUtility.PromotMenuChoice(1, 5);

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
                DungeonChoiceMenu();
                break;
            case 5:
                RestMenu();
                break;
        }
        MainManu(); // 혹시나 몰라서 받아주는 부분
    }

    private void RestMenu()
    {
        Console.Clear();
        Console.WriteLine("쉬고가겠나");
        ConsoleUtility.PrintTextHighlights("", "1. ", "500G를 지불하면 피를 회복합니다.");
        ConsoleUtility.PrintTextHighlights("", "0. ", "나가기");
        int choice = ConsoleUtility.PromotMenuChoice(1, 5);
        switch (choice)
        {
            case 0:
                MainManu();
                break;
            case 1:
                if (player.Gold < 500)
                {
                    Console.WriteLine("안돼 돌아가");
                    Console.ReadKey();

                }
                else
                {
                    Console.WriteLine("체력이 회복됐습니다.");
                    player.Rest();
                    Console.ReadKey();
                }
                break;
        }

    }

    private void DungeonMenu(int max)
    {
        Console.Clear();
        ConsoleUtility.ShowTitle("■ 던전입구 ■");
        Console.WriteLine("던전에 들어가면 전투가 시작됩니다.");
        Console.WriteLine();

        Console.WriteLine("2. 전투 시작");
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("0. 뒤로가기");
        Console.WriteLine();

        int choice = ConsoleUtility.PromotMenuChoice(0, 3);

        switch (choice)
        {
            case 0:
                MainManu();
                break;
            case 1:
                StatusMenu();
                break;
            case 2:
                BattleManu(max);
                break;
        }
        MainManu();
    }

    private void BattleManu(int max)
    {
        Console.Clear();
        ConsoleUtility.ShowTitle("■ Battle!! ■");
        Console.WriteLine();
        // 전투돌입할때마다 초기화
        battleMonster.Clear();
        Random rand = new();
        int random = rand.Next(1, max);
        for (int i = 0; i < random; i++)
        {
            while (true)
            {
                Monster randomMonster = GetRandomMonster();
                if (!battleMonster.Contains(randomMonster))
                {
                    battleMonster.Add(randomMonster);
                    Console.WriteLine($"Lv.{battleMonster[i].Level} {battleMonster[i].Name} HP {battleMonster[i].CurrentHp}");
                    break;
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name}({player.Job})");
        Console.WriteLine($"HP {player.CurrentHp} / {player.MaxHp}");
        Console.WriteLine();
        Console.WriteLine("1. 공격");
        Console.WriteLine();

        int choice = ConsoleUtility.PromotMenuChoice(1, 1);

        if (choice == 1)
        {
            StartBattle(random);
        }
    }

    private void StartBattle(int random)
    {
        while (true)
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("■ Battle!! ■");
            Console.WriteLine();

            for (int i = 0; i < random; i++)
            {
                if (!battleMonster[i].IsAlive())
                {
                    ConsoleUtility.PrintTextDeath("", $"[{i + 1}] Lv.{battleMonster[i].Level} {battleMonster[i].Name} HP {battleMonster[i].CurrentHp}");
                }
                else
                {
                    Console.WriteLine($"[{i + 1}] Lv.{battleMonster[i].Level} {battleMonster[i].Name} HP {battleMonster[i].CurrentHp}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Level}  {player.Name}({player.Job})");
            Console.WriteLine($"HP {player.CurrentHp} / {player.MaxHp}");
            Console.WriteLine();

            int choice = ConsoleUtility.PromotMenuChoice(1, random);

            int selectedMonsterIndex = choice - 1;

            //이미 죽은 몬스터 구분
            if (!battleMonster[selectedMonsterIndex].IsAlive())
            {
                Console.WriteLine("선택한 몬스터가 이미 죽었습니다. 다른 몬스터를 선택하세요.");
                Console.ReadKey();
                continue; // 다시 반복문의 처음으로 돌아가 다시 선택할 수 있도록 합니다.
            }

            Monster selectedMonster = battleMonster[selectedMonsterIndex];

            Console.WriteLine();
            ConsoleUtility.PrintTextHighlights("", $"{player.Name}가 공격!");

            if (battleRandom.Next(100) < 10)
            {
                Console.WriteLine("회피!");
                Console.WriteLine($"몬스터 {selectedMonster.Name}이(가) 공격을 회피했습니다.");
            }
            else
            {
                if (battleRandom.Next(100) < 15)
                {
                    // 추가 효과가 발생한 경우
                    int criticalDamage = (int)(player.Atk * 1.6); // 160%의 데미지
                    Console.WriteLine("치명타 발생! 추가 데미지를 가합니다.");
                    Console.WriteLine($"[데미지 : {criticalDamage}]");
                    selectedMonster.MonterTakeDamage(criticalDamage);
                }
                else
                {
                    // 추가 효과가 발생하지 않은 경우
                    Console.WriteLine($"[데미지 : {player.Atk}]");
                    selectedMonster.MonterTakeDamage(player.Atk);
                }
                Console.WriteLine($"몬스터 {selectedMonster.Name}의 HP: {selectedMonster.CurrentHp}");

                if (!selectedMonster.IsAlive())
                {
                    Console.WriteLine($"몬스터 {selectedMonster.Name}를 물리쳤습니다!");
                }
            }

            bool allMonstersDead = true;
            foreach (var monster in battleMonster)
            {
                if (monster.IsAlive())
                {
                    allMonstersDead = false;
                    break;
                }
            }

            //플레이어 승리
            if (allMonstersDead)
            {
                Console.WriteLine();
                Console.WriteLine("모든 몬스터를 물리쳤습니다.");
                Console.WriteLine("던전 입구로 돌아갑니다.");

                foreach (var monster in battleMonster)
                {
                    monster.Reset();
                }


                Thread.Sleep(1000);
                Console.WriteLine("아무키나 누르세요...");
                Console.ReadKey();
                DungeonChoiceMenu();
            }

            // 몬스터의 공격차례
            Thread.Sleep(1000);
            Console.WriteLine();
            Console.WriteLine("몬스터 차례");

            if (battleRandom.Next(100) < 10)
            {
                Console.WriteLine("회피!");
                Console.WriteLine($"{player.Name}이(가) 공격을 회피했습니다.");
            }
            else
            {
                foreach (Monster m in battleMonster)
                {
                    if (m.IsAlive())
                    {
                        ConsoleUtility.PrintTextHighlights("", $"{m.Name}이(가) 공격합니다!");

                        if (battleRandom.Next(100) < 15)
                        {
                            // 추가 효과가 발생한 경우
                            int criticalDamage = (int)(m.Atk * 1.6); // 160%의 데미지
                            Console.WriteLine("치명타 발생! 추가 피해를 입었습니다.");
                            Console.WriteLine($"[데미지 : {criticalDamage}]");
                            player.PlayerTakeDamage(criticalDamage);
                        }
                        else
                        {
                            Console.WriteLine($"[데미지 : {m.Atk}]");
                            player.PlayerTakeDamage(m.Atk);
                        }

                        Console.WriteLine($"{player.Name}의 HP: {player.CurrentHp}");
                        Console.WriteLine();
                        Thread.Sleep(1000);
                    }
                }
            }

            Console.WriteLine("아무키나 누르세요...");
            Console.ReadLine();

            //플레이어 패배
            if (!player.IsAlive())
            {
                Console.WriteLine("전투에서 패배했습니다.");
                Console.WriteLine("마을로 돌아갑니다.");

                foreach (var monster in battleMonster)
                {
                    monster.Reset();
                }

                player.playerdefeat();

                //골드값 0미만으로 떨어지지 않게 Player에서 조정한 후
                //골드를 일정값 빼주는 메서드 작성

                Thread.Sleep(1000);
                Console.ReadKey();
                MainManu();
            }

        }
    }

    private Monster GetRandomMonster()
    {
        Random rand = new Random();
        return monster[rand.Next(monster.Count)];
    }

    private void StatusMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 상태보기 ■");
        Console.WriteLine("캐릭터의 정보가 표기됩니다.");

        ConsoleUtility.PrintTextHighlights("Lv. ", player.Level.ToString("00")); //00은 두글자 제한
        Console.WriteLine();
        Console.WriteLine($"{player.Name}({player.Job})");

        //능력치 강화된 부분 추가하기
        int bonusAtk = inventory.Select(item => item.IsEquipped ? item.Atk : 0).Sum();
        int bonusDef = inventory.Select(item => item.IsEquipped ? item.Def : 0).Sum();
        int bonusHp = inventory.Select(item => item.IsEquipped ? item.Hp : 0).Sum();


        ConsoleUtility.PrintTextHighlights("공격력 :", (player.Atk + bonusAtk).ToString(), bonusAtk > 0 ? $"(+{bonusAtk})" : "");
        ConsoleUtility.PrintTextHighlights("방어력 :", (player.Def + bonusDef).ToString(), bonusDef > 0 ? $"(+{bonusDef})" : "");
        ConsoleUtility.PrintTextHighlights("체  력 :", $"{(player.CurrentHp + bonusHp).ToString()}/{(player.MaxHp + bonusHp)}".ToString(), bonusHp > 0 ? $"(+{bonusHp})" : "");

        ConsoleUtility.PrintTextHighlights("Gold :", player.Gold.ToString());
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

    private void DungeonChoiceMenu()
    {
        Console.Clear();
        foreach(Dungeon dungeon in dungeons) 
        {
            dungeon.PrintDungeon();
        }
        ConsoleUtility.PrintTextHighlights("", "0. ", "나가기");
        int choice = ConsoleUtility.PromotMenuChoice(0, 3);
        switch (choice){
            case 0:
                MainManu();
                break;
            default:
                DungeonMenu(dungeons[choice-1].MaxMonster);
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

