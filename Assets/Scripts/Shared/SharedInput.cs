using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class SharedInput : NetworkBehaviour {
	public struct InputState {
		public float horizontal;
		public float vertical;
		public float horizontalLook;
		public float verticalLook;

		public bool fire;
	}

	public InputState state;

	[Command]
	void CmdSend(InputState input) {
		state = input;

		LogState("CmdSend:");
	}

	[ClientCallback]
	void FixedUpdate() {
		LogState("FixedUpdate:");

		CmdSend(state);
	}

	[ClientCallback]
	void Update() {
		state.horizontal = Input.GetAxis ("Horizontal");
		state.vertical = Input.GetAxis ("Vertical");
		state.horizontalLook = Input.GetAxis ("HorizontalLook");
		state.verticalLook = Input.GetAxis ("VerticalLook");
		
		state.fire = Input.GetButton ("Fire1");
	}

	public void LogState(String message) {
		Debug.Log (message);
	}
}
