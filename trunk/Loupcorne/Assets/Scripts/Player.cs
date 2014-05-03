using UnityEngine;
using System.Collections.Generic;

public class Player : Entity
{
    private List<Skill> skills;

	void Start ()
    {
        this.skills = new List<Skill>();
        this.skills.Add(new Skill() { SkillEffectName = "MissileTest" });
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.localPosition.z));
            //worldMousePos = new Vector3(worldMousePos.x, 1, worldMousePos.z + 10);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.name == "Ground")
                {
                    Vector3 worldMousePos = new Vector3(hit.point.x, 1, hit.point.z);
                    Debug.Log(worldMousePos);
                    this.skills[0].Cast(this, worldMousePos);
                }
            }
        }
	}
}
