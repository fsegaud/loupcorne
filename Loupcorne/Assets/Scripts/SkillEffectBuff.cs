using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SkillEffectBuff : SkillEffect
{
    private UnitsManager unitManager;
    private float startTime;
    private string buffDescriptorName;

    void Awake()
    {
        this.unitManager = UnitsManager.Instance;
    }

    void Start()
    {
        this.startTime = Time.time;

        // Apply buff descriptor.
        LoupCorne.Framework.IDatatable<LoupCorne.Framework.SimDescriptor> descriptorDatatable = LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimDescriptor>();
        LoupCorne.Framework.SimDescriptor buffDescriptor = descriptorDatatable.GetElement(this.SkillEffectElement.BuffDescriptor);
        this.buffDescriptorName = buffDescriptor.Name;
        this.unitManager.player.AddDescriptor(buffDescriptor);
        this.unitManager.player.Refresh();

        if (!string.IsNullOrEmpty(this.SkillEffectElement.Gfx))
        {
            GameObject go = GameObject.Instantiate(Resources.Load(this.SkillEffectElement.Gfx), this.transform.position, this.transform.rotation) as GameObject;
            go.transform.parent = this.unitManager.player.gameObject.transform;
        }

        if (!string.IsNullOrEmpty(this.SkillEffectElement.Sfx))
        {
            GameObject.Instantiate(Resources.Load(this.SkillEffectElement.Sfx), this.transform.position, this.transform.rotation);
        }

        // Hook this to player.
        this.transform.parent = this.unitManager.player.gameObject.transform;
        this.transform.localPosition = Vector3.up * .1f;
    }

    void Update()
    {
        // Check for elapsed timer.
        if (Time.time >= this.startTime + this.SkillEffectElement.Duration)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        this.unitManager.player.RemoveDescriptorByName(this.buffDescriptorName);
        this.unitManager.player.Refresh();
    }
}