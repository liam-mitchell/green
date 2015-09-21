using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

/**
 * PlayerDataServer
 * 
 * Respond to messages related to storing and retrieving persistent player data.
 * 
 * Messages accepted:
 *   - MSG_CREATE_PLAYER       => CreatePlayerMessage
 *   - MSG_REQ_PLAYER_DATA     => RequestPlayerDataMessage
 *   - MSG_SAVE_PLAYER         => SavePlayerMessage
 * 
 * Messages sent:
 *   - MSG_PLAYER_CREATED      => PlayerCreatedMessage
 *   - MSG_PLAYER_NOT_CREATED  => PlayerNotCreatedMessage
 *   - MSG_PLAYER_DATA         => PlayerDataMessage
 *   - MSG_PLAYER_SAVED        => PlayerSavedMessage
 *   - MSG_PLAYER_NOT_SAVED    => PlayerNotSavedMessage
 */
public class PlayerDataServer : MonoBehaviour {
	private PlayerDataStorage storage;

	void Start() {
		storage = new PlayerDataStorage();
		NetworkServer.RegisterHandler((short)MessageTypes.CREATE_PLAYER, OnCreatePlayer);
		NetworkServer.RegisterHandler((short)MessageTypes.REQ_PLAYER_DATA, OnRequestPlayerData);
		NetworkServer.RegisterHandler((short)MessageTypes.SAVE_PLAYER, OnSavePlayer);
	
		NetworkServer.Configure(ConnectionConfiguration.GetConfiguration(), 5);

		var serverData = ServerInfo.GetHost ("player-data");
		if (!NetworkServer.Listen (serverData.port)) {
			Debug.Log ("Unable to start server!");
			return;
		}
		Debug.Log (String.Format("Listening on port {0}", serverData.port));
	}

	public void OnCreatePlayer(NetworkMessage msg) {
		var player = msg.ReadMessage<CreatePlayerMessage>().player;
		Debug.Log (String.Format ("Creating player {0}", player.Username));
		if (storage.CreatePlayer(player)) {
			msg.conn.SendByChannel((short)MessageTypes.PLAYER_CREATED, new PlayerCreatedMessage(), 0);
		}
		else {
			msg.conn.SendByChannel((short)MessageTypes.PLAYER_NOT_CREATED, new PlayerNotCreatedMessage(), 0);
		}
	}

	/**
	 * TODO: Validate that this comes from a server :P
	 */
	public void OnRequestPlayerData(NetworkMessage msg) {
		var username = msg.ReadMessage<RequestPlayerDataMessage>().player.Username;
		Debug.Log (String.Format ("Requesting player {0}", username));
		msg.conn.SendByChannel((short)MessageTypes.PLAYER_DATA, new PlayerDataMessage(storage.GetPlayerData (username)), 0);
	}

	/**
	 * TODO: Validate that this comes from a server :P 
	 */
	public void OnSavePlayer(NetworkMessage msg) {
		var data = msg.ReadMessage<SavePlayerMessage>().data;
		Debug.Log (String.Format ("Saving player {0}", data.player.Username));
		if (storage.SavePlayerData(data)) {
			msg.conn.SendByChannel((short)MessageTypes.PLAYER_SAVED, new PlayerSavedMessage(data.player), 0);
		}
		else {
			msg.conn.SendByChannel((short)MessageTypes.PLAYER_NOT_SAVED, new PlayerNotSavedMessage(data.player), 0);
		}
	}
}
