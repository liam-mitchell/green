using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class Ship : NetworkBehaviour {
	public class ShipSpawningException : Exception {
		public ShipSpawningException(string message) : base(message) {}
		public ShipSpawningException() : base() {}
	}

	public List<ShipPart> parts;
	public Vector3 position;

//	private GameObject ship;

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

	void Awake() {
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

	public void Serialize(ShipMessage message) {
		foreach (var part in parts) {
			part.Serialize(message);
		}
	}

	public void Deserialize(ShipMessage message) {
		ShipPart part;
		while ((part = ShipPart.Deserialize(gameObject, message)) != null) {
			AddPart(part);
		}
	}

	public void Spawn() {
		SpawnParts();
	}

//	private GameObject SpawnShip() {
//		return Spawner().Spawn();
//	}
//
//	private GameObject SpawnShip(string name) {
//		return Spawner().Spawn(name);
//	}

	private void SpawnParts() {
//		input = GetComponent<PlayerInput>();

		foreach (var part in parts) {
			part.Spawn(this);
		}

		engines = new ShipEngines(this);
	}

	private ShipSpawner Spawner() {
		var spawner = GameObject.Find("ShipSpawner").GetComponent<ShipSpawner>();
		if (spawner == null) {
			throw new ShipSpawningException("Unable to find ShipSpawner!");
		}

		return spawner;
	}
}