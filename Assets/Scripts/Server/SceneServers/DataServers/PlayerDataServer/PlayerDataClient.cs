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

	public void RegisterHandlers(NetworkMessageDelegate onPlayerCreated,
	                             NetworkMessageDelegate onPlayerNotCreated,
	                             NetworkMessageDelegate onConnected,
	                             NetworkMessageDelegate onPlayerShipChanged,
	                             NetworkMessageDelegate onPlayerShipNotChanged,
	                             NetworkMessageDelegate onPlayerShip,
	                             NetworkMessageDelegate onPlayerShipNotFound)
	{
		client.RegisterHandler((short)MessageTypes.PLAYER_CREATED, onPlayerCreated);
		client.RegisterHandler((short)MessageTypes.PLAYER_NOT_CREATED, onPlayerNotCreated);
		client.RegisterHandler((short)MsgType.Connect, onConnected);
		client.RegisterHandler((short)MessageTypes.PLAYER_SHIP_CHANGED, onPlayerShipChanged);
		client.RegisterHandler((short)MessageTypes.PLAYER_SHIP_NOT_CHANGED, onPlayerShipNotChanged);
		client.RegisterHandler((short)MessageTypes.PLAYER_SHIP, onPlayerShip);
		client.RegisterHandler((short)MessageTypes.PLAYER_SHIP_NOT_FOUND, onPlayerShipNotFound);
	}

	public void Connect() {
		var info = ServerInfo.GetHost ("PlayerData");
		client.Connect (info.host, info.port);
	}

	public void Disconnect() {
		client.Disconnect();
	}

	public void CreatePlayer(Player player) {
		if (!client.SendByChannel((short)MessageTypes.CREATE_PLAYER, new CreatePlayerMessage(player), 0)) {
			Debug.Log ("Couldn't send player message");
		}
	}

	public void ChangePlayerShip(Player player, Ship ship) {
		if (!client.SendByChannel((short)MessageTypes.CHANGE_PLAYER_SHIP, new ChangePlayerShipMessage(ship, player), 0)) {
			Debug.Log ("Couldn't send player change ship message");
		}
	}

	public void RequestPlayerShip(Player player) {
		if (!client.SendByChannel((short)MessageTypes.REQ_PLAYER_SHIP, new RequestPlayerShipMessage(player), 0)) {
			Debug.Log ("Couldn't send player request ship message");
		}
	}

	// TODO
	// ChangePlayerShip()
	// PlayerShipChanged()
	// PlayerShipNotChanged()
}
