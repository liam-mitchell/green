using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Base ShipPart class.
 * 
 * Handles serialization of parts, and spawning of GameObjects, delegating to
 * appropriate abstract functions where necessary.
 * 
 * In order to create a new ShipPart, a few steps are required:
 *   - Add a unique string "id" to your class (probably the class name)
 *   - Override the ID() function to return your "id" variable
 *   - Add a deserializer to @deserializers which adds a component of the appropriate type
 *      - The key for this should be your class's "id" variable
 *   - Override Attach(), Detach(), ToJSON() and FromJSON() appropriately
 * 
 * You must also create a Prefab class which loads the appropriate prefabs from the Resources
 * folder, and set @clientPrefab and @serverPrefab in Awake(). Technically, this could be done
 * every time a part is constructed, but it's nice to just cache it in another small class.
 * 
 * Right now, overriding Mass() is also required. That's stupid though, and will change.
 * For now, just return 1000.
 * 
 * See Cube and CubePrefab for a simple example.
 */
public abstract class ShipPart : MonoBehaviour {
	public delegate ShipPart Deserializer(GameObject obj);
	private static Dictionary<string, Deserializer> deserializers = new Dictionary<string, Deserializer>() {
		{Cube.id, (obj) => obj.AddComponent<Cube>()},
		{Engine.id, (obj) => obj.AddComponent<Engine>()},
//		{Weapon.Name, (obj) => obj.AddComponent<Weapon>()},
	};

	public GameObject clientPrefab;
	public GameObject serverPrefab;
	[HideInInspector]
	public GameObject part;

	public abstract string ID();

	protected Vector3 position;
	protected Quaternion rotation;

	public bool Lock(Ship ship) {
		return true; // this should probably... like... check a distance or something T.T
	}

	public GameObject Spawn(Ship ship) {
		Debug.Log (String.Format ("Spawning part for {0}: {1}, {2}", this, clientPrefab, serverPrefab));
		if (!NetworkServer.active) {
			part = (GameObject)GameObject.Instantiate(clientPrefab, position, rotation);
		}
		else {
			part = (GameObject)GameObject.Instantiate(serverPrefab, position, rotation);
		}

		Debug.Log (String.Format ("part @{0}, ship @{1}", part.transform.position, ship.transform.position));
		part.transform.SetParent(ship.transform, false);
		Debug.Log (String.Format ("part now @{0}, ship now @{1}", part.transform.position, ship.transform.position));
		return part;
	}

	public static ShipPart Deserialize(GameObject obj, ShipMessage msg) {
		var json = msg.NextPart();
		if (json == null) {
			return null;
		}

		Debug.Log(String.Format ("Deserializing part id {0}: {1}", json["id"].Value, json.ToString()));
		var id = json["id"].Value;
		var part = deserializers[id](obj);
		Debug.Log (String.Format ("Deserializing part: {0}", json.ToString ()));
		part.FromJSON (json["data"].AsObject);
		return part;
	}

	public void Serialize(ShipMessage msg) {
		if (part != null) {
			position = part.transform.position;
			rotation = part.transform.rotation;
		}

		var json = new JSONClass();
		json["id"] = ID();
		json["data"] = ToJSON();
		Debug.Log (String.Format ("Serializing part: {0}", json.ToString ()));
		msg.AddPart (json);
	}

	public ShipPart Clone(GameObject obj) {
		var msg = new ShipMessage();
		Serialize(msg);
		return Deserialize(obj, msg);
	}

	// This probably shouldn't be here, but instead just be a stat of most/all parts.
	// We'll see.
	public abstract int Mass();

	// Attach and detach from @ship.
	// These functions should adjust statistics on the ship appropriately, as well as adding or
	// removing themselves from any necessary lists (ie. engines must add themselves to ShipEngines,
	// and remove themselves when detached).
	public abstract void Attach(Ship ship);
	public abstract void Detach(Ship ship);

	// Serialize class to JSON.
	// These functions must mirror each other exactly (ie. FromJSON(ToJSON()) should produce the same
	// part) and serialize all necessary data to recreate the part on another machine.
	//
	// The one catch: these functions should use the @position and @rotation variables, rather than
	// writing their part's data directly, since this is called before Spawn() (and therefore no
	// part is attached).
	public abstract JSONClass ToJSON();
	public abstract void FromJSON(JSONClass json);
}
