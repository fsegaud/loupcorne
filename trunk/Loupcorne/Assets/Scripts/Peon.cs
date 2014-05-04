﻿using UnityEngine;
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

        base.Start();
	}

	void Update () 
	{
        animation["walk"].speed = (float)this.GetPropertyValue(SimProperties.Speed) / 3;
        animation["protect"].weight = 2;
        animation["protect"].speed = 2;
        animation["protect"].blendMode = AnimationBlendMode.Additive;

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
            _navAgent.Stop();
			break;
		}
	
	}


	public override void Kill()
	{
        base.Kill();

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
