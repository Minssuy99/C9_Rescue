using ConsoleApp1;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

public partial class GameManager
{
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
                    battleMonster.Add(randomMonster.CloneMonster());
                    Console.WriteLine($"Lv.{battleMonster[i].Level} {battleMonster[i].Name} HP {battleMonster[i].CurrentHp}");
                    break;
                }
            }
        }

        Console.WriteLine();

        ShowPlayerStatus();
        
        Console.WriteLine("1. 싸운다");
        if (player.InTower = false)
        {
            Console.WriteLine("0. 도망친다.");
            Console.WriteLine();

            int choice = ConsoleUtility.PromotMenuChoice(0, 1);

            if (choice == 1)
            {
                StartBattle(random);
            }
            if (choice == 0)
            {
                DungeonChoiceMenu();
            }
        }
        else
        {
            Console.WriteLine();
            int choice = ConsoleUtility.PromotMenuChoice(1, 1);

            if (choice == 1)
            {
                StartBattle(random);
            }
        }
    }

    public void StartBattle(int random)
    {
        while (true)
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("■ Battle!! ■");
            Console.WriteLine();

            ShowMonsterStatus(random);

            ShowPlayerStatus();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[행동 선택]"); //s2를 강조함
            Console.ResetColor();
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 스킬");
            Console.WriteLine("3. 아이템");
            Console.WriteLine();
            int cho1 = ConsoleUtility.PromotMenuChoice(1, 3);
            if (cho1 == 1)
            {
                Console.WriteLine();
                ConsoleUtility.PrintTextHighlights("", "[몬스터 선택]");
                Console.WriteLine();
                Console.WriteLine("0. 뒤로 가기");

                int choice = GetPlayerChoice(random);
                if (choice == 0)
                {
                    GameManager.Instance.StartBattle(random);
                }

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
                if (GameManager.Instance.skills.Count == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("사용할 수 있는 스킬이 없습니다.");
                    Console.WriteLine();
                    Console.WriteLine("아무 키나 눌러주세요.");
                    Console.ReadKey();
                }
                else
                {
                    for (int i = 0; i < skills.Count; i++)
                    {
                        skills[i].PrintSkillStatDescription(i);
                    }

                }

                Skill.UseSkill(random);

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
            else if (cho1 == 3)
            {
                Console.WriteLine("미구현");
                continue;
            }
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
            Console.WriteLine();

            int choice = ConsoleUtility.PromotMenuChoice(1, 1);

            switch (choice)
            {
                case 1:
                    if (player.InTower)
                        BattleManu(player.NowDongeon * 2 + 1);
                    else
                        BattleManu(max);
                    break;
                case 0:
                    MainMenu();
                    break;
            }
        }

        else
        {
            Console.WriteLine("1. 전투 시작");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine();

            int choice = ConsoleUtility.PromotMenuChoice(0, 1);

            switch (choice)
            {
                case 1:
                    if (player.InTower)
                        BattleManu(player.NowDongeon * 2 + 1);
                    else
                        BattleManu(max);
                    break;
                case 0:
                    MainMenu();
                    break;
            }
        } 
        MainMenu();
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
}