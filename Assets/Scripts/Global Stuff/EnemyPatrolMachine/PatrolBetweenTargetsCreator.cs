using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatrolBetweenTargetsCreator
{
	const string targetContainerName = "EnemysTargets";
	protected Transform TargetsContainer { get; private set; }

    public abstract bool IsBetweenTargets(Transform obj);

    protected Transform carrier;
    protected List<Transform> targetsList;


    public PatrolBetweenTargetsCreator(Transform carrier)
	{
        TargetsContainer = GameObject.Find(targetContainerName).transform;

        targetsList = new List<Transform>();

        this.carrier = carrier;
    }


	Transform _currentTarget;
	public Transform CurrentTarget
	{
		get
		{
			return _currentTarget ?? targetsList[0];
        }
	}


	bool _isDefPosBetweenTargetsHasBeenSet = false;
	Vector2 _defaultPositionBetweenTargets;
    public Vector2 DefaultPositionBetweenTargets
	{
		get
		{
			if (_isDefPosBetweenTargetsHasBeenSet)
				return _defaultPositionBetweenTargets;

			return targetsList[0]?.position ?? Vector3.zero;
        }
		set
		{
			_defaultPositionBetweenTargets = value;
			_isDefPosBetweenTargetsHasBeenSet = true;
        }	
	}

	public void SetupTargets()
	{
		if (TargetsContainer == null)
		{
			TargetsContainer = new GameObject(targetContainerName).transform;
			TargetsContainer.position = Vector3.zero;
        }

        foreach (var target in targetsList)
		{
			target.parent = TargetsContainer;
        }
	}

    int _targetsListIndex = 0;
    public void SwitchTarget()
	{
		if (targetsList == null || targetsList.Count == 0)
			return;

		_targetsListIndex++;
		if (!(_targetsListIndex < targetsList.Count))
			_targetsListIndex = 0;

        _currentTarget = targetsList[_targetsListIndex];
    }

}

