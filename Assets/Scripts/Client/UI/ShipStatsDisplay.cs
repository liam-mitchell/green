using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ShipStatsDisplay : MonoBehaviour {
	public Text maxVelocity;
	public Text maxAcceleration;
	public Text mass;

	public Ship ship;

	void Start() {
		// TODO Find a better way to organize these without manually editing the positions of everything
		gameObject.transform.localPosition = new Vector3(-(Screen.width / 2) + 100, (Screen.height / 2) - 80);
	}

	void OnGUI() {
		if (ship != null) {
			maxVelocity.text = String.Format("Max velocity: {0}", ship.engines.MaxVelocity);
			maxAcceleration.text = String.Format ("Max acceleration: {0}", ship.engines.MaxAcceleration);
			mass.text = String.Format ("Mass: {0}", ship.Mass);
		}
	}
}
