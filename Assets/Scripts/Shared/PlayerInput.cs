using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

/**
 * PlayerInput
 * 
 * Sends the local player's commands to the server for processing.
 */
public class PlayerInput : NetworkBehaviour {
	// Input state to send from client to region server
	public struct InputState {
		public float horizontal;
		public float vertical;
		public float horizontalLook;
		public float verticalLook;
		public float accelerate;

		public bool fire;
		public bool locking;

		public bool craftingMode;
	}

	public InputState state;

	[Command]
	void CmdSend(InputState input) {
		state = input;
		Debug.Log (String.Format ("accelerate: {0}", state.accelerate));
	}

	[ClientCallback]
	void FixedUpdate() {
		CmdSend(state);
	}

	[ClientCallback]
	void Update() {
		// [ClientCallback] unfortunately runs on the region server, since it has an active
		// NetworkClient connection to the data server. So, manually check if the server is
		// active here (since this object must be alive on both server and client, in order
		// to process input.
		if (NetworkServer.active) {
			return;
		}

		state.horizontal = Input.GetAxis ("Horizontal");
		state.vertical = Input.GetAxis ("Vertical");
		state.horizontalLook = Input.GetAxis ("HorizontalLook");
		state.verticalLook = Input.GetAxis ("VerticalLook");
		state.accelerate = Input.GetAxis ("Accelerate");
		
		state.fire = Input.GetButtonDown ("Fire1");
		state.locking = Input.GetButtonDown ("Locking");
		state.craftingMode = Input.GetButtonDown ("CraftingMode");
	}
}
