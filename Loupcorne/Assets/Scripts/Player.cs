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

    [SerializeField] private float displacementSpeed = 10f;
    [SerializeField] private float animationSpeed = 2f;



    private readonly List<Skill> skills = new List<Skill>();
    private int activeSkill = 0;

	void Start () 
    {
        _state = PlayerState.IDLE;

        animation["idle"].speed = animationSpeed;
        animation["run"].speed = animationSpeed;

        // Apply simulation object.
        this.SetSimObject(LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimObject>().GetElement("King"));

        // Add all skills to skill list.
        LoupCorne.Framework.IDatatable<SkillEffectElement> skillDatatable = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>();
        List<string> skillEffectEllementNames = skillDatatable.GetElements().Select(e => e.Name).ToList();
        skillEffectEllementNames.ForEach(n => this.skills.Add(new Skill() { SkillEffectName = n }));

        // Apply all skill's simulation descriptors.
        LoupCorne.Framework.IDatatable<LoupCorne.Framework.SimDescriptor> descriptorDatatable = LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimDescriptor>();
        foreach (Skill s in this.skills)
        {
            string descriptorName = skillDatatable.GetElement(s.SkillEffectName).Descriptor;
            if (!string.IsNullOrEmpty(descriptorName))
            {
                LoupCorne.Framework.SimDescriptor descriptor = descriptorDatatable.GetElement(descriptorName);
                this.AddDescriptor(descriptor);
            }
        }

        this.Refresh();
	}

    void OnGUI()
    {
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
        GUI.Label(new Rect(10, 10, 200, 25), string.Format("ActiveSkill={0}", this.skills[this.activeSkill].SkillEffectName));

        GUI.Box(new Rect(10, 40, 200, 200), string.Empty);
        GUI.Label(new Rect(10, 40, 200, 200), debugInfo);
#endif
    }

	void Update () 
    {
        //Player Rotation
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
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
                inputs *= displacementSpeed * Time.deltaTime;
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
                this.activeSkill = this.skills.Count - 1;
            }
            else if (this.activeSkill > this.skills.Count - 1)
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
                if (hit.collider.gameObject.name == "Ground")
                {
                    Vector3 worldMousePos = new Vector3(hit.point.x, 1, hit.point.z);
                    this.skills[this.activeSkill].Cast(this, worldMousePos);
                }
            }
        }
    }
}
