using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ShipUpdater : MonoBehaviour {
	public Ship ship;

	// Update is called once per frame
	void Update () {
		if (ship != null) {
			if (NetworkServer.active) {
				ship.UpdateServer();
			}
			else {
				ship.UpdateClient();
			}
		}
	}
}
