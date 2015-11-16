using UnityEngine;
using System.Collections;

public class EnginePrefab : ShipPartPrefab {
	protected override ShipPart CreatePart (GameObject obj) {
		var part = new Engine();
		part.velocityPerKilogram = 1000000.0f;
		part.accelerationPerKilogram = part.velocityPerKilogram / 10.0f;
		part.part = obj;
		return part;
	}
}
