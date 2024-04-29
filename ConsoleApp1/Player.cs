using System.Buffers.Text;

internal class Player
{
    public string Name { get; }
    public string Job { get; }
    public int Level { get; private set; }
    public int BaseAtk { get; private set; }
    public int BaseDef { get; private set; }
    public int BaseHp { get; private set; }
    public int Atk { get; private set; }
    public int Def { get; private set; }
    public int Hp { get; private set; }
    public int Gold { get; set; }

    public Player(string name, string job, int level, int baseAtk, int baseDef, int baseHp, int gold)
    {
        Name = name;
        Job = job;
        Level = level;
        BaseAtk = baseAtk;
        BaseDef = baseDef;
        BaseHp = baseHp;
        Atk = baseAtk;
        Def = baseDef;
        Hp = baseHp;
        Gold = gold;
    }

    public void UpdateStats(List<Item> inventory)
    {
        // 기본 능력치 초기화
        Atk = BaseAtk;
        Def = BaseDef;
        Hp = BaseHp;

        // 장착된 아이템에 따라 능력치 업데이트
        foreach (var item in inventory)
        {
            if (item.IsEquipped)
            {
                Atk += item.Atk;
                Def += item.Def;
                Hp += item.Hp;
            }
        }
    }
}


