using UnityEngine;
using UnityEngine.Assertions;
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
	public PlayerInput input;

	public bool AddPart(ShipPart part) {
		parts.Add(part);
		mass += part.Mass ();
		Debug.Log (String.Format("Adding part {0}", part));
		part.Attach(this);
		return true;
	}

	public void RemovePart(ShipPart part) {
		Debug.Log (String.Format("Removing part {0}", part));
		parts.Remove(part);
		mass -= part.Mass ();
		part.Detach(this);
	}

	public Ship() {
		parts = new List<ShipPart>();
		position = Vector3.zero;
		engines = new ShipEngines(this);
	}

	public void UpdateServer() {
		Assert.IsTrue(NetworkServer.active, "Tried to call server ship update on client");
		engines.UpdateServer();
	}

	public void UpdateClient() {
		Assert.IsTrue (!NetworkServer.active, "Tried to call client ship update on server");
		engines.UpdateClient();
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
		SpawnParts();
		return ship;
	}

	public GameObject Spawn(string name) {
		ship = SpawnShip(name);
		SpawnParts();
		return ship;
	}

	public GameObject Attach(GameObject other) {
		ship = other;
		SpawnParts();
		return other;
	}

	private GameObject SpawnShip() {
		return Spawner().Spawn();
	}

	private GameObject SpawnShip(string name) {
		return Spawner().Spawn(name);
	}

	private void SpawnParts() {
		input = ship.GetComponent<PlayerInput>();
		var updater = ship.AddComponent<ShipUpdater>();
		updater.ship = this;

		foreach (var part in parts) {
			var obj = part.Spawn(this);
			Debug.Log (String.Format ("adding obj {0} to ship {1} (part {2})", obj, ship, part));
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