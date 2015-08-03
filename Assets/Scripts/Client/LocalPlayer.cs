using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LocalPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var netID = GetComponent<NetworkIdentity>();
		if (netID.isLocalPlayer) {
			Debug.Log("Local player detected!");
			transform.Find("Camera").gameObject.SetActive(true);
			SharedStart();
		}
		else if (NetworkServer.active) {
			Debug.Log ("Started player on server!");
			SharedStart();
		}
	}

	private void SharedStart() {
		GetComponent<SharedPlayerMovement>().enabled = true;
		GetComponent<SharedInput>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
