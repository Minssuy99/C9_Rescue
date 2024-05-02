using ConsoleApp1;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

public class GameManager
{
    private Player player;
    private List<Item> inventory;
    private List<Item> storeInventory;
    private List<Item> dungeonItemList;
    private List<Item> weaponReward;
    private List<Monster> monster;
    private List<Monster> battleMonster;
    Random battleRandom = new Random();
    private List<Dungeon> dungeons;
    private List<Quest> quest;
    private List<Potion> potion;
    private List<Potion> potioninventory;

    public GameManager()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        player = new Player("", "", 1, 10, 5, 100, 100, 11, 0, 15000);
        battleMonster = new List<Monster>();

        monster = new List<Monster>();
        monster.Add(new Monster("미니언", 2, 5, 2, 15, 15, 2, ""));
        monster.Add(new Monster("대포미니언", 5, 9, 5, 25, 25, 5, "대포"));
        monster.Add(new Monster("공허충", 3, 8, 3, 10, 10, 3, ""));
        monster.Add(new Monster("칼날부리새끼", 1, 3, 1, 8, 8, 1, ""));
        monster.Add(new Monster("어스름 늑대", 5, 10, 5, 8, 8, 5, ""));

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

        dungeonItemList = new List<Item>();
        dungeonItemList.Add(new Item("대포", "대포 미니언의 강력한 대포입니다.", ItemType.WEAPON, 9, 0, 0, 300));

        weaponReward = new List<Item>();

        quest = new List<Quest>();
        quest.Add(new Quest("마을을 위협하는 미니언 처치",
                            "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나?\n" +
                            "마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\n" +
                            "모험가인 자네가 좀 처치해주게!\n",
                            "미니언 5마리 처치 (0/5)",
                            "방패 x 1", 5));

        quest.Add(new Quest("장비를 장착해보자",
                            "어떤 장비를 장착하느냐에 따라 모험가의 전투력이 크게 달라질 수 있다네.\n" +
                            "어서 장비를 장착해보도록 하게!",
                            "장비 1개 장착",
                            "포션 x 3", 10));

        quest.Add(new Quest("더욱 더 강해지기!",
                            "모험가여, 당신의 모험은 아직 끝나지 않았다." +
                            "계속해서 성장하고 더욱 강해져야 한다." +
                            "자, 새로운 도전에 나서보시게!",
                            "몬스터 10마리 처치 (0/10)",
                            "도끼 x 1", 15));

        potion = new List<Potion>();
        potion.Add(new Potion("빨간 포션", "붉은 약초로 만든 물약이다", PotionEffect.Heal, 20, 100));
        potion.Add(new Potion("주황 포션", "붉은 약초의 농축 물약이다.", PotionEffect.Heal, 30, 150));
        potion.Add(new Potion("하얀 포션", "붉은 약초의 고농축 물약이다.", PotionEffect.Heal, 50, 300));

