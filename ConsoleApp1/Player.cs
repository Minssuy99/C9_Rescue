using System.Numerics;

internal class Player
{
    public string Name { get; set; }
    public string Job { get; set; }
    public int Level { get; private set; }
    public float Atk { get; set; }
    public int Def { get; set; }
    public int MaxHp { get; private set; }
    public int CurrentHp { get; private set; }
    public int MaxExp { get; private set; }
    public int CurrentExp { get; private set; }
    public int Gold { get; set; }
    public bool InTower {  get; set; }
    public int NowDongeon { get; set; }
    public Player(string name, string job, int level, float atk, int def, int maxHp, int currentHp, int maxExp, int currentExp, int gold)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        MaxHp = maxHp;
        CurrentHp = currentHp;
        MaxExp = maxExp;
        CurrentExp = currentExp;
        Gold = gold;
        NowDongeon = 1;
    }

    public void PlayerTakeDamage(int damage)
    {
        Random rand = new();
        int dmg;
        if (damage - Def < 0)
            dmg = 0;
        else
            dmg=damage- Def;
        CurrentHp -= rand.Next(dmg,damage+1);
        if (CurrentHp < 0)
            CurrentHp = 0;
    }

    public bool IsAlive()
    {
        return CurrentHp > 0;
    }

    public void playerdefeat()
    {
        CurrentHp = 1;
    }

    public void Rest()
    {
        CurrentHp = MaxHp;
    }
    public void GetExp(int exp)
    {
        CurrentExp += exp;
    }
    public void LevelUp()
    {
        if (CurrentExp >= MaxExp)
        {
            Console.WriteLine("레벨 업!");
            Level++;
            MaxExp = (int)Math.Pow(Level, 3) + 30;
            Atk = Atk + 0.5f;
            Def++;
        }
    }
    public Player PlayerCreate(Player player)
    {
        
        Console.Clear();
        Console.WriteLine("스크립트 스킵 [K]");
        ConsoleUtility.MakeSentence("\n당신을 환영합니다!\n당신의 이름은 무엇인가요?");
        ConsoleUtility.MakeSentence("\n");
        player.Name = Console.ReadLine();
        ConsoleUtility.MakeSentence($"\n그렇군요.{player.Name}, 만나서 반갑습니다.\n당신의 직업은 무엇인가요?\n\n1. 전사\n2. 기사\n3. 궁수\n\n");
        int choice = ConsoleUtility.PromotMenuChoice(1, 3);

        switch (choice)
        {
            case 1:
                player.Job = "전사";
                break;
            case 2:
                player.Job = "기사";
                break;
            case 3:
                player.Job = "궁수";
                break;
        }

        ConsoleUtility.MakeSentence("\n\n그렇군요. 감사합니다.\n이제 당신이 누구인지 알겠습니다.\n그러면 모험을 떠나볼까요?");
        Console.ReadKey();
        return player;
    }

    public void Heal(int healAmount)
    {
        CurrentHp += healAmount;

        // 체력이 최대 체력을 초과하지 않도록 조정
        if (CurrentHp > MaxHp)
        {
            CurrentHp = MaxHp;
        }
    }
}