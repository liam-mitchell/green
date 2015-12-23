using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;
using System;

public class Engine : ShipPart {	
	public const string id = "Engine";
	public override string ID() {return id;}

	public float velocityPerKilogram;
	public float accelerationPerKilogram;
	
	public Engine() {}
	public Engine(Vector3 pos, Quaternion rot, double vpkg, double apkg) {
		position = pos;
		rotation = rot;
		velocityPerKilogram = (float)vpkg;
		accelerationPerKilogram = (float)apkg;
	}

	public override JSONClass ToJSON() {
		var json = new JSONClass();
		json["position"] = Helpers.ToJSON(position);
		json["rotation"] = Helpers.ToJSON(rotation);
		json["vpkg"].AsFloat = velocityPerKilogram;
		json["apkg"].AsFloat = accelerationPerKilogram;
		return json;
	}
	
	public override void FromJSON(JSONClass json) {
		position = Helpers.Vector3FromJSON(json["position"].AsArray);
		rotation = Helpers.QuaternionFromJSON(json["rotation"].AsArray);
		velocityPerKilogram = json["vpkg"].AsFloat;
		accelerationPerKilogram = json["apkg"].AsFloat;
	}

	public override int Mass() {
		return 1000;
	}

	public override void Attach(Ship ship) {
		ship.engines.AddEngine(this);
	}

	public override void Detach(Ship ship) {
		ship.engines.RemoveEngine(this);
	}

	void Awake() {
		clientPrefab = EnginePrefab.clientPrefab;
		serverPrefab = EnginePrefab.serverPrefab;
	}
}
