using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class CraftingLocation : NetworkBehaviour {
	public float entryDistance;

	private PlayerDataPeer playerData;
	private ShipServer server;

	[ServerCallback]
	void Start() {
		playerData = new PlayerDataPeer();
		playerData.RegisterHandlers(OnPlayerSaved, OnPlayerNotSaved);
		playerData.Connect();

		server = GameObject.Find("ShipServer").GetComponent<ShipServer>();
	}

	[Server]
	public void Enter(Player p) {
		Debug.Log ("Player entered crafting location!");
		if (PlayerCanEnter(p)) {
			var data = new PlayerData(p, server.GetPlayerShip(p).transform.position);
			playerData.SavePlayer (data);
		}
	}

	public bool PlayerCanEnter(Player p) {
		var ship = server.GetPlayerShip(p);
		if (ship == null) {
			return false;
		}

		if ((ship.transform.position - transform.position).magnitude > entryDistance) {
			return false;
		}

		return true;
	}

	public void OnPlayerSaved(NetworkMessage msg) {
		var player = msg.ReadMessage<PlayerSavedMessage>().player;
		var connection = server.GetPlayerConnection(player);
		Debug.Log (String.Format("Player saved: {0}", player.Username));
		connection.SendByChannel ((short)MessageTypes.CHANGE_SCENE, new ChangeSceneMessage("ShipEditor"), 0);
	}

	public void OnPlayerNotSaved(NetworkMessage msg) {
		var player = msg.ReadMessage<PlayerNotSavedMessage>().player;
		Debug.Log (String.Format("Player not saved: {0}", player.Username));
	}
}
