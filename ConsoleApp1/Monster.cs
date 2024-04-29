

internal class Monster
{
    private int level;
    private string name;
    private int hp;
    private int atk;
    private bool isLive;
    public int Level { get => level; set => level = value; }
    public string Name { get => name; set => name = value; }
    public int Hp { get => hp; set => hp = value; }
    public int Atk { get => atk; set => atk = value; }
    public bool IsLive { get => isLive; set => isLive = value; }

    public Monster(int num)
    {
        switch (num)
        {        
            case 0:
                level = 2;name = "미니언";hp = 15;atk = 5;isLive = true;
                break;
            case 1: 
                level = 3;name = "공허충";hp = 10;atk = 9;isLive = true;
                break;
            case 2:
                level = 5; name = "대포미니언"; hp = 25; atk = 8; isLive = true;
                break;
        }
    }

    internal void PrintMonsterInfo(bool fight = false, int idx = 0)
    {
        if (fight)
        {
            Console.Write($"{idx}. ");
        }
        if (isLive)
        {
            ConsoleUtility.PrintTextHighlights("Lv", level.ToString(), name);
            ConsoleUtility.PrintTextHighlights("HP ", hp.ToString());
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            ConsoleUtility.PrintTextHighlights("Lv", level.ToString(), name);
            Console.Write("Dead");
        }

        Console.WriteLine();
    }
    public bool CorrectMonster()
    {
        return true;
    }

    internal void MonsterPhase(Player player)
    {
        Console.Clear();
        ConsoleUtility.PrintTextHighlights("", "Battle!");
        Console.WriteLine("\n");
        ConsoleUtility.PrintTextHighlights("Lv.",level.ToString());
        ConsoleUtility.PrintTextHighlights($"{name} 의 공격", "!");
        Console.WriteLine();
        ConsoleUtility.PrintTextHighlights($"{player.Name} 을(를) 맞췄습니다.   [데미지 : ", atk.ToString(), "]");
        Console.WriteLine("\n\n");
        ConsoleUtility.PrintTextHighlights("Lv.", player.Level.ToString(),player.Name);
        Console.WriteLine();
        ConsoleUtility.PrintTextHighlights("HP ", $"{player.Hp} -> {player.Hp -= atk}");
        Console.WriteLine("\n");
        ConsoleUtility.PrintTextHighlights("", "0. ", "다음");
        Console.WriteLine("\n");
        Console.WriteLine("대상을 선택해주세요.");

        switch(ConsoleUtility.PromotMenuChoice(0, 0)){
            case 0:break;
        }
    }
}