using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChangePlayerShipMessage : ShipMessage {
	public Player player;

	public ChangePlayerShipMessage() : base () {}
	public ChangePlayerShipMessage(Ship s, Player p) : base(s) {
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
