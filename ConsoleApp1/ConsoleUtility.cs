



internal class ConsoleUtility
{
    public static void PrintGameHeader()
    {
        Console.WriteLine("======================================================");
        Console.WriteLine("     _____ _____   _____  _______  _______  _____     ");
        Console.WriteLine("    / ____|  __  |  /| | |   __  ||__   __|/ /| |");
        Console.WriteLine("   | (___ | |__) /   | | |  |__) |   | |  /   | |  ");
        Console.WriteLine("    | ___ |  ___/ /__| | |   _   /   | | / /__| |      ");
        Console.WriteLine("    ____) | |  / ____| | |  |  | |   | |/ ____| |   ");
        Console.WriteLine("  _|_____/|_| /_/   _|_|_|__|_ |_|_ _|_|__ |_ | |_ ");
        Console.WriteLine(" |  __ || |  | | |  | |  ____|  ____/ __ ||  |  | |");
        Console.WriteLine(" | |  | | |  | |  | | | | ___| |_  | |  | | | | | |");
        Console.WriteLine(" | |  | | |  | | . `| | ||_  |  _||| |  | | || || |");
        Console.WriteLine(" | |__| | |__| | |||  | |__| | |___| |__| | | | | |");
        Console.WriteLine(" |_____/||____/|_| ||_| _____|______|____/|_|  | _|");
        Console.WriteLine("======================================================");
        Console.WriteLine("               아무키나 누르세요...");
        Console.WriteLine("======================================================");
        Console.ReadKey();
    }

    public static int PromotMenuChoice(int min, int max)
    {
        while (true)
        {
            Console.Write("원하시는 번호를 입력해주세요.");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
            {
                return choice;
            }
            Console.WriteLine("잘못된 입력입니다. 다시 시도해주세요.");
        }
    }

    internal static void ShowTitle(string title) //제목 강조
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(title);
        Console.ResetColor();
    }

    public static void PrintTextHighlights(string s1, string s2, string s3 = "") //s2를 강조하는 함수
    {
        Console.Write(s1);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(s2); //s2를 강조함
        Console.ResetColor();
        Console.WriteLine(s3);
    }

    public static int GetPrintableLength(string str)
    {
        int length = 0;
        foreach (char c in str)
        {
            if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter) //카테고리에 해당하는 글자는 긴글자다.
            {
                length += 2; //가나다
            }
            else
            {
                length += 1; //12abcd
            }
        }

        return length;
    }

    public static string PadRightForMixedText(string str, int totalLength)
    {
        int currentLength = GetPrintableLength(str);
        int padding = totalLength - currentLength;
        return str.PadRight(str.Length + padding); //PadRight 공간을 공백으로 채워준다
    }
}