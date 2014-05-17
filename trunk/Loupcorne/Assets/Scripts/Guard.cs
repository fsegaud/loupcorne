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
	[SerializeField] private float attackSpeed;
	private Entity _target;
	private float _attackTimer;

    private NavMeshAgent _navAgent;
	
	protected override void Start () 
	{
       

		_um = UnitsManager.Instance;
		_state = GuardStates.IDLE;

		_attackTimer = attackSpeed;

        _navAgent = GetComponent<NavMeshAgent>();

		UnitsManager.PeonRemoved += OnPeonRemoved;

        // Apply simulation object.
        this.SetSimObject(LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimObject>().GetElement("Guard"));
        this.Refresh();

        Player.PlayerIsDead += OnPlayerDeath;

        base.Start();
	}

	void Update () 
	{
#if UNITY_EDITOR && _STAY_IDLE
        _state = GuardStates.IDLE;
        return;
#endif
        animation["run"].speed = (float)this.GetPropertyValue(SimProperties.Speed) / 4;
        _navAgent.speed = (float)this.GetPropertyValue(SimProperties.Speed);

		switch(_state)
		{
		case GuardStates.IDLE:
            animation.CrossFade("idle");
            float distToPlayer = Vector3.Distance(transform.position, _um.player.transform.position);
            bool targetAssigned = false;
            //Search for a peon in visible area
			foreach(Peon peon in _um.peons)
			{
				float distToPeon = Vector3.Distance(transform.position, peon.transform.position);

                if (distToPeon <= viewDistance) //if peon is in visible area
				{
					if(distToPlayer <= viewDistance) //and player is also in visible area
					{
						float r = Random.Range(0f, 1f);
                        if (r > 0.5f) //50% to focus player
						{
							_target = _um.player;
						}
						else
						{
							_target = peon;
						}
					}
					else //if player isn't in visible area then focus peon
					{
						_target = peon;

					}
					_state = GuardStates.CHASING;
                    targetAssigned = true;
					break;
				}
			}
            if (targetAssigned == false) //if no target was assigned when browsing peons
            {
                if (distToPlayer <= viewDistance) //check if player is visible
                {
                    _target = _um.player;
                    targetAssigned = true;
                    _state = GuardStates.CHASING;
                }
            }
			break;
		case GuardStates.CHASING:
                animation.CrossFade("run");
            _navAgent.SetDestination(_target.transform.position);
			break;
		case GuardStates.ATTACK:
            _navAgent.Stop();
            animation.Play("hit");
			_attackTimer += Time.deltaTime;
			if(_attackTimer >= attackSpeed)
            {
                float effectiveDmg = (float)this.GetPropertyValue(SimProperties.Attack) * 0.3f - (float)_target.GetPropertyValue(SimProperties.Defence) * 0.1f;
                _target.Hit(effectiveDmg);
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

    void OnTriggerExit(Collider other)
    {
        Entity triggeringEntity = other.GetComponent<Entity>();
        if (triggeringEntity is Peon || triggeringEntity is Player)
        {
            _state = GuardStates.CHASING;
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
		UnitsManager.PeonRemoved -= OnPeonRemoved;
        Player.PlayerIsDead -= OnPlayerDeath;
	}

    public override void Kill()
    {
        base.Kill();
        _um.RemoveGuard(this);
        Destroy(gameObject);
    }

    private void OnPlayerDeath()
    {
        _state = GuardStates.IDLE;
    }
}
