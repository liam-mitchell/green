using UnityEngine;
using System.Collections;
using System;

public class EditorCamera : FocusCamera {
	public GameObject target;

	protected override bool Pan() {
		return Input.GetAxis ("Pan") > 0.0f;
	}

	protected override float Zoom() {
		return Input.GetAxis ("Zoom");
	}

	protected override float PanUp() {
		return Input.GetAxis ("RotateUp");
	}

	protected override float PanSide() {
		return Input.GetAxis ("RotateSide");
	}

	protected override Vector3 Target() {
		return target.transform.position;
	}
}
