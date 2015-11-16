using UnityEngine;
using System.Collections;

public class PlayerCamera : FocusCamera {
	[HideInInspector]
	public GameObject player;

	protected override Vector3 Target() {
		if (player == null) {
			return Vector3.zero;
		}

		return player.transform.position;
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
