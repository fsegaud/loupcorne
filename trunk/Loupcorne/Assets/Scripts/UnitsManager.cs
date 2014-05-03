using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsManager : Singleton<UnitsManager> 
{
	public Transform player;

	public List<Guard> guards;

	public List<Peon> peons;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		//guards = new List<Guard>();
		//peons = new List<Peon>();
	}

	void Start () 
	{
	
	}

	void Update () 
	{
	
	}
}
