using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerDataMessage : MessageBase {
	public PlayerData data;
	
	public PlayerDataMessage() {}
	public PlayerDataMessage(PlayerData d) {data = d;}

	public override void Serialize(NetworkWriter writer) {
		writer.Write(data.player.Username);
		writer.Write(data.position);
	}

	public override void Deserialize(NetworkReader reader) {
		data = new PlayerData(new Player(reader.ReadString ()), reader.ReadVector3 ());
	}
}

