using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;

public class PlayerShipMessage : ShipMessage {
	public Player player;
	
	public PlayerShipMessage() : base () {}
	public PlayerShipMessage(Player p) : base() {player = p;}
	public PlayerShipMessage(Player p, JSONArray j) : base(j) {player = p;}
	
	public override void Deserialize(NetworkReader reader) {
		base.Deserialize(reader);
		player = new Player(reader.ReadString());
	}
	
	public override void Serialize(NetworkWriter writer) {
		base.Serialize(writer);
		writer.Write (player.Username);
	}
}
