using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections;

public class ChangePlayerShipMessage : ShipMessage {
	public Player player;

	public ChangePlayerShipMessage() : base () {}
	public ChangePlayerShipMessage(Player p) : base() {player = p;}
	public ChangePlayerShipMessage(Player p, JSONArray j) : base(j) {player = p;}

	public override void Deserialize(NetworkReader reader) {
		base.Deserialize(reader);
		player = new Player(reader.ReadString());
	}

	public override void Serialize(NetworkWriter writer) {
		base.Serialize(writer);
		writer.Write (player.Username);
	}
}
