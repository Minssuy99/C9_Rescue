using System.Numerics;

internal class Player
{
    public string Name { get; }
    public string Job { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int MaxHp { get; }
    public int CurrentHp { get; private set; }
    public int Gold { get; set; }

    public Player(string name, string job, int level, int atk, int def, int maxHp, int currentHp, int gold)
    {
        Name = name;
        Job = job;
        Level = level;
        Atk = atk;
        Def = def;
        MaxHp = maxHp;
        CurrentHp = currentHp;
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

    public void Reset()
    {
        CurrentHp = MaxHp;
    }
}