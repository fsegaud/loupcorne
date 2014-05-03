using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsManager : Singleton<UnitsManager> 
{
	public Player player;

	public List<Guard> guards;

	public List<Peon> peons;

	public delegate void RemovePeonCallback(Peon p);
	public static event RemovePeonCallback OnRemovePeon;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		//guards = new List<Guard>();
		//peons = new List<Peon>();
	}

	void Start () 
	{
	
	}

	void Update () 
	{
	
	}

	public void RemovePeon(Peon p)
	{
		OnRemovePeon(p);
		peons.Remove(p);
	}
}
