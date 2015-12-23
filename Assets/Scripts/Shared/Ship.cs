using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Base Ship class.
 * 
 * Maintains a list of ship parts, as well as containers for each type of ship part
 * (engines, weapons, etc). ShipParts receive a reference to their Ship when they
 * are attached via ShipPart.Attach().
 * 
 * This ship can be serialized into a ShipMessage (or subclass). To deserialize, an
 * empty Ship should be created (with @input set appropriately), then Ship.Deserialize()
 * should be called on the received ShipMessage.
 * 
 * After deserialization, the parts are attached, but their GameObjects have not been
 * spawned. Once the GameObjects are required, Ship.Spawn() must be called, which calls
 * ShipPart.Spawn() for all attached parts.
 * 
 * Must be linked in inspector:
 *   @input (PlayerInput)
 */
public class Ship : NetworkBehaviour {
	public class ShipSpawningException : Exception {
		public ShipSpawningException(string message) : base(message) {}
		public ShipSpawningException() : base() {}
	}

	public List<ShipPart> parts;
	public Vector3 position;

	private float mass;
	public float Mass {get {return mass;}}

	public ShipEngines engines;
	public PlayerInput input;

	// Adds @part to the ship.
	// Calls part.Attach() with a reference to this ship, allowing the part
	// to adjust any necessary stats and add itself to any necessary lists.
	public bool AddPart(ShipPart part) {
		parts.Add(part);
		mass += part.Mass ();
		Debug.Log (String.Format("Adding part {0}", part));
		part.Attach(this);
		return true;
	}

	// Removes @part from the ship, if it is attached.
	// Calls part.Detach() if found (see AddPart).
	public void RemovePart(ShipPart part) {
		Debug.Log (String.Format("Removing part {0}", part));
		if (parts.Remove(part)) {
			mass -= part.Mass ();
			part.Detach(this);
		}
	}

	void Awake() {
		parts = new List<ShipPart>();
		position = Vector3.zero;
		engines = new ShipEngines(this);
	}

	void Update() {
		Debug.Log ("Updating ship engines...");
		if (NetworkServer.active) {
			engines.UpdateServer();
		}
		else {
			engines.UpdateClient();
		}
	}

	public void UpdateServer() {
		Assert.IsTrue(NetworkServer.active, "Tried to call server ship update on client");
		engines.UpdateServer();
	}

	public void UpdateClient() {
		Assert.IsTrue (!NetworkServer.active, "Tried to call client ship update on server");
		engines.UpdateClient();
	}

	// Serialize each part into the message
	public void Serialize(ShipMessage message) {
		foreach (var part in parts) {
			part.Serialize(message);
		}
	}

	// Deserialize parts one at a time, adding them one by one.
	public void Deserialize(ShipMessage message) {
		ShipPart part;
		while ((part = ShipPart.Deserialize(gameObject, message)) != null) {
			AddPart(part);
		}
	}

	// Spawn parts' associated GameObjects
	public void Spawn() {
		SpawnParts();
	}

	private void SpawnParts() {
		foreach (var part in parts) {
			part.Spawn(this);
		}
	}
}