

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

    public Monster(int level, string name, int hp, int atk, bool isLive)
    {
        this.Level = level;
        this.Name = name;
        this.Hp = hp;
        this.Atk = atk;
        this.IsLive = isLive;
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

    internal void MonsterPhase()
    {
        ConsoleUtility.PrintTextHighlights("", "Battle!");
        Console.WriteLine("\n");
        ConsoleUtility.PrintTextHighlights("Lv.",level.ToString());
        //Console.WriteLine($"{})
        ConsoleUtility.PrintTextHighlights("", "0. ", "다음");
        Console.ReadLine();
    }
}