
internal class Player
{
    public string Name { get; }
    public string Job { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int Hp { get; set;}
    public int DefultHp { get; set; }
    public int Gold { get; set; }

    public Player(string name, string job, int level, int atk, int def, int hp, int gold)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        Hp = hp;
        Gold = gold;
        DefultHp = hp;
    }

    internal void PlayerPhase(Monster monster)
    {
        Console.Clear();
        Random rand = new();
        int difference = (Atk * 10 / 100)+1;
        int nowAttack = rand.Next(Atk - difference, Atk + difference + 1);
        ConsoleUtility.PrintTextHighlights("", "Battle!");
        Console.WriteLine("\n\n");
        Console.WriteLine($"{Name} 의 공격!");
        ConsoleUtility.PrintTextHighlights("Lv.", monster.Level.ToString(), $" {monster.Name} 을(를) 맞췄습니다!");
        Console.WriteLine();
        ConsoleUtility.PrintTextHighlights("[데미지 : ", monster.Atk.ToString(), "]");
        Console.WriteLine("\n");
        ConsoleUtility.PrintTextHighlights("Lv.", monster.Level.ToString(), $" {monster.Name}");
        Console.WriteLine();
        ConsoleUtility.PrintTextHighlights("HP ", $"{monster.Hp} -> ");
        if (monster.Hp > 0)
        {
            monster.Hp -= nowAttack;
            Console.WriteLine(monster.Hp);
        }
        if (monster.Hp <= 0)
        {
            Console.WriteLine("Dead");
            monster.IsLive = false;
        }
        ConsoleUtility.PrintTextHighlights("", "0. ", "다음"); 
        Console.WriteLine();
    }

    public void HpPotion(int potion)
    {
        //HP회복 기능 칸
        Hp = potion;
        DefultHp = potion;
    }
}