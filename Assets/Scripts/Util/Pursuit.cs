using UnityEngine;
using System.Collections;

public class Pursuit {
	static public Quaternion RotateTowards(GameObject pursuer, GameObject target, float maxAngle) {
		var targetRotation = Quaternion.LookRotation(target.transform.position - pursuer.transform.position);
		return Quaternion.RotateTowards(pursuer.transform.rotation, targetRotation, maxAngle);
	}
}
