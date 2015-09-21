using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerDataClient {
	private NetworkClient client;
	public NetworkClient Client {get {return client;}}

	public PlayerDataClient() {
		client = new NetworkClient();
		client.Configure(ConnectionConfiguration.GetConfiguration(), 5);
	}

	public void RegisterHandlers(NetworkMessageDelegate onPlayerCreated, NetworkMessageDelegate onPlayerNotCreated, NetworkMessageDelegate onConnected) {
		client.RegisterHandler((short)MessageTypes.PLAYER_CREATED, onPlayerCreated);
		client.RegisterHandler((short)MessageTypes.PLAYER_NOT_CREATED, onPlayerNotCreated);
		client.RegisterHandler((short)MsgType.Connect, onConnected);
	}

	public void Connect() {
		var info = ServerInfo.GetHost ("player-data");
		client.Connect (info.host, info.port);
	}

	public void CreatePlayer(Player player) {
		if (!client.SendByChannel((short)MessageTypes.CREATE_PLAYER, new CreatePlayerMessage(player), 0)) {
			Debug.Log ("Couldn't send player message");
		}
	}

	public void ChangePlayerPrefab(Player player, GameObject prefab) {

	}
}
