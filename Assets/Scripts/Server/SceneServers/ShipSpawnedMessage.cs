using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class ShipSpawnedMessage : MessageBase {
	public UInt32 id;
	public Ship ship;
	public ShipSpawnedMessage() {
		ship = new Ship();
	}
	public ShipSpawnedMessage(UInt32 i, Ship s) {
		id = i;
		ship = s;
	}
	
	public override void Serialize(NetworkWriter writer) {
		writer.Write (id);
		ship.Serialize(writer);
	}
	
	public override void Deserialize(NetworkReader reader) {
		id = reader.ReadUInt32();
		ship.Deserialize(reader);
	}
}
