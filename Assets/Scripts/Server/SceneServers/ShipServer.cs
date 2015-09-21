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
	public GameObject shipPrefab;

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

	// Active player connections
	private List<PlayerConnection> connections;

	public GameObject GetPlayerShip(Player p) {
		var connection = FindPlayerConnection(p.Username);
		if (connection == null) {
			return null;
		}

		return connection.ship;
	}

	public NetworkConnection GetPlayerConnection(Player p) {
		var connection = FindPlayerConnection(p.Username);
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
		Debug.Log (String.Format ("OnSendPlayer: {0}", username));

		var ship = CreatePlayer();
		NetworkServer.AddPlayerForConnection(msg.conn, ship, 0);
		connections.Add (new PlayerConnection(msg.conn, username, ship));
	}

	// Creates a ship, and adds the ship as the player for connection
	private GameObject CreatePlayer() {
		var pos = GetStartPosition();
		var ship = (GameObject)GameObject.Instantiate(shipPrefab, pos.position, pos.rotation);
		return ship;
	}

	private PlayerConnection FindPlayerConnection(string username) {
		return connections.Find (c => c.username == username);
	}
}
