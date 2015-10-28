using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShipPartSpawner : MonoBehaviour {
	public List<GameObject> parts;

	void Start() {
		foreach (var part in parts) {
			var prefab = part.GetComponent<ShipPartPrefab>();
			if (prefab == null) {
				throw new ShipPart.PartSpawningException(String.Format("No ShipPartPrefab attached to part {0}", part));
			}
		}
	}
}
