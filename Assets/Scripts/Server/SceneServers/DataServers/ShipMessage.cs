using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ShipMessage : MessageBase {
	public Ship ship;

	public ShipMessage(Ship s) {ship = s;}
	public ShipMessage() {}
	
	public override void Serialize(NetworkWriter writer) {
		ship.Serialize(writer);
	}
	
	public override void Deserialize(NetworkReader reader) {
		ship = new Ship();
		ship.Deserialize(reader);
	}
}
