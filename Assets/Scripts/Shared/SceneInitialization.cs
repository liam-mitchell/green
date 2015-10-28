using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SceneInitialization : MonoBehaviour {
	public bool server;

	// Use this for initialization
	void Start () {
		if (server) {
			StartServer();
		}
	}

	public void StartServer() {
		NetworkServer.Configure(ConnectionConfiguration.GetConfiguration(), 5);
		if (!GetComponent<NetworkManager>().StartServer ()) {
			Debug.Log("Unable to start server!");
		}
		else {
			Debug.Log ("Started server!");
		}
	}
}
