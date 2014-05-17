using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour 
{
    private int capacity;
    private int nbGuard;

	void Awake()
	{
		capacity = 1;
		nbGuard = 0;
	}

	// Use this for initialization
	void Start () {

		//Debug.Log("Start SpawnPoint");
        
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool AddGuard(GameObject guard)
	{
		//Debug.Log(capacity);
		//Debug.Log(nbGuard);
        if (nbGuard < capacity)
        {
			//Debug.Log("Adding Guard");
            nbGuard++;
            guard.transform.localPosition = this.transform.localPosition;
            return true;
        }
        return false;
    }
}
