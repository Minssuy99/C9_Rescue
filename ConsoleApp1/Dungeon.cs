using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class Dungeon
    {
        public string Name;
        public int Def;
        public int Gold;
        public int MaxMonster;
        public int Count;
        public Dungeon(int count,  string name, int def, int MaxMonster, int gold)
        {
            this.Count = count;
            this.Name = name;
            this.Def = def;
            this.MaxMonster = MaxMonster;
            this.Gold = gold;
        }
        public void PrintDungeon()
        {
            //던전의 종류를 프린트해준다.
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{Count}. ");
            Console.ResetColor();
            Console.Write($"{Name}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("|");
            Console.ResetColor();
            Console.Write("방어력");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" {Def} ");
            Console.ResetColor();
            Console.WriteLine("이상 권장");
        }
    }
}
