using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ShipActiveStatsDisplay : MonoBehaviour {
	public Text velocity;
	
	public Ship ship;

	void Start() {
		// TODO Find a better way to organize these without manually editing the positions of everything
		gameObject.transform.localPosition = new Vector3(-(Screen.width / 2) + 100, (Screen.height / 2) - 20);
	}

	void OnGUI() {

		if (ship != null) {
			velocity.text = String.Format("Velocity: {0}", ship.engines.Velocity);
		}
	}
}