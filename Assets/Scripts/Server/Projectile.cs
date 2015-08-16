using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Projectile : NetworkBehaviour {
	public float velocity;
	public float trackingVelocity;
	public float range;
	public int damage;

	private Vector3 startPosition;
	private GameObject target;
	private List<GameObject> ignoredCollisions;

	[ServerCallback]
	void Start() {
		startPosition = transform.position;
	}

	[ServerCallback]
	void FixedUpdate() {
//		Debug.Log (String.Format("TrackVelocity: ({0}, {1}, {2})", TrackVelocity().x, TrackVelocity().y, TrackVelocity().z));
//		Debug.Log (String.Format("position: ({0}, {1}, {2})", transform.position.x, transform.position.y, transform.position.z));
//		Debug.Log (String.Format("target: ({0}, {1}, {2})", target.transform.position.x, target.transform.position.y, target.transform.position.z));
		Debug.Log (target);
		var oldPosition = transform.position;
		transform.position += GetVelocity() * Time.fixedDeltaTime;

		CheckCollisions(oldPosition);

		if ((transform.position - startPosition).magnitude >= range) {
			Debug.Log("Target out of range!");
			Die();
		}
	}

	[Server]
	private Vector3 TrackVelocity() {
		if (target) {
			Debug.Log ("Tracking!");
			var direction = target.transform.position - transform.position;
			return direction.normalized * trackingVelocity;
		}

		return Vector3.zero;
	}

	[Server]
	private Vector3 GetVelocity() {
		var v = transform.forward * velocity;
		if (target) {
			v += (target.transform.position - transform.position).normalized * trackingVelocity;
		}

		return v.normalized * velocity;
	}

	[Server]
	private void CheckCollisions(Vector3 old) {
		Vector3 direction = old - transform.position;
		bool dead = false;

		RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, direction.magnitude, ~(1 << (int)Layers.TARGETABLE) & ~(1 << (int)Layers.IGNORE_RAYCAST));
		foreach (var hit in hits) {
			Debug.Log(String.Format ("RaycastHit: object {0}", hit.collider.gameObject));
			if (ignoredCollisions != null && !ignoredCollisions.Contains (hit.collider.gameObject)) {
				hit.collider.transform.root.gameObject.GetComponentInChildren<PlayerHealth>().CollisionProjectile(this);
				Debug.Log ("Collided projectile!");
				dead = true;
			}
		}

		if (dead) {
			Die();
		}
	}

	[Server]
	private void Die() {
		Debug.Log ("Projectile dying!");
		RpcDie();
		NetworkServer.Destroy(gameObject);
	}

	[Server]
	public void Lock(GameObject t) {
		Debug.Log (String.Format ("Projectile locked: {0}", t));
		target = t;
	}

	[Server]
	public void Ignore(GameObject o) {
		if (ignoredCollisions == null) {
			ignoredCollisions = new List<GameObject>();
		}

		Debug.Log("Ignoring object!");
		ignoredCollisions.Add (o);
		Debug.Log("Ignored object!");
	}

	[ClientRpc]
	protected abstract void RpcDie();
}