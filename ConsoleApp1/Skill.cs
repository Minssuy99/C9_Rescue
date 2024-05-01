using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
    internal class Skill
    {
        public string SkillName { get; }
        public string SkillInfo { get; }
        public SkillType SkillType { get; }
        public int SkillLevel { get; }

        public SkillRangeType SkillRangeType { get; }

        public Skill(string skillName,string skillInfo,SkillType skillType,int skillLevel,SkillRangeType skillRangeType)
        {
            SkillName = skillName;
            SkillInfo = skillInfo;
            SkillType = skillType;
            SkillLevel = skillLevel;
            SkillRangeType = skillRangeType;
        }
        public void SkillRange(SkillRangeType skillRangeType , Monster selectedMonster)
        {
            int i = (int)skillRangeType;

            if (i % 3 == 0) 
            {
                //단일기
                selectedMonster.CurrentHp -= 0;
                Console.WriteLine($"{selectedMonster.Name}에게 데미지!");
            }
            else if (i % 3 == 1)
            {
                //광역기
                for (int j = 0; j < GameManager.Instance.battleMonster.Count; j++)
                {
                    selectedMonster.CurrentHp -= 0;
                    Console.WriteLine($"{selectedMonster.Name}에게 데미지!");
                }
            }
            else
            {
                //도트기 근데 이건 만들려나 모르겠네
            }
        }

        public void SkillDamage()
        {
            //데미지 계산 식
        }
        public List<Skill> skills;
    }
}
