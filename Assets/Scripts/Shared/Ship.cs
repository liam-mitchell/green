using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class Ship {
	public List<ShipPart> parts;
	public Vector3 position;

	public bool AddPart(ShipPart part) {
		parts.Add(part);
		Debug.Log (String.Format("Adding part {0}", part));
		return true;
	}

	public void RemovePart(ShipPart part) {
		Debug.Log (String.Format("Removing part {0}", part));
		parts.Remove(part);
	}

	public Ship() {
		parts = new List<ShipPart>();
		position = Vector3.zero;
	}

	public void Serialize(NetworkWriter writer) {
		Debug.Log ("Serializing ship");
		writer.Write(parts.Count);
		foreach (var part in parts) {
			part.Serialize(writer);
		}
	}

	public void Deserialize(NetworkReader reader) {
		Debug.Log ("Deserializing ship");
		var count = reader.ReadInt32 ();
		for (var i = 0; i < count; ++i) {
			var part = new ShipPart();
			part.Deserialize(reader);
			AddPart(part);
		}
	}

	public GameObject Spawn() {
		var ship = new GameObject("Ship");
		ship.transform.position = position;

		foreach (var part in parts) {
			var obj = part.Spawn();
			obj.transform.parent = ship.transform;
		}

		return ship;
	}
}
