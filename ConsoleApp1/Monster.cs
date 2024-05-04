

using System.Diagnostics;

public class Monster
{
    public string Name { get; }
    public int Level { get; }
    public int Atk { get; }
    public int Def { get; }
    public int MaxHp { get; }
    public float CurrentHp { get; set; }
    public int Exp { get; }
    public string DropItem { get; }
    public bool IsDead { get; private set; }

    public Monster(string name, int level, int atk, int def, int maxHp, float currentHp, int exp, string dropItem , bool isdead = false)
    {
        Name = name;
        Level = level;
        Atk = atk;
        Def = def;
        MaxHp = maxHp;
        CurrentHp = currentHp;
        Exp = exp;
        DropItem = dropItem;
        IsDead = isdead;
    }
    public Monster CloneMonster()
    {
        Monster clone = new Monster(Name, Level, Atk, Def, MaxHp, CurrentHp, Exp, DropItem, IsDead = false);
        return clone;
    }
    public void MonterTakeDamage(float damage)
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