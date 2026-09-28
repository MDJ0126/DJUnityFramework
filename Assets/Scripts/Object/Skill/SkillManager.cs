using System.Collections.Generic;

namespace Game
{
    public class SkillManager
    {
        public List<Skill> Skills { get; private set; } = new();

        /// <summary>
        /// 스킬 추가
        /// </summary>
        /// <param name="skill"></param>
        public void AddSkill(Skill skill)
        {
            Skills.Add(skill);
        }

        /// <summary>
        /// 스킬 사겢
        /// </summary>
        public void RemoveAtSkill(int index)
        {
            Skills.RemoveAt(index);
        }

        /// <summary>
        /// 스킬 가져오기
        /// </summary>
        /// <param name="index">스킬 인덱스</param>
        /// <returns></returns>
        public Skill GetSkill(int index)
        {
            if (Skills.Count > index)
            {
                return Skills[index];
            }
            else
            {
                return null;
            }
        }
    }
}
