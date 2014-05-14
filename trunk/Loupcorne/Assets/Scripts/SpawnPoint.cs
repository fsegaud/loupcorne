using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour 
{
    private int capacity;
    private int nbGuard;

	// Use this for initialization
	void Start () {

        capacity = GameStats.difficulty;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool AddGuard(GameObject guard)
    {
        if (nbGuard < capacity)
        {
            nbGuard++;
            guard.transform.position = this.transform.position;
            return true;
        }
        return false;
    }
}
