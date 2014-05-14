using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : Entity 
{
    enum PlayerState
    {
        IDLE,
        MOVING,
        DEAD
    }
    private PlayerState _state;

    //private readonly List<Skill> lockedSkills = new List<Skill>();
    //private readonly List<Skill> skills = new List<Skill>();

    public delegate void PlayerIsDeadCallback();
	public static event PlayerIsDeadCallback PlayerIsDead;

	public int ActiveSkill
	{
        get;
        set;
    }

	protected override void Start () 
    {
      

        _state = PlayerState.IDLE;

        

        // Apply simulation object.
        this.SetSimObject(LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimObject>().GetElement("King"));


        SkillManager.Instance.ClearSkills();
        SkillManager.OnSkillUnlocked += this.SkillManager_OnSkillUnlocked;
        SkillManager.Instance.UnlockSkill(1, Skill.Alignment.Evil);
        SkillManager.Instance.UnlockSkill(2, Skill.Alignment.Evil);
        SkillManager.Instance.UnlockSkill(3, Skill.Alignment.Evil);
        SkillManager.Instance.UnlockSkill(1, Skill.Alignment.Good);
        SkillManager.Instance.UnlockSkill(2, Skill.Alignment.Good);
        SkillManager.Instance.UnlockSkill(3, Skill.Alignment.Good);

        this.Refresh();

        base.Start();
	}

    void Destroy()
    {
        SkillManager.OnSkillUnlocked -= this.SkillManager_OnSkillUnlocked;
    }

    protected override void OnGUI()
    {
        base.OnGUI();

#if UNITY_EDITOR && FALSE
        string debugInfo = string.Format(
            "MaxHealth={0}\nAtk={1}, Def={2}\nSpeed={3}\nCDR={4}, DmgR={5}\nAlignement={6}\n-----\n{7}",
            this.GetPropertyValue(SimProperties.MaxHealth),
            this.GetPropertyValue(SimProperties.Attack),
            this.GetPropertyValue(SimProperties.Defence),
            this.GetPropertyValue(SimProperties.Speed),
            this.GetPropertyValue(SimProperties.CooldownReductionRatio),
            this.GetPropertyValue(SimProperties.DamageReductionRatio),
            this.GetPropertyValue(SimProperties.Alignement),
            string.Join("\n", this.GetTags()));

        GUI.Box(new Rect(10, 10, 200, 25), string.Empty);
        GUI.Label(new Rect(10, 10, 200, 25), string.Format("ActiveSkill={0} ({1})", SkillManager.Instance.Skills[this.activeSkill].SkillEffectName, SkillManager.Instance.Skills[this.activeSkill].Timer.ToString("0.0")));

        GUI.Box(new Rect(10, 40, 200, 175), string.Empty);
        GUI.Label(new Rect(10, 40, 200, 175), debugInfo);
#endif
    }

	void Update () 
    {
        if (_state != PlayerState.DEAD)
        {
            //Animation Speed
            animation["hit"].weight = 2;
            animation["hit"].speed = 2;
            animation["run"].speed = (float)this.GetPropertyValue(SimProperties.Speed) / 3f;

            //Player Rotation
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
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



            if (Input.GetMouseButtonDown(0))
            {
                animation.Play("hit");
                string sfxName = string.Format(@"Sfx/Sword{0}", Random.Range(1, 3));
                GameObject.Instantiate(Resources.Load(sfxName), this.transform.position, this.transform.rotation);

                Vector3 attackDirection = transform.forward; //* (float)this.GetPropertyValue(SimProperties.AttackRange);
                Vector3 attackOrigin = new Vector3(transform.position.x, 1, transform.position.z);
                Ray attacRay = new Ray(attackOrigin, attackDirection);
                RaycastHit attackHit;
                if (Physics.Raycast(attacRay, out attackHit, (float)this.GetPropertyValue(SimProperties.AttackRange)))
                {
                    if (attackHit.collider.gameObject.CompareTag("Guard") || attackHit.collider.gameObject.CompareTag("Peon"))
                    {
                        Entity target = attackHit.collider.GetComponent<Entity>();
                        float hitStrength = (float)this.GetPropertyValue(SimProperties.Attack) * 0.3f - (float)target.GetPropertyValue(SimProperties.Defence) * 0.1f;
                        target.Hit(hitStrength);
                    }
                }
            }
            UpdateSkills();
        }
  
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
            case PlayerState.DEAD:
                
                break;
        }
	}

    void UpdateSkills()
    {
        //Animation Speed
        animation["spell"].weight = 2;
        animation["spell"].speed = 3;

        float scrollWheelOffset = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheelOffset != 0)
        {
            this.ActiveSkill += scrollWheelOffset > 0 ? 1 : -1;

            if (this.ActiveSkill < 0)
            {
                this.ActiveSkill = SkillManager.Instance.Skills.Count - 1;
            }
            else if (this.ActiveSkill > SkillManager.Instance.Skills.Count - 1)
            {
                this.ActiveSkill = 0;
            }
        }

        // Web browser support
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.ActiveSkill--;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            this.ActiveSkill++;
        }
        if (this.ActiveSkill < 0)
        {
            this.ActiveSkill = SkillManager.Instance.Skills.Count - 1;
        }
        else if (this.ActiveSkill > SkillManager.Instance.Skills.Count - 1)
        {
            this.ActiveSkill = 0;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
        {
            animation.Play("spell");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    Vector3 worldMousePos = new Vector3(hit.point.x, 1, hit.point.z);
                    SkillManager.Instance.Skills[this.ActiveSkill].Cast(this, worldMousePos);
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

    public override void Kill()
    {
        base.Kill();
        _state = PlayerState.DEAD;
        animation.Play("death");
        PlayerIsDead();

    }
}
