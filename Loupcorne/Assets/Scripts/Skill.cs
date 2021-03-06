﻿using System;
using UnityEngine;

public class Skill
{
    public enum Alignment
    {
        Good = 0,
        Evil,
        Test
    }

    public string SkillEffectName
    {
        get;
        set;
    }

    private float cooldown = -1f;
    private float lastCast = -1000f;
    public float Timer
    {
        get
        {
            if (this.cooldown < 0f)
            {
                SkillEffectElement element = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>().GetElement(this.SkillEffectName);
                this.cooldown = element.Cooldown;
            }

            return Mathf.Clamp(this.lastCast - UnityEngine.Time.time + this.cooldown, 0f, float.MaxValue);
        }
    }

    public bool Cast(Player player, UnityEngine.Vector3 target)
    {
        if(this.Timer > float.Epsilon)
        {
            return false;
        }

        this.lastCast = UnityEngine.Time.time;

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
        else if (type == typeof(SkillEffectBuff))
        {
            o = (GameObject)GameObject.Instantiate(Resources.Load(element.Prefab), Vector3.zero, Quaternion.identity);
            o.transform.parent = player.transform;
            o.transform.localPosition = Vector3.zero;
        }

        o.GetComponent<SkillEffect>().SkillEffectElement = element;

        // Apply alignement change.
        double currentAlignement = player.GetPropertyValue(SimProperties.Alignement);
        player.SetPropertyBaseValue(SimProperties.Alignement, currentAlignement + element.AlignementGainOnUse);
        player.Refresh();

        return true;
    }
}
