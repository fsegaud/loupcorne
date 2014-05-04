using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SkillManager
{
    private static SkillManager instance;

    public delegate void SkillUnlockedEventHandler(Skill skill);
    public static event SkillUnlockedEventHandler OnSkillUnlocked;

    private readonly List<Skill> lockedSkills = new List<Skill>();
    private readonly List<Skill> unlockedSkills = new List<Skill>();

    public static SkillManager Instance
    {
        get
        {
            return SkillManager.instance ?? (SkillManager.instance = new SkillManager());
        }
    }

    public List<Skill> Skills
    {
        get
        {
            return this.unlockedSkills;
        }
    }

    private SkillManager()
    {
        this.GenerateSkillList();
    }

    private void GenerateSkillList()
    {
        // Add all skills to locked skill list.
        LoupCorne.Framework.IDatatable<SkillEffectElement> skillDatatable = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>();
        List<string> skillEffectEllementNames = skillDatatable.GetElements().Select(e => e.Name).ToList();
        skillEffectEllementNames.ForEach(n => this.lockedSkills.Add(new Skill() { SkillEffectName = n }));

        // For now, all skills are considered unlocked.
        this.unlockedSkills.AddRange(this.lockedSkills);
        if (SkillManager.OnSkillUnlocked != null)
        {
            this.unlockedSkills.ForEach(s => SkillManager.OnSkillUnlocked(s));
        }
    }
}
