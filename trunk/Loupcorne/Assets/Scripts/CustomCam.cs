using UnityEngine;
using System.Collections;

public class CustomCam : MonoBehaviour 
{
	[SerializeField] private float coefCamPos = 0.5f;

	private Transform _player;
	private Vector3 _targetPos;
	
	void Awake()
	{
		_player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Start () 
	{
	}

	void Update () 
	{
		Vector3 worldMousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.localPosition.z));

		Vector3 playerToMousePos = _player.position - worldMousePos;
		playerToMousePos *=  Mathf.Clamp(coefCamPos, 0f, 0.5f); //clamp coefCamPos to avoid camera glitchs

		_targetPos = _player.position + playerToMousePos;
		_targetPos += new Vector3(0, -_targetPos.y, 0);

		transform.parent.position = _targetPos;
	}
}
