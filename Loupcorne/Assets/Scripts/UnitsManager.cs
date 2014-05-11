using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsManager : Singleton<UnitsManager> 
{
	public Player player;

	public List<Guard> guards;

	public List<Peon> peons;

	public delegate void RemovePeonCallback(Peon p);
	public static event RemovePeonCallback PeonRemoved;

    public delegate void RemoveGuardCallback(Guard g);
    public static event RemoveGuardCallback GuardRemoved;

    public delegate void AllGuardRemovedCallback();
    public static event AllGuardRemovedCallback AllGuardRemoved;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		guards = new List<Guard>();
		peons = new List<Peon>();
	}

	void Start () 
	{
	
	}

	void Update () 
	{
	
	}

	public void RemovePeon(Peon p)
	{
        if (p != null)
        {
            if (peons.Contains(p))
            {
                peons.Remove(p);
                GameStats.nbPeonKilled++;
                if(PeonRemoved != null)
                    PeonRemoved(p);
            }
        }
	}

    public void RemoveGuard(Guard g)
    {
        if (g != null)
        {
            if (guards.Contains(g))
            {
                guards.Remove(g);
                if(GuardRemoved != null) 
                    GuardRemoved(g);
                if (guards.Count == 0)
                    AllGuardRemoved();
            }
        }
    }

    public void AddPeon(Peon p)
    {
        if(p != null)
        {
            if (!peons.Contains(p))
                peons.Add(p);
        }
    }

    public void AddGuard(Guard g)
    {
        if(g != null)
        {
            if (!guards.Contains(g))
                guards.Add(g);
        }
    }
}
