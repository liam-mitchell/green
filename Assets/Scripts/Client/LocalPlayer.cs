using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LocalPlayer : MonoBehaviour {
	public List<MonoBehaviour> sharedComponents;
	public List<MonoBehaviour> localComponents;

	// Use this for initialization
	void Start () {
		var netID = GetComponent<NetworkIdentity>();
		if (netID.isLocalPlayer) {
			Debug.Log("Local player detected!");
			transform.Find("Camera").gameObject.SetActive(true);
			LocalStart();
			SharedStart();
		}
		else if (NetworkServer.active) {
			Debug.Log ("Started player on server!");
			SharedStart();
		}
		Debug.Log ("Started local player");
	}

	private void SharedStart() {
		foreach (var c in sharedComponents) {
			c.enabled = true;
		}
	}

	private void LocalStart() {
		foreach (var c in localComponents) {
			c.enabled = true;
			Debug.Log (c);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
