using UnityEngine;
using System.Collections;

public class DestroyDelay : MonoBehaviour
{
    [SerializeField]
    private float delay;

	void Start ()
    {
        StartCoroutine(this.RunAsync());
	}

    private IEnumerator RunAsync()
    {
        yield return new WaitForSeconds(this.delay);

        GameObject.Destroy(this.gameObject);
        yield break;
    }
}
