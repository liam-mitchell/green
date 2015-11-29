using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;
using System;

public class Cube : ShipPart {
	public const string id = "Cube";
	public override string ID() {return id;}

	public override JSONClass ToJSON() {
		var json = new JSONClass();
		json["position"] = Helpers.ToJSON(transform.position);
		json["rotation"] = Helpers.ToJSON(transform.rotation);
		return json;
	}

	public override void FromJSON(JSONClass json) {
		transform.position = Helpers.Vector3FromJSON(json["position"].AsArray);
		transform.rotation = Helpers.QuaternionFromJSON(json["rotation"].AsArray);
	}

	public override int Mass() {
		return 1000;
	}

	public override void Attach(Ship ship) {}
	public override void Detach(Ship ship) {}
}
