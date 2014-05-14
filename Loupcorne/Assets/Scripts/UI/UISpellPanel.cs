using UnityEngine;
using System.Collections.Generic;

public class UISpellPanel : UIPanel
{
    private Dictionary<string, Texture> skillTextures = new Dictionary<string, Texture>();

    protected override void Draw()
    {
        base.Draw();

        int activeSkill = UnitsManager.Instance.player.ActiveSkill;
        List<Skill> skills = SkillManager.Instance.Skills;

        Skill skillTooltipToShow = null;

        GUI.BeginGroup(new Rect(Screen.width * .5f - 188f, Screen.height - 94f, 376f, 94f));
        {
            // Background.
            GUI.Box(new Rect(0f, 0f, 376f, 94f), string.Empty, "background");

            // Spells.
            float x = 8f + 15f;
            for (int i = 0; i < skills.Count; ++i)
            {
                // Selected spell.
                if (i == activeSkill)
                {
                    GUI.Label(new Rect(x - 4f, 4f + 30f, 56f, 56f), string.Empty, "activespell");
                }

                // Spell button.
                if (GUI.Button(new Rect(x, 8f + 30f, 48f, 48f), this.skillTextures[skills[i].SkillEffectName]))
                {
                    UnitsManager.Instance.player.ActiveSkill = i;
                }

                Rect collision = new Rect(Screen.width * .5f - 188f + x, 0f, 48f, 48f);
                if (collision.Contains(Input.mousePosition))
                {
                    skillTooltipToShow = skills[i];
                }

                // Cooldown.
                if (SkillManager.Instance.Skills[i].Timer > 0f)
                {
                    float timer = SkillManager.Instance.Skills[i].Timer;
                    GUI.Label(new Rect(x, 8f + 30f, 48f, 48f), timer >= 1f ? timer.ToString("0") : timer.ToString(".0"), "cooldown");
                }

                x += 56f;
            }
        }
        GUI.EndGroup();

        if (skillTooltipToShow != null)
        {
            this.DrawTooltip(skillTooltipToShow);
        }
    }

    private void DrawTooltip(Skill skill)
    {
        SkillEffectElement element = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>().GetElement(skill.SkillEffectName);

        GUI.BeginGroup(new Rect(Screen.width * .5f - 400f, Screen.height - 10f - 100f, 200f, 100f));
        {
            GUI.Box(new Rect(0f, 0f, 200f, 100f), string.Empty, "background-tooltip");

            GUI.Label(new Rect(5f, 5f, 190f, 20f), element.Title, "title");
            GUI.Label(new Rect(5f, 25f, 190f, 60f), element.Description);
            GUI.Label(new Rect(5f, 75f, 190f, 20f), string.Format("Cooldown: {0:#.0} sec.", element.Cooldown));
        }
        GUI.EndGroup();
    }

    protected override void Bind()
    {
        base.Bind();

        SkillManager.OnSkillUnlocked += this.SkillManager_OnSkillUnlocked;
    }

    protected override void Unbind()
    {
        base.Unbind();

        SkillManager.OnSkillUnlocked -= this.SkillManager_OnSkillUnlocked;
    }

    private void SkillManager_OnSkillUnlocked(Skill skill)
    {
        // Load texture if not already done.
        if (!this.skillTextures.ContainsKey(skill.SkillEffectName))
        {
            SkillEffectElement element = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>().GetElement(skill.SkillEffectName);
            this.skillTextures.Add(element.Name, Resources.Load<Texture>(element.Texture) as Texture);
        }
    }
}
