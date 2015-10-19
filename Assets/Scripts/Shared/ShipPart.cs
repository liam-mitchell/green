using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class ShipPart {
	public class PartSpawningException : Exception {
		public PartSpawningException(string message) : base(message) {}
		public PartSpawningException() : base() {}
	}
	
	public GameObject part;
	public string name;

	private ShipPartSpawner spawner;

	private Vector3 position;
	private Quaternion rotation;

	public ShipPart(string n) {
		Initialize();

		name = n;
		position = Vector3.zero;
		rotation = Quaternion.identity;
	}

	// For serialization
	public ShipPart() {
		Initialize();
	}

	private void Initialize() {
		spawner = GameObject.Find ("ShipPartSpawner").GetComponent<ShipPartSpawner>();

		if (spawner == null) {
			throw new PartSpawningException("Unable to find ShipPartSpawner");
		}
	}

	public bool Lock(Ship ship) {
		return true; // this should probably... like... check a distance or something T.T
	}

	public GameObject Spawn() {
		var prefab = spawner.parts.Find (p => p.name == name);
		if (prefab == null) {
			throw new PartSpawningException(String.Format("Unable to find part with name {0}", name));
		}

		part = (GameObject)GameObject.Instantiate(prefab, position, rotation);
		return part;
	}

	// TODO
	public void Serialize(NetworkWriter writer) {
		if (part != null) {
			position = part.transform.position;
			rotation = part.transform.rotation;
		}

		Debug.Log (String.Format ("Serializing part {0} at {1}, {2}", name, position, rotation));
		writer.Write(name);
		writer.Write(position);
		writer.Write(rotation);
	}

	public void Deserialize(NetworkReader reader) {
		name = reader.ReadString();
		position = reader.ReadVector3();
		rotation = reader.ReadQuaternion();
		Debug.Log (String.Format ("Deserializing part {0} at {1}, {2}", name, position, rotation));
	}
}
