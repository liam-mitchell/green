using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ShipStatsDisplay : MonoBehaviour {
	public Text maxVelocity;
	public Text maxAcceleration;
	public Text mass;

	public Ship ship;
	
	void OnGUI() {
		if (ship != null) {
			maxVelocity.text = String.Format("Max velocity: {0}", ship.engines.MaxVelocity);
			maxAcceleration.text = String.Format ("Max acceleration: {0}", ship.engines.MaxAcceleration);
			mass.text = String.Format ("Mass: {0}", ship.Mass);
		}
	}
}
