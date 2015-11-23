using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerDataPeer {
	private NetworkClient client;
	public NetworkClient Client {get {return client;}}

	public PlayerDataPeer() {
		client = new NetworkClient();
		client.Configure(ConnectionConfiguration.GetConfiguration(), 5);
	}

	public void RegisterHandlers(NetworkMessageDelegate onPlayerSaved, NetworkMessageDelegate onPlayerNotSaved) {
		client.RegisterHandler((short)MessageTypes.PLAYER_SAVED, onPlayerSaved);
		client.RegisterHandler((short)MessageTypes.PLAYER_NOT_SAVED, onPlayerNotSaved);
	}
	
	public void Connect() {
		var info = ServerInfo.GetHost ("PlayerData");
		client.Connect (info.host, info.port);
	}

	public void SavePlayer(PlayerData data) {
		client.SendByChannel ((short)MessageTypes.SAVE_PLAYER, new SavePlayerMessage(data), 0);
	}
}
