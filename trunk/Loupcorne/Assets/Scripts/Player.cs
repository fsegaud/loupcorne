using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : Entity 
{
    enum PlayerState
    {
        IDLE,
        MOVING
    }
    private PlayerState _state;

    //private readonly List<Skill> lockedSkills = new List<Skill>();
    //private readonly List<Skill> skills = new List<Skill>();
    private int activeSkill = 0;

	protected override void Start () 
    {
        base.Start();

        _state = PlayerState.IDLE;

        // Apply simulation object.
        this.SetSimObject(LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimObject>().GetElement("King"));

        //// Add all skills to locked skill list.
        //LoupCorne.Framework.IDatatable<SkillEffectElement> skillDatatable = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>();
        //List<string> skillEffectEllementNames = skillDatatable.GetElements().Select(e => e.Name).ToList();
        //skillEffectEllementNames.ForEach(n => this.lockedSkills.Add(new Skill() { SkillEffectName = n }));

        //// For now, all skills are considered unlocked.
        //this.skills.AddRange(this.lockedSkills);

        //// Apply all skill's simulation descriptors.
        //LoupCorne.Framework.IDatatable<LoupCorne.Framework.SimDescriptor> descriptorDatatable = LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimDescriptor>();
        //foreach (Skill s in this.skills)
        //{
        //    string descriptorName = skillDatatable.GetElement(s.SkillEffectName).Descriptor;
        //    if (!string.IsNullOrEmpty(descriptorName))
        //    {
        //        LoupCorne.Framework.SimDescriptor descriptor = descriptorDatatable.GetElement(descriptorName);
        //        this.AddDescriptor(descriptor);
        //    }
        //}

        SkillManager.OnSkillUnlocked += this.SkillManager_OnSkillUnlocked;
        SkillManager.Instance.UnlockSkill(1, Skill.Alignment.Test);
        SkillManager.Instance.UnlockSkill(2, Skill.Alignment.Test);
        SkillManager.Instance.UnlockSkill(3, Skill.Alignment.Test);

        this.Refresh();
	}

    void Destroy()
    {
        SkillManager.OnSkillUnlocked -= this.SkillManager_OnSkillUnlocked;
    }

    protected override void OnGUI()
    {
        base.OnGUI();

#if UNITY_EDITOR
        string debugInfo = string.Format(
            "MaxHealth={0}\nAtk={1}, Def={2}\nSpeed={3}\nCDR={4}, DmgR={5}\nAlignement={6}\n{7}",
            this.GetPropertyValue(SimProperties.MaxHealth),
            this.GetPropertyValue(SimProperties.Attack),
            this.GetPropertyValue(SimProperties.Defence),
            this.GetPropertyValue(SimProperties.Speed),
            this.GetPropertyValue(SimProperties.CooldownReductionRatio),
            this.GetPropertyValue(SimProperties.DamageReductionRatio),
            this.GetPropertyValue(SimProperties.Alignement),
            string.Join("\n", this.GetTags()));

        GUI.Box(new Rect(10, 10, 200, 25), string.Empty);
        GUI.Label(new Rect(10, 10, 200, 25), string.Format("ActiveSkill={0}", SkillManager.Instance.Skills[this.activeSkill].SkillEffectName));

        GUI.Box(new Rect(10, 40, 200, 200), string.Empty);
        GUI.Label(new Rect(10, 40, 200, 200), debugInfo);
#endif
    }

	void Update () 
    {
        //Animation Speed
       // animation["idle"].speed = (float)this.GetPropertyValue(SimProperties.Speed) / 5f;
        animation["run"].speed = (float)this.GetPropertyValue(SimProperties.Speed) / 5f;

        //Player Rotation
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
       foreach(RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                Vector3 worldMousePos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                transform.LookAt(worldMousePos);
            }
        }

        if (Input.GetAxis("Vertical") > 0.2f || Input.GetAxis("Vertical") < -0.2f || Input.GetAxis("Horizontal") > 0.2f || Input.GetAxis("Horizontal") < -0.2f)
            _state = PlayerState.MOVING;
        else
            _state = PlayerState.IDLE;
  
        switch (_state)
        { 
            case PlayerState.IDLE:
                animation.CrossFade("idle"); 
                break;
            case PlayerState.MOVING:
                animation.CrossFade("run");
                Vector3 inputs = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                inputs.Normalize();
                inputs *= (float)this.GetPropertyValue(SimProperties.Speed) * Time.deltaTime;
                transform.position += inputs; 
                break;
        }

        UpdateSkills();
	
	}

    void UpdateSkills()
    {
        float scrollWheelOffset = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelOffset != 0)
        {
            this.activeSkill += scrollWheelOffset > 0 ? 1 : -1;

            if (this.activeSkill < 0)
            {
                this.activeSkill = SkillManager.Instance.Skills.Count - 1;
            }
            else if (this.activeSkill > SkillManager.Instance.Skills.Count - 1)
            {
                this.activeSkill = 0;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    Vector3 worldMousePos = new Vector3(hit.point.x, 1, hit.point.z);
                    SkillManager.Instance.Skills[this.activeSkill].Cast(this, worldMousePos);
                }
            }
        }
    }

    private void SkillManager_OnSkillUnlocked(Skill skill)
    {
        LoupCorne.Framework.IDatatable<SkillEffectElement> skillDatatable = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>();
        LoupCorne.Framework.IDatatable<LoupCorne.Framework.SimDescriptor> descriptorDatatable = LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimDescriptor>();

        string descriptorName = skillDatatable.GetElement(skill.SkillEffectName).Descriptor;
        if (!string.IsNullOrEmpty(descriptorName))
        {
            LoupCorne.Framework.SimDescriptor descriptor = descriptorDatatable.GetElement(descriptorName);
            this.AddDescriptor(descriptor);
            this.Refresh();
        }
    }
}
