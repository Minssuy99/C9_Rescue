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
            damage = SkillDamage(SkillRangeType,damage);
        }

        public static void UseSkill(int random)
        {
            //스킬 없으면 되돌려보내주는 부분
            if (GameManager.Instance.skills.Count == 0)
            {
                GameManager.Instance.StartBattle(random);
            }
            Console.WriteLine();
            Console.WriteLine("0. 뒤로 가기");
            //스킬 선택
            int cho1 = ConsoleUtility.PromotMenuChoice(0, GameManager.Instance.skills.Count);

            if (cho1 == 0)
            {
                GameManager.Instance.StartBattle(random);
            }

            int i = cho1 - 1;

            //스킬 없으면 돌아가게


            //마나 있는지 판정
            if (GameManager.Instance.player.CurrentMp >= GameManager.Instance.skills[i].SkillMp)
            {
                GameManager.Instance.player.CurrentMp -= GameManager.Instance.skills[i].SkillMp;

                //스킬 범위 판정
                if (GameManager.Instance.skills[i].SkillRangeType == SkillRangeType.DirectDamage)
                {
                    //단일기
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[몬스터 선택]"); //s2를 강조함
                    Console.ResetColor();

                    int cho2 = GameManager.Instance.GetPlayerChoice(random);
                    int selectedMonsterIndex = cho2 - 1;

                    //이미 죽은 몬스터 구분
                    if (!GameManager.Instance.ValidateMonsterChoice(selectedMonsterIndex))
                        UseSkill(random);

                    Monster selectedMonster = GameManager.Instance.battleMonster[selectedMonsterIndex];

                    PlayerSkill(GameManager.Instance.skills[i], selectedMonster);
                }
                else if (GameManager.Instance.skills[i].SkillRangeType == SkillRangeType.AreaOfEffect)
                {
                    //광역기
                    for (int j = 0; j < GameManager.Instance.battleMonster.Count; j++)
                    {
                        if (GameManager.Instance.battleMonster[j].IsAlive())
                        {
                            PlayerSkill(GameManager.Instance.skills[i], GameManager.Instance.battleMonster[j]);
                        }
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

        public static int SkillDamage(SkillRangeType skillRangeType,int damage)
        {
            //데미지 계산 식
            if(skillRangeType == SkillRangeType.AreaOfEffect)
            {
                damage = (SkillLevel * 10 + damage)/4;
            }
            else
            {
                damage = SkillLevel * 10 + damage;
            }
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
            Console.Write($"데미지 {(Damage >= 0 ? ": " : "")}{ConsoleUtility.PadRightForMixedText(SkillDamage(SkillRangeType, Damage).ToString(), 4)}");
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
                int attackDamage = SkillDamage(selectSkill.SkillRangeType, selectSkill.Damage);
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
