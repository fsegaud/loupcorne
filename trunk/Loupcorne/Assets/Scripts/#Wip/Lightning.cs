using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class Lightning : MonoBehaviour
{
    [SerializeField]
    private float proba;

    [SerializeField]
    private int frameCount;

    private int remainingFrames;

    void Update()
    {
        if (!this.light.enabled)
        {
            if (Random.Range(0f, 100f) <= this.proba)
            {
                // Trigger lightning.
                this.TriggerLightining();
            }
        }
        else
        {
            if (--this.remainingFrames <= 0)
            {
                // Stop lightning.
                this.light.enabled = false;
            }
        }
    }

    private void TriggerLightining()
    {
        this.light.enabled = true;
        this.remainingFrames = this.frameCount;

        //Camera.main.GetComponent<ShakeThatCam>().Shake();
    }
}
