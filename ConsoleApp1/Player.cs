using System.Numerics;

internal class Player
{
    public string Name { get; }
    public string Job { get; }
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
}