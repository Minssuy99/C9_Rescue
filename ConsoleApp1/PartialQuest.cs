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
    private Quest GenerateQuest()
    {
        Quest.QuestType questType = (Quest.QuestType)battleRandom.Next(0, 3);
        Quest.QuestDifficulty difficulty = (Quest.QuestDifficulty)battleRandom.Next(0, 3);
        string title = "";
        string description = "";
        int questTarget = 0;
        int rewardGold = 0;
        string targetMonsterName = "";

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
                            _ => 0
                        };
                        rewardGold = difficulty switch
                        {
                            Quest.QuestDifficulty.Easy => 500,
                            Quest.QuestDifficulty.Normal => 1000,
                            Quest.QuestDifficulty.Hard => 1500,
                            _ => 0
                        };
                        targetMonsterName = "칼날부리";
                        break;
                    case 1:
                        title = "고블린 사냥";
                        description = "고블린을 처치하여 마을을 안전하게 만드세요.";
                        questTarget = difficulty switch
                        {
                            Quest.QuestDifficulty.Easy => battleRandom.Next(1, 5),
                            Quest.QuestDifficulty.Normal => battleRandom.Next(5, 10),
                            Quest.QuestDifficulty.Hard => battleRandom.Next(10, 15),
                            _ => 0
                        };
                        rewardGold = difficulty switch
                        {
                            Quest.QuestDifficulty.Easy => 500,
                            Quest.QuestDifficulty.Normal => 1000,
                            Quest.QuestDifficulty.Hard => 1500,
                            _ => 0
                        };
                        targetMonsterName = "고블린";
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
                    _ => 0
                };
                rewardGold = difficulty switch
                {
                    Quest.QuestDifficulty.Easy => 300,
                    Quest.QuestDifficulty.Normal => 600,
                    Quest.QuestDifficulty.Hard => 1000,
                    _ => 0
                };
                targetMonsterName = "어스름 늑대";
                break;
            case Quest.QuestType.Equip:
                title = "장비 장착을 해보자!";
                description = "[낡은 검]무기를 획득하여 장착하세요.";
                questTarget = 1;
                rewardGold = 600;
                break;
        }

        return new Quest(title, description, questType, difficulty, questTarget, rewardGold, targetMonsterName);
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
            Console.Write($"[{i + 1}]");
            DisplayQuestDetails(quest[i]);
        }

        Console.WriteLine("퀘스트를 선택하세요.");
        Console.WriteLine("4. 퀘스트 갱신 (500 G)");
        Console.WriteLine("0. 나가기");
        Console.WriteLine();

        int choice = ConsoleUtility.PromotMenuChoice(0, 4);
        switch (choice)
        {
            case 0:
                MainMenu();
                break;
            case 4:
                Console.WriteLine("퀘스트를 정말 초기화 하시겠습니까?");
                Console.WriteLine("(진행중인 퀘스트도 중단됩니다.)");
                Console.WriteLine("1. 네");
                Console.WriteLine("0. 아니오");
                Console.WriteLine();

                int choice1 = ConsoleUtility.PromotMenuChoice(0, 1);

                switch (choice1)
                {
                    case 0:
                        QuestMenu();
                        break;
                    case 1:
                        quest.Clear();
                        while (quest.Count < 3)
                        {
                            Quest newQuest = GenerateQuest();
                            if (!quest.Any(q => q.Title == newQuest.Title))
                            {
                                quest.Add(newQuest);
                            }
                        }
                        player.Gold -= 500;
                        Console.WriteLine("퀘스트가 갱신되었습니다.");
                        Console.WriteLine("아무 키나 누르세요...");
                        Console.ReadKey();
                        QuestMenu();
                        break;
                }
                break;
            default:
                Console.Clear();
                int selectedQuestIndex = choice - 1;
                Console.WriteLine("[퀘스트]");
                DisplayQuestDetails(quest[selectedQuestIndex]);
                Console.WriteLine();

                if (quest[selectedQuestIndex].IsCompleted)
                {
                    if (quest[selectedQuestIndex].IsRewarded)
                    {
                        Console.WriteLine($"퀘스트 '{quest[selectedQuestIndex].Title}'는 이미 완료된 퀘스트입니다.");
                    }
                    else
                    {
                        CheckQuestCompletion();
                    }
                    Console.WriteLine("아무 키나 누르세요...");
                    Console.ReadKey();
                    QuestMenu();
                }
                else if (quest[selectedQuestIndex].IsAccepted)
                {
                    Console.WriteLine("진행중인 퀘스트입니다.");
                    Console.WriteLine($"{quest[selectedQuestIndex].Title} 퀘스트를 포기하겠습니까?");
                    Console.WriteLine("1. 포기한다.");
                    Console.WriteLine("2. 뒤로가기");
                    Console.WriteLine();

                    int abandonChoice = ConsoleUtility.PromotMenuChoice(1, 2);
                    if (abandonChoice == 1)
                    {
                        quest[selectedQuestIndex].CurrentAmount = 0;
                        quest[selectedQuestIndex].IsAccepted = false;
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
                        quest[selectedQuestIndex].IsAccepted = true;
                        Console.WriteLine($"퀘스트를 수락했습니다!");
                        Console.WriteLine("아무 키나 누르세요...");
                        Console.ReadKey();
                        QuestMenu();
                    }
                    else
                    {
                        quest[selectedQuestIndex].IsAccepted = false;
                        Console.WriteLine("퀘스트를 거절했습니다.");
                        Console.WriteLine("아무 키나 누르세요...");
                        Console.ReadKey();
                        QuestMenu();
                    }
                }
                break;
        }
    }
    private void CheckQuestCompletion()
    {
        foreach (var quest in quest)
        {
            if (quest.IsCompleted)
            {
                Console.WriteLine($"퀘스트 '{quest.Title}'가 완료되었습니다!");

                // 보상
                player.Gold += quest.RewardGold;

                // 여기에 다른 보상 추가 가능함

                Console.WriteLine($"보상으로 {quest.RewardGold}골드를 받았습니다.");

                quest.RewardedQuest();
            }
        }
    }

    private void DisplayQuestDetails(Quest quest)
    {
        Console.WriteLine($"{quest.Title} {(quest.IsAccepted ? "(진행중)" : "")} {(quest.IsCompleted ? "(완료)" : "")}");
        Console.WriteLine($"{quest.Description}");
        if (quest.Type != Quest.QuestType.Equip)
        {
            Console.WriteLine($"퀘스트 유형: {Quest.GetQuestTypeName(quest.Type)} | 난이도: {Quest.GetQuestDifficultyName(quest.Difficulty)} | 목표 수치: {quest.CurrentAmount}/{quest.TargetAmount} | 보상: {quest.RewardGold} G");
        }
        else
        {
            Console.WriteLine($"퀘스트 유형: {Quest.GetQuestTypeName(quest.Type)} | 보상: {quest.RewardGold} G");
        }
        Console.WriteLine();
    }
}
