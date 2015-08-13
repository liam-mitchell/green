using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class PlayerHealth : NetworkBehaviour {
	public int maxHealth;
	private int health;

	void Start() {
		health = maxHealth;
	}

	void OnTriggerEnter(Collider c) {
		Debug.Log("Trigger entered: PlayerHealth");
	}

	[Server]
	public void CollisionProjectile(Projectile projectile) {
		Debug.Log(String.Format("Player hit by projectile! Damage: {0}", projectile.damage));
		TakeDamage(projectile.damage);
	}

	[Server]
	private void TakeDamage(int damage) {
		health -= damage;
		if (health <= 0) {
			Die();
		}
	}

	[Server]
	private void Die() {
		Debug.Log ("Player dying!");
		NetworkServer.Destroy (gameObject);
	}
}
