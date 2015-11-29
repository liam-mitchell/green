using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShipPartSpawner : MonoBehaviour {
//	public List<GameObject> parts;
//
//	void Start() {
//		foreach (var part in parts) {
//			var prefab = part.GetComponent<ShipPartPrefab>();
//			if (prefab == null) {
//				throw new ShipPart.PartSpawningException(String.Format("No ShipPartPrefab attached to part {0}", part));
//			}
//		}
//	}
//
//	public GameObject Spawn(string name) {
//		Debug.Log (String.Format ("Spawning part with name {0}", name));
//		var prefab = parts.Find (p => p.name == name);
//		if (prefab) {
//			return prefab.GetComponent<ShipPartPrefab>().Spawn();
//		}
//		else {
//			return null;
//		}
//	}
//
//	public ShipPart Create(string name) {
//		var prefab = parts.Find(p => p.name == name);
//		if (prefab) {
//			return prefab.GetComponent<ShipPartPrefab>().Create();
//		}
//		else {
//			return null;
//		}
//	}
}
