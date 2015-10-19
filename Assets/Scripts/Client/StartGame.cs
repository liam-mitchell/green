using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System;

public class StartGame : MonoBehaviour {
	public InputField input;
	public PlayerClient client;

	private PlayerDataClient playerDataClient;

	void Start() {
		client.enabled = true;
	}

	public void OnClick() {
		if (!(input.text.Length == 0)) {
			Debug.Log ("Creating player");
			var player = new Player(input.text);
			client.SetPlayer (player);

			playerDataClient = new PlayerDataClient();
			playerDataClient.RegisterHandlers(OnPlayerCreated, OnPlayerNotCreated, OnConnected);
			playerDataClient.Connect ();
		}
	}

	public void OnConnected(NetworkMessage msg) {
		playerDataClient.CreatePlayer(client.player);
	}

	public void OnPlayerCreated(NetworkMessage msg) {
		Debug.Log ("Player created");
		client.ConnectToScene ("Region");
	}

	public void OnPlayerNotCreated(NetworkMessage msg) {
		Debug.Log ("Player not created");
		// error handling yeah
	}
}
