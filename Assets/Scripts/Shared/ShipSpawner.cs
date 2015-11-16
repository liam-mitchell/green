using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShipSpawner : MonoBehaviour {
	public GameObject ship;
	public GameObject cam;
	public List<GameObject> components;

	public GameObject Spawn() {
		ClientScene.RegisterPrefab(ship);
		var s = (GameObject)GameObject.Instantiate(ship);
		Debug.Log ("Spawning ship");
		if (s.GetComponent<NetworkIdentity>().isLocalPlayer) {
			Debug.Log ("Spawning local ship");
			AddComponents(s);
		}

		return s;
	}

	public GameObject Spawn(string name) {
		var s = Spawn();
		s.name = name;
		Debug.Log (String.Format("Spawned ship with name {0}", name));
		return s;
	}

	public void AddComponents(GameObject s) {
//		var c = (GameObject)GameObject.Instantiate(cam);
//		c.transform.SetParent(s.transform);
//		c.GetComponent<PlayerCamera>().player = s;
	}
}
