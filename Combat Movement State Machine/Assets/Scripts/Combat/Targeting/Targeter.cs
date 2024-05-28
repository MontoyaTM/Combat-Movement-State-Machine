using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Targeter : MonoBehaviour
{
	[SerializeField] private CinemachineTargetGroup cineTargetGroup;

	private Camera mainCamera;
	private List<Target> targets = new List<Target>();
	public Target currentTarget {  get; private set; }

	private void Start()
	{
		mainCamera = Camera.main;
	}

	private void OnTriggerEnter(Collider other)
	{
		Target target = other.GetComponent<Target>();

		if(target == null )
		{
			return;
		}

		targets.Add(target);
		target.OnDestroyed += RemoveTarget;
	}

	private void OnTriggerExit(Collider other)
	{
		if(!other.TryGetComponent<Target>(out Target target))
		{
			return;
		}

		RemoveTarget(target);
	}

	public bool SelectTarget()
	{
		if (targets.Count == 0)
		{
			return false;
		}

		Target closestTarget = null;
		float closestTargetDistance = Mathf.Infinity;


		foreach(Target target in targets)
		{
			Vector2 viewPosition = mainCamera.WorldToViewportPoint(target.transform.position);

			if(viewPosition.x < 0 || viewPosition.x > 1 || viewPosition.y < 0 || viewPosition.y > 1)
			{
				continue;
			}

			Vector2 toCenter = viewPosition - new Vector2(0.5f, 0.5f);

			if(toCenter.sqrMagnitude < closestTargetDistance )
			{
				closestTarget = target;
				closestTargetDistance = toCenter.sqrMagnitude;
			}
		}

		if ( closestTarget == null ) { return false; }

		currentTarget = closestTarget;
		cineTargetGroup.AddMember(currentTarget.transform, 1f, 2);

		return true;

	}

	public void CancelTarget()
	{
		if(currentTarget == null) { return; }

		cineTargetGroup.RemoveMember(currentTarget.transform);
		currentTarget = null;
	}

	private void RemoveTarget(Target target)
	{
		if (currentTarget == target)
		{
			cineTargetGroup.RemoveMember(currentTarget.transform);
			currentTarget = null;
		}

		target.OnDestroyed -= RemoveTarget;
		targets.Remove(target);
	}


}
