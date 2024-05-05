using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1
{
    public enum SkillType
    {
        AttackSkills = 0,

        SupportSkills = 1,
    }
    public enum SkillRangeType
    {
        DirectDamage = 0,

        AreaOfEffect = 1,
    }
    public class Skill
    {
        public string SkillName { get; }
        public string SkillInfo { get; }
        public SkillType SkillType { get; }
        public static int SkillLevel { get; set; }
        public int SkillMp { get; }
        public int Damage { get; }
        public SkillRangeType SkillRangeType { get; set; }

        public Skill(string skillName, string skillInfo, SkillType skillType, int skillLevel, int skillMp, int damage, SkillRangeType skillRangeType)
        {
            SkillName = skillName;
            SkillInfo = skillInfo;
            SkillType = skillType;
            SkillLevel = skillLevel;
            SkillMp = skillMp;
            SkillRangeType = skillRangeType;
            Damage = damage;
            damage = SkillDamage(damage);
        }

        public static void UseSkill(int random)
        {
            int cho1 = ConsoleUtility.PromotMenuChoice(1, GameManager.Instance.skills.Count);

            int i = cho1 - 1;

            if (GameManager.Instance.player.CurrentMp >= GameManager.Instance.skills[i].SkillMp)
            {
                GameManager.Instance.player.CurrentMp -= GameManager.Instance.skills[i].SkillMp;

                if (GameManager.Instance.skills[i].SkillRangeType == SkillRangeType.DirectDamage)
                {
                    //단일기
                    Console.WriteLine("스킬의 대상을 선택해주세요.");
                    int cho2 = GameManager.Instance.GetPlayerChoice(random);
                    int selectedMonsterIndex = cho2 - 1;

                    //이미 죽은 몬스터 구분
                    GameManager.Instance.ValidateMonsterChoice(selectedMonsterIndex);

                    Monster selectedMonster = GameManager.Instance.battleMonster[selectedMonsterIndex];

                    PlayerSkill(GameManager.Instance.skills[i], selectedMonster);
                }
                else if (GameManager.Instance.skills[i].SkillRangeType == SkillRangeType.AreaOfEffect)
                {
                    //광역기
                    for (int j = 0; j < GameManager.Instance.battleMonster.Count; j++)
                    {
                        PlayerSkill(GameManager.Instance.skills[i], GameManager.Instance.battleMonster[j]);
                    }
                }
                else
                {
                    //도트뎀기 근데 이건 만들려나 모르겠네
                }
            }
            else
            {
                Console.WriteLine("마나가 부족합니다.");
                Console.ReadKey();
            }
        }

        public static int SkillDamage(int damage)
        {
            //데미지 계산 식
            damage = SkillLevel * 10 + damage;
            return damage;
        }
        public List<Skill> skills;

        internal void PrintSkillStatDescription(int idx)
        {
            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{idx + 1}");
            Console.ResetColor();
            Console.Write("] ");
            Console.Write(ConsoleUtility.PadRightForMixedText(SkillName, 12));
            Console.Write(" | ");
            Console.Write($"데미지 {(Damage >= 0 ? ": " : "")}{ConsoleUtility.PadRightForMixedText(SkillDamage(Damage).ToString(), 4)}");
            Console.Write(" | ");
            Console.Write($"소비 마나 {(SkillMp >= 0 ? ": " : "")}{ConsoleUtility.PadRightForMixedText(SkillMp.ToString(), 4)}");
            Console.Write(" | ");
            if (SkillRangeType == SkillRangeType.DirectDamage)
            {
                Console.Write($"대상: {ConsoleUtility.PadRightForMixedText("단일", 4)}");
            }
            else if (SkillRangeType == SkillRangeType.AreaOfEffect)
            {
                Console.Write($"대상: {ConsoleUtility.PadRightForMixedText("범위", 4)}");
            }

            Console.Write(" | ");
            Console.WriteLine(SkillInfo);
        }
        private static void PlayerSkill(Skill selectSkill, Monster selectedMonster)
        {
            Console.WriteLine();
            ConsoleUtility.PrintTextHighlights("", $"{GameManager.Instance.player.Name}의 {selectSkill.SkillName}!");

            if (GameManager.Instance.battleRandom.Next(100) < 10)
            {
                Console.WriteLine("회피!");
                Console.WriteLine($"몬스터 {selectedMonster.Name}이(가) 공격을 회피했습니다.");
            }
            else
            {
                int attackDamage = SkillDamage(selectSkill.Damage);
                if (GameManager.Instance.IsCriticalHit())
                    GameManager.Instance.InflictCriticalDamage(selectedMonster, attackDamage);
                else
                    GameManager.Instance.InflictRegularDamage(selectedMonster, attackDamage);

                if (!selectedMonster.IsAlive())
                {
                    GameManager.Instance.HandleMonsterDefeat(selectedMonster);
                }
            }
            Thread.Sleep(1000);
        }
    }
}
