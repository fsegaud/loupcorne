using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightAnimator1 : MonoBehaviour
{
    [SerializeField]
    private float min;

    [SerializeField]
    private float max;

    [SerializeField]
    private float speed;

	void Update ()
    {
        Vector3 v = this.transform.localPosition;
        this.transform.localPosition = new Vector3(v.x, this.min + Mathf.PingPong(Time.time * this.speed, this.max - this.min), v.z);
	}
}
