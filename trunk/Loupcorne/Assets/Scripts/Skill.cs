using System;
using UnityEngine;

class Skill
{
    public string SkillEffectName
    {
        get;
        set;
    }

    public void Cast(Player player, UnityEngine.Vector3 target)
    {
        SkillEffectElement element = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>().GetElement(this.SkillEffectName);
        string typename = string.Format("SkillEffect{0}", element.Type);
        Type type = Type.GetType(typename);

        GameObject o = null;
        if (type == typeof(SkillEffectMissile))
        {
            Vector3 spawn = new Vector3(player.transform.position.x, 1, player.transform.position.z);
            Quaternion rotation = Quaternion.LookRotation(target - spawn, Vector3.up);
            o = (GameObject)GameObject.Instantiate(Resources.Load(element.Prefab), spawn, Quaternion.identity);
            o.transform.LookAt(target);
        }
        else if (type == typeof(SkillEffectAoe))
        {
            Vector3 spawn = new Vector3(target.x, 0f, target.z);
            o = (GameObject)GameObject.Instantiate(Resources.Load(element.Prefab), spawn, Quaternion.identity);
        }

        o.GetComponent<SkillEffect>().SkillEffectElement = element;
    }
}
