using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Weapon : NetworkBehaviour {
	public float cooldown;
	public Spawner projectileSpawner;
	public TargetingSystem targetingSystem;

	private PlayerInput input;
	private float currentCooldown;
	private GameObject lockTarget;

	[ServerCallback]
	void Start() {
		input = GetComponent<PlayerInput>();
	}

	[ServerCallback]
	void Update() {
		if (input.state.fire && currentCooldown <= 0.0f) {
			var projectile = projectileSpawner.Spawn (transform.position + transform.right * 5,
			                                          Quaternion.LookRotation(transform.right, transform.up));
			var component = projectile.GetComponent<Projectile>();
			component.Lock(targetingSystem.Target());
			component.Ignore(gameObject);
			currentCooldown = cooldown;
		}

		currentCooldown -= Time.deltaTime;
	}
}
