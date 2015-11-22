using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ShipActiveStatsDisplay : MonoBehaviour {
	public Text velocity;
	
	public Ship ship;
	
	void OnGUI() {
		if (ship != null) {
			velocity.text = String.Format("Velocity: {0}", ship.engines.Velocity);
		}
	}
}