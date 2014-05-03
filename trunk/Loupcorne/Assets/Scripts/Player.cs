using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Player : Entity
{
    private readonly List<Skill> skills = new List<Skill>();
    private int activeSkill = 0;

	void Start ()
    {
        // Add all skills to skill list.
        List<string> skillEffectEllementNames = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>().GetElements().Select(e => e.Name).ToList();
        skillEffectEllementNames.ForEach(n => this.skills.Add(new Skill() { SkillEffectName = n }));
	}

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), string.Format("ActiveSkill = {0}", this.activeSkill));
    }
	
	void Update ()
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
                    Debug.Log(worldMousePos);
                    this.skills[this.activeSkill].Cast(this, worldMousePos);
                }
            }
        }
	}
}
