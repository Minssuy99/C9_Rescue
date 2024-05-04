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

            GameManager.Instance.player.CurrentMp -=GameManager.Instance.skills[i].SkillMp;

            if (GameManager.Instance.skills[i].SkillRangeType == SkillRangeType.DirectDamage)
            {
                //단일기
                Console.WriteLine("스킬의 대상을 선택해주세요.");
                int cho2 = ConsoleUtility.PromotMenuChoice(1, random);
                int selectedMonsterIndex = cho2 - 1;

                //이미 죽은 몬스터 구분
                if (!GameManager.Instance.battleMonster[selectedMonsterIndex].IsAlive())
                {
                    Console.WriteLine("선택한 몬스터가 이미 죽었습니다. 다른 몬스터를 선택하세요.");
                    Console.ReadKey();
                }

                Monster selectedMonster = GameManager.Instance.battleMonster[selectedMonsterIndex];

                Console.WriteLine();
                ConsoleUtility.PrintTextHighlights("", $"{GameManager.Instance.player.Name}의 {GameManager.Instance.skills[i].SkillName}!");

                if (GameManager.Instance.battleRandom.Next(100) < 10)
                {
                    Console.WriteLine("회피!");
                    Console.WriteLine($"몬스터 {selectedMonster.Name}이(가) 공격을 회피했습니다.");
                }
                else
                {
                    if (GameManager.Instance.battleRandom.Next(100) < 15)
                    {
                        // 추가 효과가 발생한 경우
                        int criticalDamage = (int)(SkillDamage(GameManager.Instance.skills[i].Damage) * 1.6); // 160%의 데미지
                        Console.WriteLine("치명타 발생! 추가 데미지를 가합니다.");
                        Console.WriteLine($"[데미지 : {criticalDamage}]");
                        selectedMonster.MonterTakeDamage(criticalDamage);
                    }
                    else
                    {
                        // 추가 효과가 발생하지 않은 경우
                        Console.WriteLine($"[데미지 : {SkillDamage(GameManager.Instance.skills[i].Damage)}]");
                        selectedMonster.MonterTakeDamage(SkillDamage(GameManager.Instance.skills[i].Damage));
                    }
                    Console.WriteLine($"몬스터 {selectedMonster.Name}의 HP: {selectedMonster.CurrentHp}");

                    if (!selectedMonster.IsAlive())
                    {
                        Console.WriteLine($"몬스터 {selectedMonster.Name}를 물리쳤습니다!");
                        Console.WriteLine($"경험치 {selectedMonster.Exp}를 얻었습니다!");
                        GameManager.Instance.player.GetExp(selectedMonster.Exp);
                        GameManager.Instance.player.LevelUp();

                        GameManager.Instance.MonsterDied(selectedMonster);
                        ConsoleUtility.PrintTextHighlights("경험치 :", $"{GameManager.Instance.player.CurrentExp}/{GameManager.Instance.player.MaxExp}".ToString());
                    }
                }

                bool allMonstersDead = true;
                foreach (var monster in GameManager.Instance.battleMonster)
                {
                    if (monster.IsAlive())
                    {
                        allMonstersDead = false;
                        break;
                    }
                }

                //플레이어 승리
                if (allMonstersDead)
                {
                    GameManager.Instance.ShowResultMenu(random);
                }

                // 몬스터의 공격차례
                Thread.Sleep(1000);
                Console.WriteLine();
                Console.WriteLine("몬스터 차례");

                foreach (Monster m in GameManager.Instance.battleMonster)
                {
                    if (m.IsAlive())
                    {
                        if (GameManager.Instance.battleRandom.Next(100) < 10)
                        {
                            Console.WriteLine("회피!");
                            Console.WriteLine($"{GameManager.Instance.player.Name}이(가) 공격을 회피했습니다.");
                        }
                        else
                        {
                            ConsoleUtility.PrintTextHighlights("", $"{m.Name}이(가) 공격합니다!");

                            if (GameManager.Instance.battleRandom.Next(100) < 15)
                            {
                                // 추가 효과가 발생한 경우
                                int criticalDamage = (int)(m.Atk * 1.6); // 160%의 데미지
                                Console.WriteLine("치명타 발생! 추가 피해를 입었습니다.");
                                Console.WriteLine($"[데미지 : {criticalDamage}]");
                                GameManager.Instance.player.PlayerTakeDamage(criticalDamage);
                            }
                            else
                            {
                                Console.WriteLine($"[데미지 : {m.Atk}]");
                                GameManager.Instance.player.PlayerTakeDamage(m.Atk);
                            }

                            Console.WriteLine($"{GameManager.Instance.player.Name}의 HP: {GameManager.Instance.player.CurrentHp}");
                            Console.WriteLine();
                            Thread.Sleep(1000);
                        }
                    }
                }

                Console.WriteLine("아무키나 누르세요...");
                Console.ReadLine();

                //플레이어 패배
                if (!GameManager.Instance.player.IsAlive())
                {
                    Console.WriteLine("전투에서 패배했습니다.");
                    Console.WriteLine("마을로 돌아갑니다.");

                    foreach (var monster in GameManager.Instance.battleMonster)
                    {
                        monster.Reset();
                    }

                    GameManager.Instance.player.playerdefeat();

                    //골드값 0미만으로 떨어지지 않게 Player에서 조정한 후
                    //골드를 일정값 빼주는 메서드 작성

                    Thread.Sleep(1000);
                    Console.ReadKey();
                    GameManager.Instance.MainMenu();
                }
                GameManager.Instance.battleMonster[selectedMonsterIndex].CurrentHp -= SkillDamage(GameManager.Instance.skills[i].Damage);
                Console.WriteLine($"{GameManager.Instance.battleMonster[selectedMonsterIndex].Name}에게 데미지!");
            }
            else if (GameManager.Instance.skills[i].SkillRangeType == SkillRangeType.AreaOfEffect)
            {
                ConsoleUtility.PrintTextHighlights("", $"{GameManager.Instance.player.Name}의 {GameManager.Instance.skills[i].SkillName}!");
                //광역기
                for (int j = 0; j < GameManager.Instance.battleMonster.Count; j++)
                {
                    if (GameManager.Instance.battleRandom.Next(100) < 10)
                    {
                        Console.WriteLine("회피!");
                        Console.WriteLine($"몬스터 {GameManager.Instance.battleMonster[j].Name}이(가) 공격을 회피했습니다.");
                    }
                    else
                    {
                        if (GameManager.Instance.battleRandom.Next(100) < 15)
                        {
                            // 추가 효과가 발생한 경우
                            int criticalDamage = (int)(SkillDamage(GameManager.Instance.skills[i].Damage) * 1.6); // 160%의 데미지
                            Console.WriteLine("치명타 발생! 추가 데미지를 가합니다.");
                            Console.WriteLine($"[데미지 : {criticalDamage}]");
                            GameManager.Instance.battleMonster[j].MonterTakeDamage(criticalDamage);
                        }
                        else
                        {
                            // 추가 효과가 발생하지 않은 경우
                            Console.WriteLine($"[데미지 : {SkillDamage(GameManager.Instance.skills[i].Damage)}]");
                            GameManager.Instance.battleMonster[j].MonterTakeDamage(SkillDamage(GameManager.Instance.skills[i].Damage));
                        }
                        Console.WriteLine($"몬스터 {GameManager.Instance.battleMonster[j].Name}의 HP: {GameManager.Instance.battleMonster[j].CurrentHp}");
                    }
                }
                for (int k = 0;k< random;k++)
                {
                    if (!GameManager.Instance.battleMonster[k].IsAlive())
                    {
                        Console.WriteLine($"몬스터 {GameManager.Instance.battleMonster[k].Name}를 물리쳤습니다!");
                        Console.WriteLine($"경험치 {GameManager.Instance.battleMonster[k].Exp}를 얻었습니다!");
                        GameManager.Instance.player.GetExp(GameManager.Instance.battleMonster[k].Exp);
                        GameManager.Instance.player.LevelUp();

                        GameManager.Instance.MonsterDied(GameManager.Instance.battleMonster[k]);
                        ConsoleUtility.PrintTextHighlights("경험치 :", $"{GameManager.Instance.player.CurrentExp}/{GameManager.Instance.player.MaxExp}".ToString());
                    }
                }
                bool allMonstersDead = true;
                foreach (var monster in GameManager.Instance.battleMonster)
                {
                    if (monster.IsAlive())
                    {
                        allMonstersDead = false;
                        break;
                    }
                }

                //플레이어 승리
                if (allMonstersDead)
                {
                    GameManager.Instance.ShowResultMenu(random);
                }

                // 몬스터의 공격차례
                Thread.Sleep(1000);
                Console.WriteLine();
                Console.WriteLine("몬스터 차례");

                foreach (Monster m in GameManager.Instance.battleMonster)
                {
                    if (m.IsAlive())
                    {
                        if (GameManager.Instance.battleRandom.Next(100) < 10)
                        {
                            Console.WriteLine("회피!");
                            Console.WriteLine($"{GameManager.Instance.player.Name}이(가) 공격을 회피했습니다.");
                        }
                        else
                        {
                            ConsoleUtility.PrintTextHighlights("", $"{m.Name}이(가) 공격합니다!");

                            if (GameManager.Instance.battleRandom.Next(100) < 15)
                            {
                                // 추가 효과가 발생한 경우
                                int criticalDamage = (int)(m.Atk * 1.6); // 160%의 데미지
                                Console.WriteLine("치명타 발생! 추가 피해를 입었습니다.");
                                Console.WriteLine($"[데미지 : {criticalDamage}]");
                                GameManager.Instance.player.PlayerTakeDamage(criticalDamage);
                            }
                            else
                            {
                                Console.WriteLine($"[데미지 : {m.Atk}]");
                                GameManager.Instance.player.PlayerTakeDamage(m.Atk);
                            }

                            Console.WriteLine($"{GameManager.Instance.player.Name}의 HP: {GameManager.Instance.player.CurrentHp}");
                            Console.WriteLine();
                            Thread.Sleep(1000);
                        }
                    }
                }

                Console.WriteLine("아무키나 누르세요...");
                Console.ReadLine();

                //플레이어 패배
                if (!GameManager.Instance.player.IsAlive())
                {
                    Console.WriteLine("전투에서 패배했습니다.");
                    Console.WriteLine("마을로 돌아갑니다.");

                    foreach (var monster in GameManager.Instance.battleMonster)
                    {
                        monster.Reset();
                    }

                    GameManager.Instance.player.playerdefeat();

                    //골드값 0미만으로 떨어지지 않게 Player에서 조정한 후
                    //골드를 일정값 빼주는 메서드 작성

                    Thread.Sleep(1000);
                    Console.ReadKey();
                    GameManager.Instance.MainMenu();
                }
            }
            else
            {
                //도트뎀기 근데 이건 만들려나 모르겠네
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
    }
}
