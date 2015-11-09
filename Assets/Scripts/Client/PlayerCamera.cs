using UnityEngine;
using System.Collections;

public class PlayerCamera : FocusCamera {
	protected override Vector3 Target() {
		return Vector3.zero;
	}

	protected override bool Pan() {
		return true;
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
}
