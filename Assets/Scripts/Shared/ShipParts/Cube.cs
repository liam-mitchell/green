using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Cube : ShipPart {
	public const string Name = "Cube";

	public Cube() {}
	public Cube(Vector3 pos, Quaternion rot) {
		position = pos;
		rotation = rot;

		Debug.Log ("creating cube");
	}

	public static ShipPart _Deserialize(NetworkReader reader) {
		var pos = reader.ReadVector3();
		var rot = reader.ReadQuaternion();
		Debug.Log (String.Format ("Deserializing cube at {0}, {1}", pos, rot));
		return new Cube(pos, rot);
	}

	public override void Serialize(NetworkWriter writer) {
		Save();

		writer.Write(Name);
		writer.Write(position);
		writer.Write(rotation);
		
		Debug.Log (String.Format ("Serializing cube at {0}, {1}", position, rotation));
	}

	public override int Mass() {
		return 1000;
	}

	protected override void Attach(Ship ship) {}
}
