using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour 
{
	enum GuardStates
	{
		IDLE,
		CHASING,
		ATTACK
	} 
	private GuardStates _state;
	private UnitsManager _um;
	[SerializeField] private float viewDistance;
	[SerializeField] private float speed;
	private Peon _target;
	
	void Start () 
	{
		_um = UnitsManager.Instance;
		_state = GuardStates.IDLE;
	}

	void Update () 
	{
		switch(_state)
		{
		case GuardStates.IDLE:
			Debug.Log (_um.peons.Count);
			foreach(Peon peon in _um.peons)
			{
				float dist = Vector3.Distance(transform.position, peon.transform.position);
				Debug.Log (dist);
				if(dist <= viewDistance)
				{
					_target = peon;
					_state = GuardStates.CHASING;
					break;
				}
			}
			break;
		case GuardStates.CHASING:
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, step);
			break;
		case GuardStates.ATTACK:
			Debug.Log("Guard : Die communist scum !");
			//TODO: attack
			break;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<Peon>() != null)
		{
			_state = GuardStates.ATTACK;
		}
	}
}
