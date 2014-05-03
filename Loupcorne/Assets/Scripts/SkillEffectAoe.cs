using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class SkillEffectAoe : SkillEffect
{
    private UnitsManager unitManager;
    private float startTime;

    void Awake()
    {
        this.unitManager = UnitsManager.Instance;
    }

    void Start()
    {
        //TMP
        this.SkillEffectElement = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>().GetElement("AoeTest");

        this.startTime = Time.time;
        Debug.Log(string.Format("StartTime={0} Duration={1}", this.startTime, this.SkillEffectElement.Duration));

        if (!string.IsNullOrEmpty(this.SkillEffectElement.Gfx))
        {
            GameObject.Instantiate(Resources.Load(this.SkillEffectElement.Gfx), this.transform.position, this.transform.rotation);
        }

        if (!string.IsNullOrEmpty(this.SkillEffectElement.Sfx))
        {
            /* TODO: Play sfx. */
        }
    }

    void Update()
    {
        // Filter targets.
        string[] tags = this.SkillEffectElement.Tags.Split(',');
        List<Entity> targets = new List<Entity>();
        if (tags.Contains("Guard"))
        {
            targets.AddRange(this.unitManager.guards.ToArray());
        }
        if (tags.Contains("Peon"))
        {
            targets.AddRange(this.unitManager.peons.ToArray());
        }
        if (tags.Contains("King"))
        {
            targets.Add(this.unitManager.player);
        }

        // Do Damages.
        targets.Where(t => Vector3.Distance(transform.position, t.transform.position) <= this.SkillEffectElement.Radius).ToList()
            .ForEach(t => t.Hit(this.SkillEffectElement.Damage * Time.deltaTime));

        // Check for elapsed timer.
        if (Time.time >= this.startTime + this.SkillEffectElement.Duration)
        {
            Debug.Log("DESTROY AOE");
            GameObject.Destroy(this.gameObject);
        }
    }
}
