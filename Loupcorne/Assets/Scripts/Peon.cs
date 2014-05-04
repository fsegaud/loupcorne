using UnityEngine;
using System.Collections;

public class Peon : Entity 
{
	enum PeonStates
	{
		IDLE,
		MOVING,
		ATTACKED
	}
	private PeonStates _state;
	private UnitsManager _um;
	[SerializeField] private float moveMax;
	[SerializeField] private float speed;
	private Vector3 _target;

    private NavMeshAgent _navAgent;

	void Start () 
	{
		_um = UnitsManager.Instance;
		_state = PeonStates.IDLE;

        _navAgent = GetComponent<NavMeshAgent>();

        // Apply simulation object.
        this.SetSimObject(LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimObject>().GetElement("Peon"));
	}

	void Update () 
	{
		switch(_state)
		{
		case PeonStates.IDLE:
			float moveX = Random.Range(0, moveMax);
			float moveZ = Random.Range(0, moveMax);
			_target = new Vector3(moveX, transform.position.y, moveZ);
            _navAgent.SetDestination(_target);
            _state = PeonStates.MOVING;
			break;
        case PeonStates.MOVING:
            if (_navAgent.remainingDistance == 0)
            {
                _navAgent.Stop();
                _state = PeonStates.IDLE;
            }
			break;
		case PeonStates.ATTACKED:
            _navAgent.Stop();
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
