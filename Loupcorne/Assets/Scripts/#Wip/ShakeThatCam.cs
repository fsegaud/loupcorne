using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShakeThatCam : MonoBehaviour
{
    [SerializeField]
    private int numPoints;

    [SerializeField]
    private float maxOffset;

    private Queue<Vector3> points = new Queue<Vector3>();
    private Vector3 localPosOrig;

    void Start()
    {
        this.localPosOrig = this.transform.localPosition;
    }

    void Update()
    {
        if (this.points.Count > 0)
        {
            this.transform.localPosition = this.localPosOrig + this.points.Dequeue();
        }
        else
        {
            this.transform.localPosition = this.localPosOrig;
        }
    }

    public void Shake()
    {
        for (int i = 0; i < this.numPoints; ++i)
        {
            this.points.Enqueue(new Vector3(Random.Range(0f, this.maxOffset), Random.Range(0f, this.maxOffset), Random.Range(0f, this.maxOffset)));
        }
    }
}
