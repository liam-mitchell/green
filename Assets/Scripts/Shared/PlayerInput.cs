using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class PlayerInput : NetworkBehaviour {
	public struct InputState {
		public float horizontal;
		public float vertical;
		public float horizontalLook;
		public float verticalLook;

		public bool fire;
		public bool locking;
	}

	public InputState state;

	[Command]
	void CmdSend(InputState input) {
		state = input;
	}

	[ClientCallback]
	void FixedUpdate() {
		CmdSend(state);
	}

	[ClientCallback]
	void Update() {
		state.horizontal = Input.GetAxis ("Horizontal");
		state.vertical = Input.GetAxis ("Vertical");
		state.horizontalLook = Input.GetAxis ("HorizontalLook");
		state.verticalLook = Input.GetAxis ("VerticalLook");
		
		state.fire = Input.GetButton ("Fire1");
		state.locking = Input.GetButton ("Locking");
	}

	public void LogState(String message) {
		Debug.Log (message);

		Debug.Log (String.Format ("h: {0}", state.horizontal));
		Debug.Log (String.Format ("v: {0}", state.vertical));
		Debug.Log (String.Format ("hl: {0}", state.horizontalLook));
		Debug.Log (String.Format ("vl: {0}", state.verticalLook));
	}
}
