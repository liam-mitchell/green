using UnityEngine;
using System.Collections;
using System;

public class ShipEngines {
	private Ship ship;
	private float velocityPerKilogram;
	private float accelerationPerKilogram;

	private float velocity;

	public float MaxVelocity {get {return velocityPerKilogram / ship.Mass;}}
	public float MaxAcceleration {get {return accelerationPerKilogram / ship.Mass;}}
	public float Velocity {get {return velocity;}}

	public ShipEngines(Ship s) {
		ship = s;
		velocityPerKilogram = 0;
		accelerationPerKilogram = 0;
		velocity = 0;
	}

	public void UpdateServer() {
		Accelerate();
		ship.gameObject.transform.position += ship.gameObject.transform.forward * velocity * Time.deltaTime;
	}

	public void UpdateClient() {
		Accelerate(); // just update velocity for display, don't actually move - the server does that
	}

	public void AddEngine(Engine engine) {
		velocityPerKilogram += engine.velocityPerKilogram;
		accelerationPerKilogram += engine.accelerationPerKilogram;
		Debug.Log (String.Format ("Added engine: vpkg {0}, apkg {1}, now @({2}, {3})", engine.velocityPerKilogram, engine.accelerationPerKilogram, velocityPerKilogram, accelerationPerKilogram));
	}

	public void RemoveEngine(Engine engine) {
		velocityPerKilogram -= engine.velocityPerKilogram;
		accelerationPerKilogram -= engine.accelerationPerKilogram;
	}

	private void Accelerate() {
		if (ship.input == null || accelerationPerKilogram == 0 || velocityPerKilogram == 0) {
			Debug.Log (String.Format ("Early exit from Accelerate(): {0}, {1}, {2}", ship.input, accelerationPerKilogram, velocityPerKilogram));
			return;
		}

		var aship = accelerationPerKilogram / ship.Mass * ship.input.state.accelerate;
		var fship = ship.Mass * aship;
		
		var vmax = velocityPerKilogram / ship.Mass;
		var fdrag = fship * velocity / vmax * ship.input.state.accelerate;
		
		var f = fship - fdrag;
		var a = f / ship.Mass;
		velocity += a * Time.deltaTime;
		Debug.Log (String.Format("Updated ship engines: velocity {0}, a {1}, f {2}, pos {3}, ship mass {4}, vmax {5}, accelerate {6}", velocity, a, f, ship.gameObject.transform.position, ship.Mass, vmax, ship.input.state.accelerate));
	}
}
