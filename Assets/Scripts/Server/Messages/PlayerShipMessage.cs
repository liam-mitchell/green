using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerShipMessage : ShipMessage {
	public Player player;
	
	public PlayerShipMessage() : base () {}
	public PlayerShipMessage(Ship s, Player p) : base(s) {
		player = p;
	}
	
	public override void Deserialize(NetworkReader reader) {
		base.Deserialize(reader);
		player = new Player(reader.ReadString());
	}
	
	public override void Serialize(NetworkWriter writer) {
		base.Serialize(writer);
		writer.Write (player.Username);
	}
}
