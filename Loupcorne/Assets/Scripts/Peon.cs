using UnityEngine;
using System.Collections;

public class Peon : Entity 
{
	enum PeonStates
	{
		IDLE,
		MOVE,
		ATTACKED
	}
	private PeonStates _state;
	private UnitsManager _um;
	[SerializeField] private float moveMax;
	[SerializeField] private float speed;
	private Vector3 _target;

	void Start () 
	{
		_um = UnitsManager.Instance;
		_state = PeonStates.IDLE;
	
	}

	void Update () 
	{
		switch(_state)
		{
		case PeonStates.IDLE:
			float moveX = Random.Range(0, moveMax);
			float moveZ = Random.Range(0, moveMax);
			_target = new Vector3(moveX, transform.position.y, moveZ);
			_state = PeonStates.MOVE;
			break;
		case PeonStates.MOVE:
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, _target, step); 
			break;
		case PeonStates.ATTACKED:
			break;
		}
	
	}


	public override void Kill()
	{
		Debug.Log ("WAAARRRGHHH");
		_um.RemovePeon(this);
		Destroy(gameObject);

	}

	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<Guard>() != null)
		{
			_state = PeonStates.ATTACKED;
		}
	}
}
