using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Spawner : NetworkBehaviour {
	public GameObject prefab;

	[Server]
	public GameObject Spawn(Vector3 position, Quaternion rotation) {
		GameObject o = (GameObject)Instantiate(prefab, position, rotation);
		NetworkServer.Spawn (o);
		return o;
	}
}
