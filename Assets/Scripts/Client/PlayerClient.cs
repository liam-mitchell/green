using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using System.Collections;
using System;

public class PlayerClient : MonoBehaviour {
	public GameObject shipPrefab;
	public GameObject playerPrefab;
	public GameObject cameraPrefab;

	// TODO This should be a separate message class, inheriting from PlayerMessage...
	// and not using this weird ass byte encoding thing (since we can write strings...)
	public class SendPlayerMessage : MessageBase {
		private Player player;

		public SendPlayerMessage(Player p) {
			player = p;
		}

		public SendPlayerMessage() {}

		public override void Serialize(NetworkWriter writer) {
			writer.WriteBytesFull(System.Text.Encoding.UTF8.GetBytes(player.Username));
		}

		public override void Deserialize(NetworkReader reader) {
			player = new Player(System.Text.Encoding.UTF8.GetString(reader.ReadBytesAndSize()));
		}

		public string Username {get {return player.Username;}}
	}

	public class RequestPlayerMessage : MessageBase {
		public RequestPlayerMessage() {}
		public override void Serialize(NetworkWriter writer) {}
		public override void Deserialize(NetworkReader reader) {}
	}
	
	public Player player;
	private NetworkClient client;
	public NetworkClient Client {get {return client;}}

	void Awake() {
		Assert.raiseExceptions = true;

		client = new NetworkClient();
		client.RegisterHandler((short)MessageTypes.REQ_PLAYER, OnRequestPlayer);
		client.RegisterHandler((short)MessageTypes.CHANGE_SCENE, OnChangeScene);
		client.RegisterHandler((short)MessageTypes.PLAYER_SHIP_SPAWNED, OnShipSpawned);
		client.Configure(ConnectionConfiguration.GetConfiguration(), 5);
		GameObject.DontDestroyOnLoad(gameObject);
	}

	public void SetPlayer(Player p) {
		player = p;
	}

	public void ConnectToScene(string scene) {
		Disconnect();
		// Debug.Log (String.Format ("Connecting to scene: {0}", scene));
		Application.LoadLevel (scene);

		ServerData server = ServerInfo.GetHost(scene);
		if (server != null) {
			client.Connect(server.host, server.port);
		}
	}

	public void OnRequestPlayer(NetworkMessage msg) {
		msg.conn.SendByChannel ((short)MessageTypes.PLAYER, new SendPlayerMessage(player), 0);
		// Debug.Log (String.Format("OnRequestPlayer"));
		ClientScene.RegisterPrefab(shipPrefab);
		ClientScene.RegisterPrefab(playerPrefab);
		ClientScene.Ready (msg.conn);
		ClientScene.AddPlayer(0);
		// Debug.Log ("Client scene ready!");
	}

	public void OnChangeScene(NetworkMessage msg) {
		var scene = msg.ReadMessage<ChangeSceneMessage>().scene;
		// Debug.Log (String.Format ("Changing scene: {0}", scene));
		ConnectToScene(scene);
	}

	public void OnShipSpawned(NetworkMessage msg) {
		var message = msg.ReadMessage<ShipSpawnedMessage>();
		var obj = ClientScene.FindLocalObject (new NetworkInstanceId(message.id));
		var ship = obj.GetComponent<Ship>();
		ship.Deserialize(message);
		ship.Spawn();

		if (obj.GetComponent<NetworkIdentity>().isLocalPlayer) {
			var cam = (GameObject)GameObject.Instantiate(cameraPrefab);
			cam.transform.SetParent(obj.transform);
			cam.GetComponent<PlayerCamera>().player = obj;
			cam.GetComponentInChildren<ShipStatsDisplay>().ship = ship;
			cam.GetComponentInChildren<ShipActiveStatsDisplay>().ship = ship;
		}
	}

	private void Disconnect() {
		if (client.isConnected) {
			ClientScene.RemovePlayer (0);
			ClientScene.DestroyAllClientObjects();
			client.Disconnect ();
		}
	}
}
