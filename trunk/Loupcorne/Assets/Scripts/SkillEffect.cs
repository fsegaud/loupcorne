using UnityEngine;
using System.Collections;

public class SkillEffect : MonoBehaviour
{
    public SkillEffectElement SkillEffectElement
    {
        get;
        set;
    }

    protected virtual float ComputeDamage(Entity opponent)
    {
        return this.SkillEffectElement.Damage * (float)UnitsManager.Instance.player.GetPropertyValue(SimProperties.Attack) * .3f
                    - (float)opponent.GetPropertyValue(SimProperties.Defence) * .1f 
                    + 2f;
    }
}
