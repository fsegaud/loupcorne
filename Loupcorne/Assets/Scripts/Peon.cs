﻿using UnityEngine;
using System.Collections;
using System.Linq;

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
	private Vector3 _target;

    private NavMeshAgent _navAgent;

	protected override void Start () 
	{
		_um = UnitsManager.Instance;
		_state = PeonStates.IDLE;

        _navAgent = GetComponent<NavMeshAgent>();

        // Apply simulation object.
        this.SetSimObject(LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimObject>().GetElement("Peon"));
        this.Refresh();

        UnitsManager.GuardRemoved += OnGuardRemoved;

        base.Start();
	}

	void Update () 
	{
        animation["walk"].speed = (float)this.GetPropertyValue(SimProperties.Speed) / 1;
        //animation["protect"].weight = 2;
        //animation["protect"].speed = 2;
        //animation["protect"].blendMode = AnimationBlendMode.Additive;

        _navAgent.speed = (float)this.GetPropertyValue(SimProperties.Speed);

		switch(_state)
		{
		case PeonStates.IDLE:
                animation.CrossFade("idle");
			float moveX = Random.Range(-moveMax, moveMax);
			float moveZ = Random.Range(-moveMax, moveMax);
			_target = new Vector3(moveX, transform.position.y, moveZ);
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, _target, -1, path))
            {
                _navAgent.SetDestination(_target);
                _state = PeonStates.MOVING;
            }
            
			break;
        case PeonStates.MOVING:
            animation.CrossFade("walk");
            if (_navAgent.remainingDistance == 0)
            {
                _navAgent.Stop();
                _state = PeonStates.IDLE;
            }
			break;
		case PeonStates.ATTACKED:
            animation.Play("protect");
            _navAgent.SetDestination(transform.position);
			break;
		}
	
	}


	public override void Kill()
	{
        base.Kill();
		_um.RemovePeon(this);
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<Guard>() != null && _state != PeonStates.ATTACKED)
			_state = PeonStates.ATTACKED;
	}

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Guard>() != null)
        {
            if(other.GetComponent<Guard>()._target == this)
                _state = PeonStates.IDLE;

            if (!_um.guards.Any(g => g._target == this))
                _state = PeonStates.IDLE;
        }
    }

    private void OnGuardRemoved(Guard g)
    {
        if (g._target == this)
            _state = PeonStates.IDLE;
    }

    void OnDisable()
    {
        UnitsManager.GuardRemoved -= OnGuardRemoved;
    }
}
