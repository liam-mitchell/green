using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;
using System;

public class Weapon : ShipPart {
	public const string id = "Weapon";
	public override string ID() {return id;}
	
	public override JSONClass ToJSON() {
		var json = new JSONClass();
		json["position"] = Helpers.ToJSON(position);
		json["rotation"] = Helpers.ToJSON(rotation);
		return json;
	}
	
	public override void FromJSON(JSONClass json) {
		position = Helpers.Vector3FromJSON(json["position"].AsArray);
		rotation = Helpers.QuaternionFromJSON(json["rotation"].AsArray);
	}
	
	public override int Mass() {
		return 1000;
	}
	
	public override void Attach(Ship ship) {}
	public override void Detach(Ship ship) {}
	
	void Awake() {
		clientPrefab = WeaponPrefab.clientPrefab;
		serverPrefab = WeaponPrefab.serverPrefab;
	}
}
