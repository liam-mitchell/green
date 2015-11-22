using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Engine : ShipPart {	
	public const string Name = "Engine";
	public float velocityPerKilogram;
	public float accelerationPerKilogram;
	
	public Engine() {}
	public Engine(Vector3 pos, Quaternion rot, double vpkg, double apkg) {
		position = pos;
		rotation = rot;
		velocityPerKilogram = (float)vpkg;
		accelerationPerKilogram = (float)apkg;
	}
	
	public static ShipPart _Deserialize(NetworkReader reader) {
		var pos = reader.ReadVector3();
		var rot = reader.ReadQuaternion();
		var vpkg = reader.ReadDouble ();
		var apkg = reader.ReadDouble ();
		Debug.Log (String.Format ("Deserializing engine at {0}, {1}, {2}, {3}", pos, rot, vpkg, apkg));
		return new Engine(pos, rot, vpkg, apkg);
	}
	
	public override void Serialize(NetworkWriter writer) {
		Save();

		writer.Write(Name);
		writer.Write(position);
		writer.Write(rotation);
		writer.Write((double)velocityPerKilogram);
		writer.Write((double)accelerationPerKilogram);

		Debug.Log (String.Format ("Serializing engine at {0}, {1}", position, rotation));
	}

	public override int Mass() {
		return 1000;
	}

	public override void Attach(Ship ship) {
//		if (NetworkServer.active) {
//			ship.engines.AddEngine(this);
//		}
		ship.engines.AddEngine(this);
	}

	public override void Detach(Ship ship) {
		ship.engines.RemoveEngine(this);
	}

//	public Vector3 Force() {
//		return force * Vector3.forward;
//	}
}
