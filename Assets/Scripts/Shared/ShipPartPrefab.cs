using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class ShipPartPrefab : MonoBehaviour {
	public GameObject clientPrefab;
	public GameObject serverPrefab;

	public GameObject Spawn() {
		if (NetworkServer.active) {
			return GameObject.Instantiate(serverPrefab);
		}
		else {
			return GameObject.Instantiate(clientPrefab);
		}
	}

	public ShipPart Create() {
		var obj = Spawn();
		return CreatePart(obj);
	}

	protected abstract ShipPart CreatePart(GameObject obj);
}