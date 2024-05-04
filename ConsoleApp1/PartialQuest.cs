using ConsoleApp1;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

public partial class GameManager
{
    List<Quest> quest = new List<Quest>();
    List<bool> questStatus = new List<bool>();
    private Quest GenerateQuest()
    {
        Quest.QuestType questType = (Quest.QuestType)battleRandom.Next(0, 3);
        Quest.QuestDifficulty difficulty = (Quest.QuestDifficulty)battleRandom.Next(0, 3);
        string title = "";
        string description = "";
        int questTarget = 0;
        int rewardGold = 0;

        switch (questType)
        {
            case Quest.QuestType.Hunt:
                int huntQuestIndex = battleRandom.Next(0, 2);
                switch (huntQuestIndex)
                {
                    case 0:
                        title = "칼날부리 사냥";
                        description = "칼날부리들을 처치하여 마을을 안전하게 만드세요.";
                        questTarget = difficulty switch
                        {
                            Quest.QuestDifficulty.Easy => battleRandom.Next(1, 5),
                            Quest.QuestDifficulty.Normal => battleRandom.Next(5, 10),
                            Quest.QuestDifficulty.Hard => battleRandom.Next(10, 15),
                            _ => 0,
                        };
                        rewardGold = difficulty switch
                        {
                            Quest.QuestDifficulty.Easy => 500,
                            Quest.QuestDifficulty.Normal => 1000,
                            Quest.QuestDifficulty.Hard => 1500,
                            _ => 0,
                        };
                        break;
                    case 1:
                        title = "고블린 사냥";
                        description = "고블린을 처치하여 마을을 안전하게 만드세요.";
                        questTarget = difficulty switch
                        {
                            Quest.QuestDifficulty.Easy => battleRandom.Next(1, 5),
                            Quest.QuestDifficulty.Normal => battleRandom.Next(5, 10),
                            Quest.QuestDifficulty.Hard => battleRandom.Next(10, 15),
                            _ => 0,
                        };
                        rewardGold = difficulty switch
                        {
                            Quest.QuestDifficulty.Easy => 500,
                            Quest.QuestDifficulty.Normal => 1000,
                            Quest.QuestDifficulty.Hard => 1500,
                            _ => 0,
                        };
                        break;
                }
                break;
            case Quest.QuestType.Collect:
                title = "따뜻한 겨울나기";
                description = "옷을 만들기 위해 필요한 늑대의 털을 가져다주세요.";
                questTarget = difficulty switch
                {
                    Quest.QuestDifficulty.Easy => battleRandom.Next(1, 3),
                    Quest.QuestDifficulty.Normal => battleRandom.Next(3, 6),
                    Quest.QuestDifficulty.Hard => battleRandom.Next(6, 10),
                    _ => 0,
                };
                rewardGold = difficulty switch
                {
                    Quest.QuestDifficulty.Easy => 300,
                    Quest.QuestDifficulty.Normal => 600,
                    Quest.QuestDifficulty.Hard => 1000,
                    _ => 0,
                };
                break;
            case Quest.QuestType.Equip:
                title = "장비 장착을 해보자";
                description = "무기를 획득하여 장착하세요.";
                questTarget = difficulty switch
                {
                    Quest.QuestDifficulty.Easy => 1,
                    Quest.QuestDifficulty.Normal => 1,
                    Quest.QuestDifficulty.Hard => 1,
                    _ => 0,
                };
                rewardGold = difficulty switch
                {
                    Quest.QuestDifficulty.Easy => 200,
                    Quest.QuestDifficulty.Normal => 400,
                    Quest.QuestDifficulty.Hard => 600,
                    _ => 0,
                };
                break;
        }

        questStatus.Add(false);

        return new Quest(title, description, questType, difficulty, questTarget, rewardGold);
    }

    private void QuestMenu()
    {
        Console.Clear();
        ConsoleUtility.ShowTitle("■ 퀘스트 메뉴 ■");
        Console.WriteLine();

        Console.WriteLine("[퀘스트 목록]");
        Console.WriteLine();

        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine($"[{i + 1}] {quest[i].Title} {(questStatus[i] ? "(진행중)" : "")}"); //bool값이 참이면 (진행중)
            Console.WriteLine($"{quest[i].Description}");
            Console.WriteLine($"퀘스트 유형: {Quest.GetQuestTypeName(quest[i].Type)} | 난이도: {Quest.GetQuestDifficultyName(quest[i].Difficulty)} | 목표 수치: {quest[i].TargetAmount} | 보상: {quest[i].RewardGold} G");
            Console.WriteLine();
        }

        Console.WriteLine("퀘스트를 선택하세요.");
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int choice = ConsoleUtility.PromotMenuChoice(0, 3);
        switch (choice)
        {
            case 0:
                MainMenu();
                break;
            default:
                Console.Clear();
                int selectedQuestIndex = choice - 1;
                Console.WriteLine("[퀘스트]");
                Console.WriteLine($"{quest[selectedQuestIndex].Title}");
                Console.WriteLine($"{quest[selectedQuestIndex].Description}");
                Console.WriteLine($"퀘스트 유형: {Quest.GetQuestTypeName(quest[selectedQuestIndex].Type)} | 난이도: {Quest.GetQuestDifficultyName(quest[selectedQuestIndex].Difficulty)} | 목표 수치: {quest[selectedQuestIndex].TargetAmount} | 보상: {quest[selectedQuestIndex].RewardGold} G");
                Console.WriteLine();
                if (questStatus[selectedQuestIndex])
                {
                    Console.WriteLine("진행중인 퀘스트입니다.");
                    Console.WriteLine($"{quest[selectedQuestIndex].Title} 퀘스트를 포기하겠습니까?");
                    Console.WriteLine("1. 포기한다.");
                    Console.WriteLine("2. 뒤로가기");
                    Console.WriteLine();

                    int abandonChoice = ConsoleUtility.PromotMenuChoice(1, 2);
                    if (abandonChoice == 1)
                    {
                        questStatus[selectedQuestIndex] = false;
                        Console.WriteLine("퀘스트를 포기했습니다.");
                        Console.WriteLine("아무 키나 누르세요...");
                        Console.ReadKey();
                        QuestMenu();
                    }
                    else
                    {
                        QuestMenu();
                    }
                }
                else
                {
                    Console.WriteLine($"{quest[selectedQuestIndex].Title} 퀘스트를 수락하시겠습니까?");
                    Console.WriteLine("1. 수락한다.");
                    Console.WriteLine("0. 거절한다.");
                    Console.WriteLine();

                    int acceptChoice = ConsoleUtility.PromotMenuChoice(0, 1);

                    if (acceptChoice == 1)
                    {
                        questStatus[selectedQuestIndex] = true;
                        Console.WriteLine($"퀘스트를 수락했습니다!");
                        Console.WriteLine("아무 키나 누르세요...");
                        Console.ReadKey();
                        QuestMenu();
                    }
                    else
                    {
                        questStatus[selectedQuestIndex] = false;
                        Console.WriteLine("퀘스트를 거절했습니다.");
                        Console.WriteLine("아무 키나 누르세요...");
                        Console.ReadKey();
                        QuestMenu();
                    }
                }
                break;
        }
    }
}
