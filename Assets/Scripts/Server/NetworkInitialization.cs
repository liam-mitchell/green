using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkInitialization : NetworkBehaviour {
	public float sendRate;

	// Use this for initialization
	void Start () {
		if (isServer) {
			Network.sendRate = sendRate;
		}
	}
}
