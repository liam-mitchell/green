using UnityEngine;
using System;
using System.Collections;

public abstract class FocusCamera : MonoBehaviour {
	public float initialDistance;
	
	public float sensitivity;
	public float zoomSensitivity;
	
	private float distance;
	
	// Use this for initialization
	void Start () {
		distance = initialDistance;
		UpdatePosition();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateDistance();
		UpdatePosition();
		UpdateRotation();
	}

	protected abstract bool Pan();
	protected abstract float Zoom();
	protected abstract float PanSide();
	protected abstract float PanUp();
	protected abstract Vector3 Target();
	
	private void UpdatePosition() {
		var target = Target();
		var direction = transform.position - target;

//		Debug.Log (String.Format("Updating position: direction {0}, position {1}, target position {2}", direction, transform.position, target));

		if (Pan()) {
			var up = Quaternion.Euler (transform.up.normalized * PanSide() * sensitivity * Time.deltaTime);
			var right = Quaternion.Euler (transform.right.normalized * PanUp() * sensitivity * Time.deltaTime);
			direction = up * direction;
			direction = right * direction;
//			Debug.Log (String.Format ("Updated direction: up {0}, right {1}, direction now {2}", up, right, direction));
		}

//		Debug.Log (String.Format ("Updating position: {0}", transform.position));

		transform.position = target + direction.normalized * distance;

//		Debug.Log (String.Format ("Updated position: {0}, distance {1}", transform.position, distance));
	}
	
	private void UpdateDistance() {
		distance += Zoom() * zoomSensitivity * Time.deltaTime;
	}
	
	private void UpdateRotation() {
		var direction = Target() - transform.position;
		var rotation = Quaternion.LookRotation (direction);
		transform.rotation = rotation;
	}
	
	private float ScaleAxis(string axis, float sensitivity) {
		return Input.GetAxis(axis) * sensitivity;
	}
	
	private float ScaleAxisByTime(string axis, float sensitivity) {
		return ScaleAxis(axis, sensitivity) * Time.deltaTime;
	}
}
