using UnityEngine;
using System.Collections;

public class ResetDifficulty : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        GameStats.difficulty = 1;
        GameStats.nbPeonKilled = 0;
        GameStats.nbPeon = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
