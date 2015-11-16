using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class Ship {
	public class ShipSpawningException : Exception {
		public ShipSpawningException(string message) : base(message) {}
		public ShipSpawningException() : base() {}
	}

	public List<ShipPart> parts;
	public Vector3 position;

	private GameObject ship;
	public GameObject gameObject {get {return ship;}}

	private float mass;
	public float Mass {get {return mass;}}

	public ShipEngines engines;
	public ShipInputHandlers inputHandlers;

	public bool AddPart(ShipPart part) {
		parts.Add(part);
		mass += part.Mass ();
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

	public void Update() {
		if (!NetworkServer.active) {
			Debug.LogError("Tried to update a ship on the client!");
			return;
		}

		inputHandlers.Update();
		engines.Update();
	}

	public void Serialize(NetworkWriter writer) {
		Debug.Log (String.Format ("Serializing ship with {0} parts", parts.Count));
		writer.Write(parts.Count);
		foreach (var part in parts) {
			part.Serialize(writer);
		}
	}

	public void Deserialize(NetworkReader reader) {
		var count = reader.ReadInt32 ();
		Debug.Log (String.Format ("Deserializing ship with {0} parts", count));
		for (var i = 0; i < count; ++i) {
			Debug.Log ("Deserializing part");
			var part = ShipPart.Deserialize(reader);
			AddPart(part);
		}
	}

	public GameObject Spawn() {
		ship = SpawnShip();
		AddParts();
		return ship;
	}

	public GameObject Spawn(string name) {
		ship = SpawnShip(name);
		AddParts();
		return ship;
	}

	public GameObject Attach(GameObject other) {
		ship = other;
		AddParts();
		return other;
	}

	private GameObject SpawnShip() {
		return Spawner().Spawn();
	}

	private GameObject SpawnShip(string name) {
		return Spawner().Spawn(name);
	}

	private void AddParts() {
		if (NetworkServer.active) {
			engines = new ShipEngines(this);
			inputHandlers = new ShipInputHandlers(ship.GetComponent<PlayerInput>());
			var updater = ship.AddComponent<ShipUpdater>();
			updater.ship = this;
		}

		foreach (var part in parts) {
			var obj = part.Spawn(this);
			Debug.Log (String.Format ("adding obj {0} to ship {1} (part {2})", obj, ship, part));
//			obj.transform.SetParent(ship.transform);
		}
	}

	private ShipSpawner Spawner() {
		var spawner = GameObject.Find("ShipSpawner").GetComponent<ShipSpawner>();
		if (spawner == null) {
			throw new ShipSpawningException("Unable to find ShipSpawner!");
		}

		return spawner;
	}
}