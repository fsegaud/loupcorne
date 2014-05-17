using UnityEngine;
using System.Collections;
using System.Linq;

public class SkillEffectMissile : SkillEffect
{
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
                entity.Hit(this.ComputeDamage(entity));
                success = true;
            }
        }

        // Check if we need to destroy the missile.
        if((success && this.SkillEffectElement.DestroyOnHitSuccess)
            || (!success && this.SkillEffectElement.DestroyOnHitFailure))
        {
            if (!string.IsNullOrEmpty(this.SkillEffectElement.Gfx))
            {
                GameObject.Instantiate(Resources.Load(this.SkillEffectElement.Gfx), this.transform.position, this.transform.rotation);
            }

            if (!string.IsNullOrEmpty(this.SkillEffectElement.Sfx))
            {
                GameObject.Instantiate(Resources.Load(this.SkillEffectElement.Sfx), this.transform.position, this.transform.rotation);
            }

            GameObject.Destroy(this.gameObject);
        }
    }
}
