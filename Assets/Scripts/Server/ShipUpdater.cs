using UnityEngine;
using System.Collections;

public class ShipUpdater : MonoBehaviour {
	public Ship ship;

	// Update is called once per frame
	void Update () {
		if (ship != null) {
			ship.Update();
		}
	}
}
