using UnityEngine;
using System.Collections;
using System;

public class ShipEngines {
	private Ship ship;
//	private int kilogramMetersPerSecond;
	private float velocityPerKilogram;
	private float accelerationPerKilogram;

	private float velocity;

	public ShipEngines(Ship s) {
		ship = s;
		velocityPerKilogram = 0;
		accelerationPerKilogram = 0;
		velocity = 0;
		// TODO register handler for motion inputs
	}

	public void Update() {
		var aship = accelerationPerKilogram / ship.Mass;
		var fship = ship.Mass * aship;

		var vmax = velocityPerKilogram / ship.Mass;
		var fdrag = fship * velocity / vmax;

		var f = fship - fdrag;
		var a = f / ship.Mass;
		velocity += a * Time.deltaTime;

		ship.gameObject.transform.position += ship.gameObject.transform.forward * velocity * Time.deltaTime;
		Debug.Log (String.Format("Updated ship engines: velocity {0}, a {1}, f {2}, pos {3}, ship mass {4}, vmax {5}", velocity, a, f, ship.gameObject.transform.position, ship.Mass, vmax));
	}

	public void AddEngine(Engine engine) {
		velocityPerKilogram += engine.velocityPerKilogram;
		accelerationPerKilogram += engine.accelerationPerKilogram;
		Debug.Log (String.Format ("Added engine: vpkg {0}, apkg {1}", engine.velocityPerKilogram, engine.accelerationPerKilogram));
	}

	public void RemoveEngine(Engine engine) {
		velocityPerKilogram -= engine.velocityPerKilogram;
		accelerationPerKilogram -= engine.accelerationPerKilogram;
	}
}
