using ConsoleApp1;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

public partial class GameManager
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

    private List<Potion> potion;
    private List<Potion> potioninventory;
    private List<Potion> potionReward;

    private DataBase dataBase = new DataBase();
    public GameManager()
    {
        InitializeGame();

        while (quest.Count < 3)
        {
            Quest newQuest = GenerateQuest();
            if (!quest.Any(q => q.Title == newQuest.Title))
            {
                quest.Add(newQuest);
            }
        }
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
        player = new Player("", "", 1, 10, 5, 100, 100, 50, 50, 11, 0, 15000);

        battleMonster = new List<Monster>();

        monster = new List<Monster>();
        monster.Add(new Monster("미니언", 2, 5, 2, 15, 15, 2, "빨간 포션"));
        monster.Add(new Monster("대포미니언", 5, 9, 5, 25, 25, 5, "대포"));
        monster.Add(new Monster("공허충", 3, 8, 3, 10, 10, 3, "빨간 포션"));
        monster.Add(new Monster("칼날부리", 1, 3, 1, 8, 8, 1, "빨간 포션"));
        monster.Add(new Monster("어스름 늑대", 5, 10, 5, 8, 8, 5, "주황 포션"));
        monster.Add(new Monster("고블린", 2, 5, 4, 14, 14, 3, "빨간 포션"));

        skills = new List<Skill>();
        skills.Add(new Skill("파이어볼", "불공", SkillType.AttackSkills, 1, 10, 20, SkillRangeType.DirectDamage)); //견본용 스킬 추가 템플릿 스킬 타입은 0이 공격 1이 서포트 스킬 레인지는 0이 단일 1이 광역
        skills.Add(new Skill("메테오", "운석", SkillType.AttackSkills, 1, 20, 10, SkillRangeType.AreaOfEffect));

        inventory = new List<Item>();

        storeInventory = new List<Item>();
        storeInventory.Add(new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", ItemType.ARMOR, 0, 5, 0, 0, 500));
        storeInventory.Add(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", ItemType.ARMOR, 0, 9, 0, 0, 2000));
        storeInventory.Add(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", ItemType.ARMOR, 0, 15, 0, 0, 3500));
        storeInventory.Add(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", ItemType.WEAPON, 2, 0, 0, 0, 600));
        storeInventory.Add(new Item("청동 도끼", "어디선가 사용됐던 것 같은 도끼입니다.", ItemType.WEAPON, 5, 0, 0, 0, 1500));
        storeInventory.Add(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", ItemType.WEAPON, 7, 0, 0, 0,  2500));

        dungeons = new List<Dungeon>();
        dungeons.Add(new Dungeon(1, "초급던전", 5, 3, 1000));
        dungeons.Add(new Dungeon(2, "중급던전", 15, 5, 3000));
        dungeons.Add(new Dungeon(3, "고급던전", 25, 7, 5000));

        dungeonItemList = new List<Item>();
        dungeonItemList.Add(new Item("대포", "대포 미니언의 강력한 대포입니다.", ItemType.WEAPON, 9, 0, 0, 0, 300));

        weaponReward = new List<Item>();

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
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("체력이 회복됐습니다.");
                    dataBase.SavePlayer(player);
                    player.Rest();
                    Console.ReadKey();
                }
                break;
        }
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
        int bonusMp = inventory.Select(item => item.IsEquipped ? item.Mp : 0).Sum();


        ConsoleUtility.PrintTextHighlights("공격력 :", (player.Atk).ToString(), bonusAtk > 0 ? $"(+{bonusAtk})" : "");
        ConsoleUtility.PrintTextHighlights("방어력 :", (player.Def).ToString(), bonusDef > 0 ? $"(+{bonusDef})" : "");
        ConsoleUtility.PrintTextHighlights("체  력 :", $"{player.CurrentHp + bonusHp}/{(player.MaxHp + bonusHp)}".ToString(), bonusHp > 0 ? $"(+{bonusHp})" : "");
        ConsoleUtility.PrintTextHighlights("마  력 :", $"{player.CurrentMp + bonusMp}/{(player.MaxMp + bonusMp)}".ToString(), bonusMp > 0 ? $"(+{bonusMp})" : "");
        ConsoleUtility.PrintTextHighlights("경험치 :", $"{player.CurrentExp}/{player.MaxExp}");

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
}

public class Program
{
    static void Main(string[] args)
    {
        GameManager.Instance.StartGame();
    }
}

