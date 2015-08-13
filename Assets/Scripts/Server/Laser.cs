using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Laser : Projectile {
	[ClientRpc]
	protected override void RpcDie() {
		//Debug.Log ("Laser dying!");
		//gameObject.GetComponent<ParticleSystem>().Play ();
	}
}
