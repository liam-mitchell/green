using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

/**
 * Server management for the ship scene
 * 
 * Responsible for managing active player connections, spawning player objects on connection, etc.
 */
public class ShipServer : SceneServer {
	public GameObject newPlayerPrefab;
	public GameObject activePlayerPrefab;

	private PlayerDataClient playerData;

	/**
	 * Metadata associated with a connected player.
	 */
	private class PlayerConnection {
		public NetworkConnection connection;
		public string username;
		public GameObject ship;

		public PlayerConnection(NetworkConnection c, string u, GameObject s) {
			connection = c;
			username = u;
			ship = s;
		}
	}

	private void RegisterHandlers() {
		playerData.RegisterHandlers(
			NoOpHandler.Handle,
			NoOpHandler.Handle,
			NoOpHandler.Handle,
			NoOpHandler.Handle,
			NoOpHandler.Handle,
			OnPlayerShip,
			OnPlayerShipNotFound
		);
	}

	// Active player connections
	private List<PlayerConnection> connections;

	public GameObject GetPlayerShip(Player p) {
		var connection = FindPlayerConnection(p);
		if (connection == null) {
			return null;
		}

		return connection.ship;
	}

	public NetworkConnection GetPlayerConnection(Player p) {
		var connection = FindPlayerConnection(p);
		if (connection == null) {
			return null;
		}

		return connection.connection;
	}

	// Server-side callback immediately after NetworkManager.StartServer is called
	public override void OnStartServer() {
		Debug.Log ("Server started");

		GameObject.DontDestroyOnLoad(gameObject);

		// Register OnSendPlayer as the handler for clients sending us player information once they connect
		NetworkServer.RegisterHandler((short)MessageTypes.PLAYER, OnSendPlayer);
		connections = new List<PlayerConnection>();

		playerData = new PlayerDataClient();
		RegisterHandlers();
		playerData.Connect();
	}

	// Server-side callback after a player connects, with the connection of the new player
	public override void OnServerConnect(NetworkConnection connection) {
		Debug.Log ("Server added player");
		connection.SendByChannel ((short)MessageTypes.REQ_PLAYER, new PlayerClient.RequestPlayerMessage(),  0);
	}

	// Server-side callback after a client calls ClientScene.AddPlayer()
	// Overridden to avoid the default behaviour of spawning a player prefab and calling AddPlayerForConnection
	// since we do that ourselves, once the player has sent us their information (ie. during OnSendPlayer)
	public override void OnServerAddPlayer(NetworkConnection connection, short playerControllerId) {}

	// Message handler for SendPlayerMessages
	// Creates the player's ship, and associates the player and ship with an active connection object
	public void OnSendPlayer(NetworkMessage msg) {
		var username = msg.ReadMessage<PlayerClient.SendPlayerMessage>().Username;
		var connection = FindPlayerConnection(new Player(username));
		if (connection == null) {
			Debug.Log ("No connection found");
			connections.Add (new PlayerConnection(msg.conn, username, null));
		}
		else {
			Debug.Log ("Connection found");
			connection.connection = msg.conn;
		}

		Debug.Log (String.Format ("OnSendPlayer: {0}", username));

		playerData.RequestPlayerShip(new Player(username));
	}

	public void OnPlayerShip(NetworkMessage msg) {
		var message = msg.ReadMessage<PlayerShipMessage>();
		var ship = message.ship.Spawn(message.player.Username);
		AddPlayerShip(message.player, ship);
		var connection = FindPlayerConnection(message.player);
		var id = connection.ship.transform.parent.GetComponent<NetworkIdentity>().netId.Value;
		Debug.Log (String.Format ("Player ship found for player {0}", message.player.Username));
		connection.connection.SendByChannel((short)MessageTypes.PLAYER_SHIP_SPAWNED, new ShipSpawnedMessage(id, message.ship), 0);
	}

	public void OnPlayerShipNotFound(NetworkMessage msg) {
		var message = msg.ReadMessage<PlayerShipNotFoundMessage>();
		Debug.Log (String.Format ("Player ship not found for player {0}", message.player.Username));
		var ship = CreatePlayer();
		//AddPlayerShip(message.player, ship);

		var connection = FindPlayerConnection(message.player);
		connection.ship = ship;
		NetworkServer.AddPlayerForConnection(connection.connection, ship, 0);
	}

	private void AddPlayerShip(Player player, GameObject ship) {
		var connection = FindPlayerConnection(player);
		if (connection != null) {
			var p = (GameObject)GameObject.Instantiate(activePlayerPrefab);
			if (!NetworkServer.AddPlayerForConnection(connection.connection, p, 0)) {
				Debug.Log ("Failed to add player...");
			}
			ship.transform.parent = p.transform;
			connection.ship = ship;
			Debug.Log (String.Format("Added player ship for player {0}", player.Username));
		}
		else {
			Debug.Log (String.Format("failed to add ship for player {0}", player.Username));
		}
	}

	// Creates a ship, and adds the ship as the player for connection
	private GameObject CreatePlayer() {
		var pos = GetStartPosition();
		var ship = (GameObject)GameObject.Instantiate(newPlayerPrefab, pos.position, pos.rotation);
		return ship;
	}

	private PlayerConnection FindPlayerConnection(Player p) {
		var conn = connections.Find (c => c.username == p.Username);
		return conn;
	}
}
