using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ProjectileSpawner : NetworkBehaviour {
	public GameObject projectilePrefab;

	[Server]
	public void Spawn(Vector3 position, Quaternion rotation, GameObject target) {
		var projectile = (GameObject)Instantiate(projectilePrefab, position, rotation);
		projectile.GetComponent<Projectile>().Lock(target);
		NetworkServer.Spawn (projectile);
	}
}
