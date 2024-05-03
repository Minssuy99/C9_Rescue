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
