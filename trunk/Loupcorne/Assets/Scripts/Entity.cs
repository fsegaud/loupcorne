using UnityEngine;
using System.Collections;

public class Entity : SimObjectWrapper 
{
    [SerializeField] protected float _health;


	void Start () 
    {
	
	}
	
	void Update () 
    {
	    //TODO inputs ?
	}

	public virtual void Hit(float hitStrength)
	{
        _health -= hitStrength;
        Debug.Log("Aie !");
        if (_health <= 0)
            Kill();
    }

    public virtual void Kill()
    {}
}
