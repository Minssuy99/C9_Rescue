using System.Numerics;

public class Player
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
    }

    public void PlayerTakeDamage(int damage)
    {
        CurrentHp -= damage;
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
        ConsoleUtility.MakeSentence("\n당신을 환영합니다!");
        ConsoleUtility.MakeSentence("\n");
        ConsoleUtility.MakeSentence("\n당신의 이름은 무엇인가요?");
        ConsoleUtility.MakeSentence("\n");
        player.Name = Console.ReadLine();
        ConsoleUtility.MakeSentence($"\n그렇군요.{player.Name}, 만나서 반갑습니다.");
        ConsoleUtility.MakeSentence("\n당신의 직업은 무엇인가요?");
        ConsoleUtility.MakeSentence("\n");
        ConsoleUtility.MakeSentence("\n1. 전사");
        ConsoleUtility.MakeSentence("\n2. 기사");
        ConsoleUtility.MakeSentence("\n3. 궁수");
        ConsoleUtility.MakeSentence("\n");
        ConsoleUtility.MakeSentence("\n");
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

        ConsoleUtility.MakeSentence("\n");
        ConsoleUtility.MakeSentence("\n그렇군요. 감사합니다.");
        ConsoleUtility.MakeSentence("\n이제 당신이 누구인지 알겠습니다.");
        ConsoleUtility.MakeSentence("\n그러면 모험을 떠나볼까요?");
        Console.ReadKey();
        return player;
    }
}