using ConsoleApp1;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

public partial class GameManager
{
    private void QuestMenu()
    {
        Console.Clear();
        Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■");
        Console.WriteLine("퀘스트 메뉴");
        Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■");
        Console.WriteLine();

        // 퀘스트 종류 랜덤 선택
        Quest.QuestType questType = (Quest.QuestType)battleRandom.Next(0, 3);

        // 퀘스트 난이도 랜덤 선택
        Quest.QuestDifficulty difficulty = (Quest.QuestDifficulty)battleRandom.Next(0, 3);

        // 난이도에 따라 퀘스트 내용과 보상 설정
        int questTarget = 0;
        int rewardGold = 0;

        string title = "";
        string description = "";

        switch (questType)
        {
            case Quest.QuestType.Hunt:
                title = "몬스터 토벌";
                description = "마을 주변에 나타난 악의적인 몬스터들을 처치하여 마을을 안전하게 만드세요.";
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
            case Quest.QuestType.Collect:
                title = "아이템 수집";
                description = "마을 주변의 산속에 흩어진 소중한 아이템들을 찾아 수집하세요.";
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
                title = "장비 장착";
                description = "강력한 장비를 장착하여 마을을 지키세요. 전설의 무기를 획득하여 마을을 방어할 수 있습니다.";
                questTarget = difficulty switch
                {
                    Quest.QuestDifficulty.Easy => 1,
                    Quest.QuestDifficulty.Normal => 2,
                    Quest.QuestDifficulty.Hard => 3,
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

        // 퀘스트 객체 생성
        Quest quest = new Quest(title, description, questType, difficulty, questTarget, rewardGold);

        // 퀘스트 정보 출력
        Console.WriteLine($"[퀘스트] {quest.Title}\n{quest.Description}\n종류: {Quest.GetQuestTypeName(quest.Type)} | 난이도: {Quest.GetQuestDifficultyName(quest.Difficulty)} | 목표: {quest.TargetAmount} | 보상: 골드 {quest.RewardGold}");

        // 사용자 입력 대기
        Console.WriteLine("\n아무 키나 누르면 메인 메뉴로 돌아갑니다...");
        Console.ReadKey();

        // 메인 메뉴로 복귀
        MainMenu();
    }
}
