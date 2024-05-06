using System.Diagnostics;

public enum PotionEffect
{
    Heal,
    Mpup,
    IncreaseAttack,
    IncreaseDefense
}

public class Potion
{
    public string Name { get; }
    public string Desc { get; }
    public PotionEffect Effect { get; }
    public int HealValue { get; }
    public int ManaValue { get; private set; }

    public int Price { get; }
    public bool Isused { get; private set; }
    public bool IsPurchased { get; private set; } //구매여부
    public int Count { get; private set; }

    public Potion(string name, string desc, PotionEffect effect, int healValue, int manaValue, int price, bool isused = false, bool isPurchased = false, int count = 0)
    {
        Name = name;
        Desc = desc;
        Effect = effect;
        HealValue = healValue;
        ManaValue = manaValue;
        Price = price;
        Isused = isused;
        IsPurchased = isPurchased;
        Count = count;
    }

    public Potion ClonePotion()
    {
        Potion clone = new Potion(Name, Desc, Effect, HealValue, ManaValue, Price);
        return clone;
    }

    internal void PrintPotionStatDescription(bool sell = false, bool withNumber = false, int idx = 0, bool withPrice = false)
    {
        Console.Write("- ");
        if (withNumber)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{idx}");
            Console.ResetColor();
        }

        Console.Write(ConsoleUtility.PadRightForMixedText(Name, 19)); //[E]가 3칸이기때문에 3칸추가

        Console.Write(" | ");

        if (HealValue != 0) Console.Write($"체력 {(HealValue >= 0 ? "+" : "")}{ConsoleUtility.PadRightForMixedText(HealValue.ToString(), 6)}");
        if (ManaValue != 0) Console.Write($"마나 {(ManaValue >= 0 ? "+" : "")}{ConsoleUtility.PadRightForMixedText(ManaValue.ToString(), 6)}");

        Console.Write(" | ");

        Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 50));

        Console.Write(" | ");

        if (withPrice)
        {
            Console.Write(Count);

            Console.Write(" | ");
            if (sell)
            {
                ConsoleUtility.PrintTextHighlights("", (4 * Price / 5).ToString(), " G");
            }
            else
            {
                ConsoleUtility.PrintTextHighlights("", Price.ToString(), " G");
            }
        }
        else 
        {
            ConsoleUtility.PrintTextHighlights("", Count.ToString(), " 개");
        }
    }

    internal void ToggleusedStates()
    {
        DecreaseCount();
    }

    internal void PrintStorePotionDescription(bool withNumber = false, int idx = 0)
    {
        Console.Write("- ");
        if (withNumber)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{idx}");
            Console.ResetColor();
            Console.Write(" ");
        }

        Console.Write(ConsoleUtility.PadRightForMixedText(Name, 19));

        Console.Write(" | ");

        if (HealValue != 0) Console.Write($"체력 {(HealValue >= 0 ? "+" : "")}{ConsoleUtility.PadRightForMixedText(HealValue.ToString(), 6)}");
        if (ManaValue != 0) Console.Write($"마나 {(ManaValue >= 0 ? "+" : "")}{ConsoleUtility.PadRightForMixedText(ManaValue.ToString(), 6)}");

        Console.Write(" | ");

        Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 50));

        Console.Write(" | ");

        ConsoleUtility.PrintTextHighlights("", Price.ToString(), " G");

        //if (IsPurchased)
        //{
        //    IncreaseCount();
        //    TogglePurchase();
        //}
    }

    internal void TogglePurchase()
    {
        IncreaseCount();
    }

    internal void IncreaseCount(int amount = 1)
    {
        Count += amount;
    }

    internal void DecreaseCount(int amount = 1)
    {
        Count -= amount;
    }

    internal void ApplyEffect(Player player)
    {
        switch (Effect)
        {
            case PotionEffect.Heal:
                player.Heal(HealValue);
                Console.WriteLine($"{player.Name}이(가) {HealValue}만큼 체력을 회복했습니다!");
                break;
            case PotionEffect.Mpup:
                player.ManaHeal(ManaValue);
                Console.WriteLine($"{player.Name}이(가) {ManaValue}만큼 마나를 회복했습니다!");
                break;
            // 다른 효과들도 필요한 경우 여기에 추가
            default:
                break;
        }
    }
}