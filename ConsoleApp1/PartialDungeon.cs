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
            if (cho1 == 1)
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