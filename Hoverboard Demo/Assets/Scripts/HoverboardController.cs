using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverboardController : MonoBehaviour
{
	public bool EnableThrusters = true;
	
	[SerializeField] private List<Thruster> _thrusters;
	[SerializeField] private float _targetHeight = 2.0f;
	[SerializeField] private bool _useRelativeForce = true;
	[SerializeField] private float _forceMultiplier = 5.0f;
	
	private Rigidbody _mainRigidBody;

	//https://youtu.be/qsfIXopyYHY
	private void Start()
	{
		_mainRigidBody = GetComponent<Rigidbody>();
		_mainRigidBody.centerOfMass = -Vector3.up * _targetHeight;
	}

	private void FixedUpdate()
	{
		if (!EnableThrusters) return;

		foreach (Thruster thruster in _thrusters)
		{
			if (!thruster) continue;
			
			Rigidbody thrusterRigidbody = thruster.Rigidbody;
			DistanceResult result = GetDistanceFromFloor(thruster);
			if (!result.FloorDetected) continue;
			
			//TODO: Make physics code frame-time-independent.
			switch (_useRelativeForce)
			{
				case true:
					thrusterRigidbody.AddRelativeForce(Vector3.up * Mathf.Abs(1 / result.Distance * _forceMultiplier), ForceMode.Acceleration);
					break;
				case false:
					thrusterRigidbody.AddForce(Vector3.up * Mathf.Abs(1 / result.Distance * _forceMultiplier), ForceMode.Acceleration);
					break;
			}
		}
	}

	private DistanceResult GetDistanceFromFloor(Thruster thruster)
	{
		Ray ray = new(thruster.transform.position + (-thruster.transform.up * thruster.Collider.bounds.extents.y),
			-thruster.transform.up);
		Physics.Raycast(ray, out RaycastHit hitInfo, _targetHeight);
		
#if UNITY_EDITOR || DEBUG
		Debug.DrawLine(ray.origin, ray.origin + (ray.direction * _targetHeight), Color.red);
		Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hitInfo.distance), Color.green);
#endif
		
		DistanceResult result = new()
		{
			FloorDetected = (bool)hitInfo.collider,
			Distance = hitInfo.distance
		};

		return result;
	}
	
	private struct DistanceResult
	{
		public bool FloorDetected;
		public float Distance;
	}
}
