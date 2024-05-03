using ConsoleApp1;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

public class GameManager
{
    private static GameManager instance;
    public Player player;

    public List<Skill> skills;

    private List<Item> inventory;
    private List<Item> storeInventory;
    private List<Item> dungeonItemList;
    private List<Item> weaponReward;

    private List<Monster> monster;
    public List<Monster> battleMonster;
    public Random battleRandom = new Random();

    private List<Dungeon> dungeons;

    private List<Quest> quest;

    private List<Potion> potion;
    private List<Potion> potioninventory;
    private List<Potion> potionReward;

    private DataBase dataBase = new DataBase();
    public GameManager()
    {
        InitializeGame();
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
    private void InitializeGame()
    {
        player = new Player("", "", 1, 10, 5, 100, 100, 11, 0, 15000);
        battleMonster = new List<Monster>();

        monster = new List<Monster>();
        monster.Add(new Monster("미니언", 2, 5, 2, 15, 15, 2, "빨간 포션"));
        monster.Add(new Monster("대포미니언", 5, 9, 5, 25, 25, 5, "대포"));
        monster.Add(new Monster("공허충", 3, 8, 3, 10, 10, 3, "빨간 포션"));
        monster.Add(new Monster("칼날부리새끼", 1, 3, 1, 8, 8, 1, "빨간 포션"));
        monster.Add(new Monster("어스름 늑대", 5, 10, 5, 8, 8, 5, "주황 포션"));

        skills = new List<Skill>();
        skills.Add(new Skill("파이어볼", "불공", SkillType.AttackSkills, 1, 0, SkillRangeType.DirectDamage)); //견본용 스킬 추가 템플릿 스킬 타입은 0이 공격 1이 서포트 스킬 레인지는 0이 단일 1이 광역
        skills.Add(new Skill("메테오", "운석", SkillType.AttackSkills, 1, 10, SkillRangeType.AreaOfEffect));

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
                            "상점에서 [낡은 검] 구입 후 장착",
                            "수련자 갑옷", 10));

        quest.Add(new Quest("더욱 더 강해지기!",
                            "모험가여, 당신의 모험은 아직 끝나지 않았다." +
                            "계속해서 성장하고 더욱 강해져야 한다." +
                            "자, 새로운 도전에 나서보시게!",
                            "몬스터 10마리 처치 (0/10)",
                            "도끼 x 1", 15));

        potion = new List<Potion>();
        potion.Add(new Potion("빨간 포션", "붉은 약초로 만든 물약이다", PotionEffect.Heal, 10, 100));
        potion.Add(new Potion("주황 포션", "붉은 약초의 농축 물약이다.", PotionEffect.Heal, 15, 150));
        potion.Add(new Potion("하얀 포션", "붉은 약초의 고농축 물약이다.", PotionEffect.Heal, 30, 300));

        potioninventory = new List<Potion>();

