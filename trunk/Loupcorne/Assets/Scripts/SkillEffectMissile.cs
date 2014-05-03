using UnityEngine;
using System.Collections;
using System.Linq;

public class SkillEffectMissile : SkillEffect
{
    void Start()
    {
        this.SkillEffectElement = LoupCorne.Framework.Database.Instance.GetDatatable<SkillEffectElement>().GetElement("MissileTest");
    }

	void Update ()
    {
        //this.transform.Translate(this.transform.forward * Time.deltaTime * this.SkillEffectElement.Speed);
        this.transform.position += (this.transform.forward * Time.deltaTime * this.SkillEffectElement.Speed);
	}

    void OnTriggerEnter(Collider other)
    {
        Entity entity = other.gameObject.GetComponent<Entity>();
        Debug.Log(other.name);

        bool success = false;
        if (entity != null)
        {
            if (entity is Player)
            {
                // Ignore collision with player.
                return;
            }

            // Check collision tags.
            System.Type type = entity.GetType();
            if (this.SkillEffectElement.Tags.Split(',').Contains(type.Name))
            {
                entity.Hit(this.SkillEffectElement.Damage);
                success = true;

                if (!string.IsNullOrEmpty(this.SkillEffectElement.Gfx))
                {
                    GameObject.Instantiate(Resources.Load(this.SkillEffectElement.Gfx), this.transform.position, this.transform.rotation);
                }

                if(!string.IsNullOrEmpty(this.SkillEffectElement.Sfx))
                {
                    /* TODO: Play sfx. */
                }
            }
        }

        // Check if we need to destroy the missile.
        if((success && this.SkillEffectElement.DestroyOnHitSuccess)
            || (!success && this.SkillEffectElement.DestroyOnHitFailure))
        {
            Debug.Log("DESTROY");
            GameObject.Destroy(this.gameObject);
        }
    }
}
