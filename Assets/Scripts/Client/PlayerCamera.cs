using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

// PlayerCamera
// Responsible for managing which objects are visible to the player and which are not.
// Must attach to player at startup.
public class PlayerCamera : MonoBehaviour {
	public GameObject CameraPrefab;

	private Camera cam;
	private NetworkIdentity networkIdentity;
	// Use this for initialization
	void Start () {
		networkIdentity = GetComponent<NetworkIdentity>();
		if (networkIdentity.isLocalPlayer) {
			Debug.Log("Local player found!");
			cam = gameObject.AddComponent<Camera>();
			cam.enabled = true;
			//gameObject.GetComponent<Renderer>().enabled = false;
			// GetComponent<Renderer>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (cam) {
			cam.transform.position = transform.position;
			cam.transform.rotation = transform.rotation;
		}
	}
}
