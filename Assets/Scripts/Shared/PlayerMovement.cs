using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class PlayerMovement : NetworkBehaviour {
	public float maxVelocity;
	public float maxTurnVelocity;

	public float maxAcceleration;
	public float maxTurnAcceleration;

	public PlayerInput input;

	private Vector3 velocity;
	private Vector3 acceleration;

	// Use this for initialization
	void Start () {
		velocity = Vector3.zero;
		acceleration = Vector3.zero;
	}

	[ClientCallback]
	void Update() {

	}

	[ServerCallback]
	void FixedUpdate() {
		Accelerate();
		Move();
	}

	private void Accelerate() {
		acceleration = (transform.up * input.state.horizontal + transform.right * -input.state.vertical) * maxAcceleration;

		velocity += acceleration * Time.fixedDeltaTime;

		if (velocity.magnitude > maxVelocity) {
			velocity = velocity.normalized * maxVelocity;
		}
	}

	private void Move() {
		transform.position += velocity * Time.fixedDeltaTime;

		var vertical = maxTurnVelocity * input.state.verticalLook * Time.fixedDeltaTime;
		var horizontal = maxTurnVelocity * input.state.horizontalLook * Time.fixedDeltaTime;
		transform.Rotate(new Vector3(horizontal, vertical, 0));
	}
}
