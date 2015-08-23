using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Missile : Projectile {
	public float trackingVelocity;
	
	[ClientRpc]
	protected override void RpcDie() {

	}

	[Server]
	protected override void UpdateVelocity() {
		if (target) {
			transform.rotation = Pursuit.RotateTowards(gameObject, target, trackingVelocity * Time.fixedDeltaTime);
		}
	}
}
