using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

[NetworkSettings(channel = 1, sendInterval = 0.05f)]
public class InterpolatedTransform : NetworkBehaviour {
	private class SyncTransform {
		public Vector3 position;
		public Quaternion rotation;
		public float time;

		public SyncTransform(Vector3 p, Quaternion r) {
			position = p;
			rotation = r;
			time = Time.time;
		}
	}

	private List<SyncTransform> transforms;

	// Update is called once per frame
	[ClientCallback]
	void Update () {
		if (transforms == null) {
			return;
		}

		var renderDelay = 2 * (1.0f / Network.sendRate);
		var renderTime = Time.time - renderDelay;

		for (int i = 0; i < transforms.Count - 1; ++i) {
			if (transforms[i + 1].time < renderTime) {
				var last = transforms[i + 1];
				var next = transforms[i];
				var lerpTime = 1 - (next.time - renderTime) / (next.time - last.time);

				transform.position = Vector3.Lerp (last.position, next.position, lerpTime);
				transform.rotation = Quaternion.Lerp (last.rotation, next.rotation, lerpTime);
				if (i < transforms.Count - 2) {
					var trimPosition = i + 2;
					transforms.RemoveRange (trimPosition, (transforms.Count - trimPosition));
				}

				break;
			}
		}
	}

	[ServerCallback]
	void FixedUpdate() {
		SetDirtyBit(1);
	}

	public override bool OnSerialize(NetworkWriter writer, bool initial) {
		writer.Write (transform.localPosition);
		writer.Write (transform.localRotation);
		Debug.Log(String.Format ("Wrote pos and rot: {0}, {1}", transform.localPosition, transform.localRotation));
		return true;
	}

	public override void OnDeserialize(NetworkReader reader, bool initial) {
		var position = reader.ReadVector3 ();
		var rotation = reader.ReadQuaternion();

		Debug.Log(String.Format ("Read pos and rot: {0}, {1}", position, rotation));

		if (transforms == null) {
			transforms = new List<SyncTransform>();
		}

		transforms.Insert (0, new SyncTransform(position, rotation));
	}
}