        potioninventory = new List<Potion>();
    }


    public void StartGame()
    {
        Console.Clear();
        ConsoleUtility.PrintGameHeader();
        player.PlayerCreate(player);
        MainMenu();
    }

    private void MainMenu()
    {
        Console.Clear();
        Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■");
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
        Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■");
        Console.WriteLine();

        Console.WriteLine("1. 상태보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 장비 상점");
        Console.WriteLine("4. 물약 상점");
        Console.WriteLine("5. 던전입장");
        Console.WriteLine("6. 여관");
        Console.WriteLine("7. 퀘스트");
        Console.WriteLine();

        int choice = ConsoleUtility.PromotMenuChoice(1, 6);

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
                PotionStoreMenu();
                break;
            case 5:
                DungeonChoiceMenu();
                break;
            case 6:
                RestMenu();
                break;
            case 7:
                QuestMenu();
                break;
        }
        MainMenu(); // 혹시나 몰라서 받아주는 부분
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
            potioninventory[i].PrintPotionStatDescription(true, i + 1);
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
                MainMenu();
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
        if (player.NowDongeon > 3)
        {
            Console.WriteLine("타워를 클리어하셨습니다");
            player.NowDongeon = 1;
            player.InTower = false;
            Console.ReadKey();
            MainMenu();
        }
        ConsoleUtility.ShowTitle("■ 던전입구 ■");
        Console.WriteLine("던전에 들어가면 전투가 시작됩니다.");
        Console.WriteLine();

        if (player.InTower)
        {
            Console.WriteLine($"2. 전투 시작 (현재 진행 : {player.NowDongeon})");
            Console.WriteLine("1. 상태 보기");
        }

        else
        {
            Console.WriteLine("2. 전투 시작");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("0. 뒤로가기");
        }
        
        Console.WriteLine();

        int choice = ConsoleUtility.PromotMenuChoice(0, 3);

        switch (choice)
        {
            case 0:
                MainMenu();
                break;
            case 1:
                StatusMenu();
                break;
            case 2:
                if (player.InTower)
                    BattleManu(player.NowDongeon * 2 + 1);
                else
                    BattleManu(max);
                break;
        }
        MainMenu();
    }
    private void BattleManu(int max)
    {
        Console.Clear();
        ConsoleUtility.ShowTitle("■ Battle!! ■");
        Console.WriteLine();
        // 전투돌입할때마다 초기화
        battleMonster.Clear();
        Random rand = new();
        int random = rand.Next(max - 2, max);
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
                    Console.WriteLine($"경험치 {selectedMonster.Exp}를 얻었습니다!");
                    player.GetExp(selectedMonster.Exp);
                    player.LevelUp();

                    MonsterDied(selectedMonster);

                    ConsoleUtility.PrintTextHighlights("경험치 :", $"{player.CurrentExp}/{player.MaxExp}".ToString());
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
                ShowResultMenu(random);
            }

            // 몬스터의 공격차례
            Thread.Sleep(1000);
            Console.WriteLine();
            Console.WriteLine("몬스터 차례");

            foreach (Monster m in battleMonster)
            {
                if (m.IsAlive())
                {
                    if (battleRandom.Next(100) < 10)
                    {
                        Console.WriteLine("회피!");
                        Console.WriteLine($"{player.Name}이(가) 공격을 회피했습니다.");
                    }
                    else
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
                player.InTower = false;
                player.playerdefeat();

                //골드값 0미만으로 떨어지지 않게 Player에서 조정한 후
                //골드를 일정값 빼주는 메서드 작성

                Thread.Sleep(1000);
                Console.ReadKey();
                MainMenu();
            }

        }
    }

    // 몬스터가 죽었을 때 호출되는 메서드
    private void MonsterDied(Monster selectedMonster)
    {
        // 몬스터가 가지고 있는 아이템의 인덱스를 찾기
        int itemIndex = dungeonItemList.FindIndex(item => item.Name == selectedMonster.DropItem);

        // 해당 아이템을 찾았는지 확인 후 인벤토리에 추가
        if (itemIndex != -1)
        {
            if (battleRandom.Next(100) < 10)
            {
                Item droppedItem = dungeonItemList[itemIndex].CloneItem();
                weaponReward.Add(droppedItem);
                Console.WriteLine($"{selectedMonster.DropItem}를 얻었습니다!");
            }
        }
    }

    private void ShowResultMenu(int random)
    {
        Console.WriteLine();
        Console.WriteLine("모든 몬스터를 물리쳤습니다.");

        foreach (var monster in battleMonster)
        {
            monster.Reset();
        }

        Thread.Sleep(1000);
        Console.WriteLine("아무키나 누르세요...");
        Console.ReadKey();

        Console.Clear();
        ConsoleUtility.ShowTitle("■ 전투 결과 ■");
        Console.WriteLine();

        Console.WriteLine($"던전에서 몬스터 {random}마리를 잡았습니다.");
        Console.WriteLine();
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name}({player.Job})");
        Console.WriteLine($"HP {player.CurrentHp} / {player.MaxHp}");
        Console.WriteLine();

        Console.WriteLine($"[전투 보상]");
        Console.WriteLine($"{random * 100}골드를 획득했습니다.");
        player.Gold += (random * 100);

        for (int i = 0; i < weaponReward.Count; i++)
        {
            inventory.Add(weaponReward[i]); // 아이템을 인벤토리에 추가
            Console.WriteLine($"{weaponReward[i].Name}를 얻었습니다!"); // 아이템 이름 출력
        }
        //여기에 보상 목록 추가

        weaponReward.Clear();
        Console.WriteLine();

        Thread.Sleep(1000);

        if(player.InTower)
        {
            DungeonMenu(++player.NowDongeon);
            Console.WriteLine("2층으로 갑니다.");

        }
        else
        {
            Console.WriteLine("던전 입구로 돌아갑니다.");
        }
        Console.WriteLine("아무키나 누르세요...");
        Console.ReadKey();
        DungeonChoiceMenu();
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


        ConsoleUtility.PrintTextHighlights("공격력 :", (player.Atk).ToString(), bonusAtk > 0 ? $"(+{bonusAtk})" : "");
        ConsoleUtility.PrintTextHighlights("방어력 :", (player.Def).ToString(), bonusDef > 0 ? $"(+{bonusDef})" : "");
        ConsoleUtility.PrintTextHighlights("체  력 :", $"{player.CurrentHp + bonusHp}/{(player.MaxHp + bonusHp)}".ToString(), bonusHp > 0 ? $"(+{bonusHp})" : "");

        ConsoleUtility.PrintTextHighlights("Gold :", player.Gold.ToString());
        Console.WriteLine();

        Console.WriteLine("0. 뒤로가기");
        Console.WriteLine();


        switch (ConsoleUtility.PromotMenuChoice(0, 0))
        {
            case 0:
                MainMenu();
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
        for (int i = 0; i < potioninventory.Count; i++)
        {
            potioninventory[i].PrintPotionStatDescription(); //나가기가 0번이라서 +1해줘서 띄워줌
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("2. 아이템관리");
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

        ConsoleUtility.ShowTitle("■ 인벤토리 - 아이템 관리 ■");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < potioninventory.Count; i++)
        {
            potioninventory[i].PrintPotionStatDescription(true, i + 1,true); //나가기가 0번이라서 +1해줘서 띄워줌
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
                inventory[keyInput - 1].ToggleEquipStates(inventory, player, keyInput);
                EquipMenu();
                break;
        }
    }

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
        foreach (Dungeon dungeon in dungeons)
        {
            dungeon.PrintDungeon();
        }
        ConsoleUtility.PrintTextHighlights("", "4. ", "타워던전");
        ConsoleUtility.PrintTextHighlights("", "0. ", "나가기");
        int choice = ConsoleUtility.PromotMenuChoice(0, 4);
        switch (choice)
        {
            case 0:
                player.InTower = false;
                MainMenu();
                break;
            case 4:
                player.InTower = true;
                DungeonMenu(player.NowDongeon);
                break;
            default:
                DungeonMenu(dungeons[choice - 1].MaxMonster);
                break;
        }
    }

    private void QuestMenu()
    {
        Console.Clear();

        ConsoleUtility.ShowTitle("■ 퀘스트 ■");
        Console.WriteLine("퀘스트를 확인하고 선택할 수 있습니다.");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[ 퀘스트 목록 ]");
        Console.ResetColor();
        Console.WriteLine();
        for (int i = 0; i < quest.Count; i++)
        {
            Console.WriteLine($"{ i + 1 }. {quest[i].Title}");
        }

        Console.WriteLine();
        Console.WriteLine("0. 뒤로가기");
        Console.WriteLine();


        switch (ConsoleUtility.PromotMenuChoice(0, 3))
        {
            case 0:
                MainMenu();
                break;
            case 1:
                PrintQuestDetails(0);
                break;
            case 2:
                PrintQuestDetails(1);
                break;
            case 3:
                PrintQuestDetails(2);
                break;
        }
        MainMenu();
    }

    private void PrintQuestDetails(int idx)
    {
        Console.Clear();
        ConsoleUtility.ShowTitle("■ 퀘스트 세부내용 ■");
        Console.WriteLine();
        Console.WriteLine($"~ {quest[idx].Title} ~");
        Console.WriteLine();


        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[ 퀘스트 주요내용 ]");
        Console.ResetColor();

        Console.WriteLine(quest[idx].Description);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[ 퀘스트 요약 ]");
        Console.ResetColor();

        Console.WriteLine(quest[idx].Summary);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[ 보상 ]");
        Console.ResetColor();

        Console.WriteLine(quest[idx].RewardItem);
        Console.WriteLine(quest[idx].RewardGold + " G");
        Console.WriteLine();
        Console.WriteLine("1. 수락하기");
        Console.WriteLine("2. 거절하기");
        Console.WriteLine();

        switch (ConsoleUtility.PromotMenuChoice(1, 2))
        {
            case 1:
                Console.WriteLine();
                Console.WriteLine("미구현 입니다. 아무 키를 눌러 이전화면으로 돌아갈 수 있습니다.");
                Console.ReadKey();
                QuestMenu();
                break;
            case 2:
                QuestMenu();
                break;
        }
        MainMenu();
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

