using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class ShipPart : MonoBehaviour {
	public delegate ShipPart Deserializer(GameObject obj);
	private static Dictionary<string, Deserializer> deserializers = new Dictionary<string, Deserializer>() {
		{Cube.id, (obj) => obj.AddComponent<Cube>()},
		{Engine.id, (obj) => obj.AddComponent<Engine>()},
//		{Weapon.Name, (obj) => obj.AddComponent<Weapon>()},
	};

	// TODO these are null after serialization, we need them to not be for spawning...
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
			part = (GameObject)GameObject.Instantiate(clientPrefab);
		}
		else {
			part = (GameObject)GameObject.Instantiate(serverPrefab);
		}

		part.transform.SetParent(ship.transform);
		Attach(ship);
		return part;
	}

	public static ShipPart Deserialize(GameObject obj, ShipMessage msg) {
		var json = msg.NextPart();
		if (json == null) {
			return null;
		}

		Debug.Log(String.Format ("Deserializing part id {0}: {1}", json["id"].Value, json.ToString()));
		var part = deserializers[json["id"].Value](obj);
		Debug.Log (String.Format ("Deserializing part: {0}", json.ToString ()));
		part.FromJSON (json["data"].AsObject);
		return part;
	}

	public void Serialize(ShipMessage msg) {
		var json = new JSONClass();
		json["id"] = ID();
		json["data"] = ToJSON();
		Debug.Log (String.Format ("Serializing part: {0}", json.ToString ()));
		msg.AddPart (json);
	}
	
	public abstract int Mass();

	public abstract void Attach(Ship ship);
	public abstract void Detach(Ship ship);

	public abstract JSONClass ToJSON();
	public abstract void FromJSON(JSONClass json);
}
