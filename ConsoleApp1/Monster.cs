

internal class Monster
{
    public string Name { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int MaxHp { get; }
    public int CurrentHp { get; private set; }
    public bool IsDead { get; private set; }

    public Monster(string name, int level, int atk, int def, int maxHp, int currentHp, bool isdead = false)
    {
        Name = name;
        Level = level;
        Atk = atk;
        Def = def;
        MaxHp = maxHp;
        CurrentHp = currentHp;
        IsDead = isdead;
    }
    }

    public void MonterTakeDamage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp < 0)
            CurrentHp = 0;
    }

    public bool IsAlive()
    {
        return CurrentHp > 0;
    }

    public void Reset()
    {
        CurrentHp = MaxHp;
    }
}