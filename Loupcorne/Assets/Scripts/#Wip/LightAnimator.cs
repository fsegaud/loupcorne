using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightAnimator : MonoBehaviour
{
    [SerializeField]
    private float min;

    [SerializeField]
    private float max;

    [SerializeField]
    private float speed;

	void Update ()
    {
        this.light.intensity = this.min + Mathf.PingPong(Time.time * this.speed, this.max - this.min);
	}
}
