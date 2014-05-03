//#define _STAY_IDLE

using UnityEngine;
using System.Collections;

public class Guard : Entity 
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
	[SerializeField] private float strength;
	[SerializeField] private float attackSpeed;
	private Entity _target;
	private float _attackTimer;
	
	void Start () 
	{
		_um = UnitsManager.Instance;
		_state = GuardStates.IDLE;

		_attackTimer = attackSpeed;

		UnitsManager.OnRemovePeon += OnPeonRemoved;

	}

	void Update () 
	{
#if UNITY_EDITOR && _STAY_IDLE
        _state = GuardStates.IDLE;
        return;
#endif

		switch(_state)
		{
		case GuardStates.IDLE:
			foreach(Peon peon in _um.peons)
			{
				float distToPeon = Vector3.Distance(transform.position, peon.transform.position);
				Debug.Log (distToPeon);
				if(distToPeon <= viewDistance)
				{
					float distToPlayer = Vector3.Distance(transform.position, _um.player.transform.position);
					if(distToPlayer <= viewDistance)
					{
						float r = Random.Range(0f, 1f);
						if(r > 0.5f)
						{
							_target = _um.player;
						}
						else
						{
							_target = peon;
						}
					}
					else
					{
						_target = peon;

					}
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
			_attackTimer += Time.deltaTime;
			if(_attackTimer >= attackSpeed)
			{
				_target.Hit(strength);
				_attackTimer = 0f;
			}
			break;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Entity triggeringEntity = other.GetComponent<Entity>();

		if(triggeringEntity is Peon || triggeringEntity is Player)
		{
			_state = GuardStates.ATTACK;
		}
	}

	private void OnPeonRemoved(Peon p)
	{
		if(_target == p)
			_target = null;
		_state = GuardStates.IDLE;
	}

	void OnDisable()
	{
		UnitsManager.OnRemovePeon -= OnPeonRemoved;
	}

    public override void Kill()
    {
        Debug.Log("WAAARRRGHHH");
        _um.RemoveGuard(this);
        Destroy(gameObject);
    }
}
