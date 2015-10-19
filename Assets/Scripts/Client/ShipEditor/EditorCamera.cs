using UnityEngine;
using System.Collections;
using System;

public class EditorCamera : MonoBehaviour {
	public GameObject target;
	public float initialDistance;

	private const float SENSITIVITY = 0.1f;
	private const float ZOOM_SENSITIVITY = 0.01f;

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

	private void UpdatePosition() {
		var direction = transform.position - target.transform.position;

		if (Input.GetAxis ("Pan") > 0.0f) {
			var up = Quaternion.Euler (transform.up.normalized * ScaleAxisByTime("RotateSide", SENSITIVITY));
			var right = Quaternion.Euler (transform.right.normalized * ScaleAxisByTime("RotateUp", SENSITIVITY));
			direction = up * direction;
			direction = right * direction;
		}

		transform.position = target.transform.position + direction.normalized * distance;
	}

	private void UpdateDistance() {
		distance += ScaleAxisByTime("Zoom", ZOOM_SENSITIVITY);
	}

	private void UpdateRotation() {
		var direction = target.transform.position - transform.position;
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
