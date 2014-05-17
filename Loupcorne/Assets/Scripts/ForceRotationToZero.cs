using UnityEngine;
using System.Collections;

public class ForceRotationToZero : MonoBehaviour
{
	void Update ()
    {
        this.transform.rotation = Quaternion.identity;
	}
}
