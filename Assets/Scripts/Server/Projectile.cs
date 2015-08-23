using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Projectile : NetworkBehaviour {
	public float velocity;
	public float lifetime;

	public int damage;

	protected Vector3 startPosition;
	protected GameObject target;
	protected List<GameObject> ignoredCollisions;

	protected float currentLifetime;

	[ServerCallback]
	void Start() {
		startPosition = transform.position;
		currentLifetime = 0;
	}

	[ServerCallback]
	void FixedUpdate() {
		UpdateVelocity();
		if (UpdatePosition() || UpdateDuration()) {
			Die();
		}
	}

	[Server]
	protected virtual bool UpdateDuration() {
		currentLifetime += Time.fixedDeltaTime;
		if (currentLifetime > lifetime) {
			return true;
		}

		return false;
	}

	[Server]
	protected virtual Vector3 Velocity() {
		return transform.forward * velocity;
	}

	[Server]
	protected virtual bool UpdatePosition() {
		var oldPosition = transform.position;
		transform.position += Velocity() * Time.fixedDeltaTime;
		return CheckCollisions(oldPosition);
	}

	[Server]
	protected virtual void UpdateVelocity() {}

	[Server]
	private bool CheckCollisions(Vector3 old) {
		Vector3 direction = old - transform.position;

		RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, direction.magnitude, ~(1 << (int)Layers.TARGETABLE) & ~(1 << (int)Layers.IGNORE_RAYCAST));
		foreach (var hit in hits) {
			Debug.Log(String.Format ("RaycastHit: object {0}", hit.collider.gameObject));
			if (ignoredCollisions != null && !ignoredCollisions.Contains (hit.collider.gameObject)) {
				hit.collider.transform.root.gameObject.GetComponentInChildren<PlayerHealth>().CollisionProjectile(this);
				Debug.Log ("Collided projectile!");
				return true;
			}
		}

		return false;
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