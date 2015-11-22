using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class ShipPart {
	public class PartSpawningException : Exception {
		public PartSpawningException(string message) : base(message) {}
		public PartSpawningException() : base() {}
	}

	public delegate ShipPart Deserializer(NetworkReader reader);
	private static Dictionary<string, Deserializer> deserializers;

	public GameObject part;
	public string name;

	public ShipPartSpawner spawner;

	protected Vector3 position;
	protected Quaternion rotation;

	static ShipPart() {
		deserializers = new Dictionary<string, Deserializer>() {
			{Cube.Name, Cube._Deserialize},
			{Engine.Name, Engine._Deserialize}
		};
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

	public GameObject Spawn(Ship ship) {
		if (spawner == null) {
			Initialize();
		}

		part = spawner.Spawn (name);
		part.transform.SetParent(ship.gameObject.transform);
		part.transform.localPosition = position;
		part.transform.localRotation = rotation;
		Attach(ship);
		return part;
	}

	public abstract void Serialize(NetworkWriter writer);

	public static ShipPart Deserialize(NetworkReader reader) {
		var name = reader.ReadString();
		Debug.Log (String.Format ("Deserializing at index {0}, count {1}", name, deserializers.Count));
		var part = deserializers[name](reader);
		part.name = name;
		return part;
	}

	public abstract int Mass();

	public abstract void Attach(Ship ship);
	public abstract void Detach(Ship ship);

	protected void Save() {
		if (part != null) {
			position = part.transform.localPosition;
			rotation = part.transform.localRotation;
		}
	}
}
