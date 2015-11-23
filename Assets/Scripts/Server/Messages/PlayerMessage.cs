using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerMessage : MessageBase {
	public Player player;
	public PlayerMessage() {}
	public PlayerMessage(Player p) {player = p;}
	
	public override void Serialize(NetworkWriter writer) {
		writer.Write (player.Username);
	}
	
	public override void Deserialize(NetworkReader reader) {
		player = new Player(reader.ReadString ());
	}
}