        potionReward = new List<Potion>();
    }


    public void StartGame()
    {
        Console.Clear();
        ConsoleUtility.PrintGameHeader();
        if(!dataBase.LoadData(ref player,ref inventory))
            player.PlayerCreate(player);
        MainMenu();
    }

    public void MainMenu()
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

        int choice = ConsoleUtility.PromotMenuChoice(1, 7);

        switch (choice)
        {
            case 1:
                StatusMenu(); //상태보기
                break;
            case 2:
                InventoryMenu(MainMenu); //인벤토리
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
        int choice = ConsoleUtility.PromotMenuChoice(0,1);
        switch (choice)
        {
            case 0:
                MainMenu();
                break;
            case 1:
                if (player.Gold < 500)
                {
                    Console.WriteLine("안돼 돌아가");
                    dataBase.SavePlayer(player);
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
            Console.WriteLine($"1. 전투 시작 (현재 진행 : {player.NowDongeon})");
            Console.WriteLine("2. 상태 보기");
        }

        else
        {
            Console.WriteLine("1. 전투 시작");
            Console.WriteLine("2. 상태 보기");
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

            ShowMonsterStatus(random);

            ShowPlayerStatus();

            Console.WriteLine();
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 스킬");
            Console.WriteLine("3. 아이템");
            Console.WriteLine();
            int cho1 = ConsoleUtility.PromotMenuChoice(1, 3);
            if(cho1 == 1)
            {
                int choice = GetPlayerChoice(random);

                int selectedMonsterIndex = choice - 1;
                Monster selectedMonster = battleMonster[selectedMonsterIndex];

                if (!ValidateMonsterChoice(selectedMonsterIndex))
                    continue;

                PlayerAttack(selectedMonster);

                if (CheckAllMonstersDead())
                {
                    HandleAllMonstersDefeat();
                    ShowResultMenu(random);
                    break;
                }

                MonsterAttack();

                if (!player.IsAlive())
                {
                    HandlePlayerDefeat();
                    break;
                }

                Console.WriteLine("아무키나 누르세요...");
                Console.ReadKey();
            }
            else if (cho1 == 2)
            {
                for (int i = 0; i < skills.Count; i++)
                {
                    skills[i].PrintSkillStatDescription(i);
                }
                Skill.UseSkill(random);
            }
            else if (cho1 == 3)
            {
                Console.WriteLine("미구현");
                continue;
            }
        }
    }

    private void ShowMonsterStatus(int random)
    {
        for (int i = 0; i < random; i++)
        {
            if (!battleMonster[i].IsAlive())
                ConsoleUtility.PrintTextDeath("", $"[{i + 1}] Lv.{battleMonster[i].Level} {battleMonster[i].Name} HP {battleMonster[i].CurrentHp}");
            else
                Console.WriteLine($"[{i + 1}] Lv.{battleMonster[i].Level} {battleMonster[i].Name} HP {battleMonster[i].CurrentHp}");
        }
        Console.WriteLine();
    }

    private void ShowPlayerStatus()
    {
        Console.WriteLine("[내정보]");
        Console.WriteLine($"Lv.{player.Level}  {player.Name}({player.Job})");
        Console.WriteLine($"HP {player.CurrentHp} / {player.MaxHp}");
        Console.WriteLine();
    }

    private int GetPlayerChoice(int random)
    {
        return ConsoleUtility.PromotMenuChoice(1, random);
    }

    private bool ValidateMonsterChoice(int selectedMonsterIndex)
    {
        if (!battleMonster[selectedMonsterIndex].IsAlive())
        {
            Console.WriteLine("선택한 몬스터가 이미 죽었습니다. 다른 몬스터를 선택하세요.");
            Console.ReadKey();
            return false;
        }
        return true;
    }

    private void PlayerAttack(Monster selectedMonster)
    {
        Console.WriteLine();
        ConsoleUtility.PrintTextHighlights("", $"{player.Name}가 공격!");

        double minAtk = Math.Ceiling(player.Atk * 0.9); // 최소 공격력 (원래 공격력의 90%)
        double maxAtk = Math.Ceiling(player.Atk * 1.1); // 최대 공격력 (원래 공격력의 110%)
        if (battleRandom.Next(100) < 10)
        {
            Console.WriteLine("회피!");
            Console.WriteLine($"몬스터 {selectedMonster.Name}이(가) 공격을 회피했습니다.");
        }
        else
        {
            int attackDamage = battleRandom.Next((int)minAtk, (int)maxAtk);
            if (IsCriticalHit())
                InflictCriticalDamage(selectedMonster, attackDamage);
            else
                InflictRegularDamage(selectedMonster, attackDamage);

            if (!selectedMonster.IsAlive())
            {
                HandleMonsterDefeat(selectedMonster);
            }
        }
        Thread.Sleep(1000);
    }

    private bool IsCriticalHit()
    {
        return battleRandom.Next(100) < 15;
    }

    private void InflictCriticalDamage(Monster selectedMonster, int attackDamage)
    {
        int criticalDamage = (int)(attackDamage * 1.6);
        Console.WriteLine("치명타 발생! 추가 데미지를 가합니다.");
        Console.WriteLine($"[데미지 : {criticalDamage}]");
        selectedMonster.MonterTakeDamage(criticalDamage);
    }

    private void InflictRegularDamage(Monster selectedMonster, int attackDamage)
    {
        Console.WriteLine($"[데미지 : {attackDamage}]");
        selectedMonster.MonterTakeDamage(attackDamage);
    }

    private void HandleMonsterDefeat(Monster selectedMonster)
    {
        Console.WriteLine($"몬스터 {selectedMonster.Name}를 물리쳤습니다!");
        Console.WriteLine($"경험치 {selectedMonster.Exp}를 얻었습니다!");
        player.GetExp(selectedMonster.Exp);
        player.LevelUp();

        MonsterDied(selectedMonster);

        ConsoleUtility.PrintTextHighlights("경험치 :", $"{player.CurrentExp}/{player.MaxExp}".ToString());
    }

    private bool CheckAllMonstersDead()
    {
        foreach (var monster in battleMonster)
        {
            if (monster.IsAlive())
                return false;
        }
        return true;
    }

    private void HandleAllMonstersDefeat()
    {
        Console.WriteLine();
        Console.WriteLine("모든 몬스터를 물리쳤습니다.");

        ResetMonsters();

        Thread.Sleep(1000);
        Console.WriteLine("아무키나 누르세요...");
        Console.ReadKey();
    }

    private void MonsterAttack()
    {
        Console.WriteLine();
        Console.WriteLine("몬스터 차례");

        double mminAtk = Math.Ceiling(player.Atk * 0.9); // 최소 공격력 (원래 공격력의 90%)
        double mmaxAtk = Math.Ceiling(player.Atk * 1.1); // 최대 공격력 (원래 공격력의 110%)

        foreach (Monster m in battleMonster)
        {
            if (m.IsAlive())
            {
                if (IsPlayerEvaded())
                {
                    Console.WriteLine("회피!");
                    Console.WriteLine($"{player.Name}이(가) 공격을 회피했습니다.");
                }
                else
                {
                    int attackDamage = battleRandom.Next((int)mminAtk, (int)mmaxAtk);

                    ConsoleUtility.PrintTextHighlights("", $"{m.Name}이(가) 공격합니다!");

                    if (IsMonsterCriticalHit())
                        InflictMonsterCriticalDamage(m, attackDamage);
                    else
                        InflictMonsterRegularDamage(m, attackDamage);

                    ShowPlayerHpAfterMonsterAttack();
                }
            }
        }
    }

    private bool IsPlayerEvaded()
    {
        return battleRandom.Next(100) < 10;
    }

    private bool IsMonsterCriticalHit()
    {
        return battleRandom.Next(100) < 15;
    }

    private void InflictMonsterCriticalDamage(Monster monster, int attackDamage)
    {
        int criticalDamage = (int)(attackDamage * 1.6);
        Console.WriteLine("치명타 발생! 추가 피해를 입었습니다.");
        Console.WriteLine($"[데미지 : {criticalDamage}]");
        player.PlayerTakeDamage(criticalDamage);
    }

    private void InflictMonsterRegularDamage(Monster monster, int attackDamage)
    {
        Console.WriteLine($"[데미지 : {attackDamage}]");
        player.PlayerTakeDamage(attackDamage);
    }

    private void ShowPlayerHpAfterMonsterAttack()
    {
        Console.WriteLine($"{player.Name}의 HP: {player.CurrentHp}");
        Console.WriteLine();
        Thread.Sleep(1000);
    }

    private void HandlePlayerDefeat()
    {
        Console.WriteLine("전투에서 패배했습니다.");
        Console.WriteLine("마을로 돌아갑니다.");

        ResetMonsters();
        player.InTower = false;
        player.playerdefeat();

        Thread.Sleep(1000);
        Console.ReadKey();
        MainMenu();
    }

    private void ResetMonsters()
    {
        foreach (var monster in battleMonster)
        {
            monster.Reset();
        }
    }


    // 몬스터가 죽었을 때 호출되는 메서드
    public void MonsterDied(Monster selectedMonster)
    {
        // 몬스터가 가지고 있는 아이템의 인덱스를 찾기, 찾지 못하면 -1 반환
        int itemIndex = dungeonItemList.FindIndex(item => item.Name == selectedMonster.DropItem);
        int potionIndex = potion.FindIndex(p => p.Name == selectedMonster.DropItem);

        // 해당 아이템을 찾았는지 확인 후 인벤토리에 추가
        if (itemIndex != -1)
        {
            if (battleRandom.Next(100) < 10) //확률적으로 아이템을 드랍함
            {
                Item droppedItem = dungeonItemList[itemIndex].CloneItem();
                weaponReward.Add(droppedItem);
                Console.WriteLine($"{selectedMonster.DropItem}를 얻었습니다!");
            }
        }

        if (potionIndex != -1)
        {
            if (battleRandom.Next(100) < 101) //확률적으로 아이템을 드랍함
            {
                Potion droppedItem = potion[potionIndex].ClonePotion();
                potionReward.Add(droppedItem);
                Console.WriteLine($"{selectedMonster.DropItem}를 얻었습니다!");
            }
        }
    }

    public void ShowResultMenu(int random)
    {
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

        Dictionary<string, int> potionCounts = new Dictionary<string, int>();

        for (int i = 0; i < potionReward.Count; i++)
        {
            string potionName = potionReward[i].Name; //포션의 이름

            if (!potionCounts.ContainsKey(potionName))
            {
                potionCounts[potionName] = 1;
            }
            else
            {
                potionCounts[potionName]++;
            }

            if (!potioninventory.Any(p => p.Name == potionName)) //포션이 인벤토리에 없다면
            {
                Potion newPotion = potionReward[i].ClonePotion(); //복제해서
                newPotion.TogglePurchase(); //복제한걸 증가시켜줌
                potioninventory.Add(newPotion); //증가시킨 포션을 추가
            }
            else //이미 있는 포션이라면
            {
                potioninventory.First(p => p.Name == potionName).TogglePurchase(); //제일 첫번째 있는 이름같은 포션의 개수 증가
            }
        }

        foreach (var kvp in potionCounts)
        {
            Console.WriteLine($"{kvp.Key} {kvp.Value}개를 얻었습니다!");
        }

        //여기에 보상 목록 추가
        potionReward.Clear();
        weaponReward.Clear();
        Console.WriteLine();

        Thread.Sleep(1000);

        if (player.InTower)
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

    private void InventoryMenu(Action returnToPreviousMenu)
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
                returnToPreviousMenu();
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
            potioninventory[i].PrintPotionStatDescription(true, i + 1, true); //나가기가 0번이라서 +1해줘서 띄워줌
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int keyInput = ConsoleUtility.PromotMenuChoice(0, potioninventory.Count);

        switch (keyInput)
        {
            case 0:
                InventoryMenu(ItemMenu);
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
                InventoryMenu(EquipMenu);
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
            Console.WriteLine($"{i + 1}. {quest[i].Title}");
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

    private void PrintQuestDetails(int questidx)
    {
        Console.Clear();
        ConsoleUtility.ShowTitle("■ 퀘스트 세부내용 ■");
        if (quest[questidx].IsAccepted == true)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[진행중]");
            Console.ResetColor();
        }
        Console.WriteLine();
        Console.WriteLine($"~ {quest[questidx].Title} ~");
        Console.WriteLine();


        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[ 퀘스트 주요내용 ]");
        Console.ResetColor();

        Console.WriteLine(quest[questidx].Description);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[ 퀘스트 요약 ]");
        Console.ResetColor();

        Console.WriteLine(quest[questidx].Summary);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[ 보상 ]");
        Console.ResetColor();

        Console.WriteLine(quest[questidx].RewardItem);
        Console.WriteLine(quest[questidx].RewardGold + " G");
        Console.WriteLine();

        if (quest[questidx].IsAccepted == true)
        {
            Console.WriteLine("1. 거절하기");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine();
            switch (ConsoleUtility.PromotMenuChoice(0, 1))
            {
                case 0:
                    QuestMenu();
                    break;
                case 1:
                    quest[questidx].IsAccepted = false;
                    PrintQuestDetails(questidx);
                    break;
            }
            QuestMenu();
        }
        Console.WriteLine("1. 수락하기");
        Console.WriteLine("0. 뒤로가기");
        Console.WriteLine();

        switch (ConsoleUtility.PromotMenuChoice(0, 1))
        {
            case 0:
                QuestMenu();
                break;
            case 1:
                quest[questidx].IsAccepted = true;
                PrintQuestDetails(questidx);
                Console.ReadKey();
                QuestMenu();
                break;
        }
        QuestMenu();
    }
}

public class Program
{
    static void Main(string[] args)
    {
        GameManager.Instance.StartGame();
    }
}

