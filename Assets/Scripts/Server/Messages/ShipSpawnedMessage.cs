using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;
using System;

public class ShipSpawnedMessage : ShipMessage {
	public UInt32 id;
//	public Ship ship;
	public ShipSpawnedMessage() : base() {}
	public ShipSpawnedMessage(UInt32 i, JSONArray j) : base(j) {id = i;}
//		ship = new Ship();
//	}
//	public ShipSpawnedMessage(UInt32 i, Ship s) {
//		id = i;
//		ship = s;
//	}
//	
	public override void Serialize(NetworkWriter writer) {
		writer.Write (id);
		base.Serialize(writer);
	}
//	
	public override void Deserialize(NetworkReader reader) {
		id = reader.ReadUInt32();
		base.Deserialize(reader);
	}
}
