using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour 
{

	void Start () 
    {
        camera.enabled = false;
	}
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            camera.enabled = !camera.enabled;
        }
	}
}
