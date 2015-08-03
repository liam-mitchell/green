using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class SharedPlayerMovement : NetworkBehaviour {
	public float maxVelocity;
	public float maxTurnVelocity;

	public float maxAcceleration;
	public float maxTurnAcceleration;

	public SharedInput input;

	private Vector3 velocity;
	private Vector3 acceleration;

	// Use this for initialization
	void Start () {
		velocity = Vector3.zero;
		acceleration = Vector3.zero;
	}

	[ClientCallback]
	void Update() {
		Debug.Log ("Update on client: SharedPlayerMovement");
		Debug.Log (String.Format("transform: ({0}, {1}, {2})", transform.position.x, transform.position.y, transform.position.z));
		Debug.Log (String.Format ("MaxAcceleration: {0}", maxAcceleration));
		Debug.Log (String.Format("vel: ({0}, {1}, {2})", velocity.x, velocity.y, velocity.z));
		Debug.Log (String.Format("accel: ({0}, {1}, {2})", acceleration.x, acceleration.y, acceleration.z));
	}

	[ServerCallback]
	void FixedUpdate() {
		Debug.Log ("FixedUpdate on server: SharedPlayerMovement");
		Accelerate();
		Move();
	}

	private void Accelerate() {
		input.LogState("Accelerating in FixedUpdate on server");
		Debug.Log (String.Format("transform: ({0}, {1}, {2})", transform.position.x, transform.position.y, transform.position.z));
		Debug.Log (String.Format ("MaxAcceleration: {0}", maxAcceleration));
		acceleration = (transform.up * input.state.horizontal + transform.right * -input.state.vertical) * maxAcceleration;

		velocity += acceleration * Time.fixedDeltaTime;
		Debug.Log (String.Format("vel: ({0}, {1}, {2})", velocity.x, velocity.y, velocity.z));
		Debug.Log (String.Format("accel: ({0}, {1}, {2})", acceleration.x, acceleration.y, acceleration.z));

		if (velocity.magnitude > maxVelocity) {
			velocity = velocity.normalized * maxVelocity;
		}
	}

	private void Move() {
		transform.position += velocity * Time.fixedDeltaTime;

		var vertical = maxTurnVelocity * input.state.verticalLook * Time.fixedDeltaTime;
		var horizontal = maxTurnVelocity * input.state.horizontalLook * Time.fixedDeltaTime;
		Debug.Log(String.Format ("Rotating by {0} vertical {1} horizontal", vertical, horizontal));
		transform.Rotate(new Vector3(horizontal, vertical, 0));
	}
}
