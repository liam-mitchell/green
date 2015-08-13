using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TargetingSystem : NetworkBehaviour {
	private enum State {UNLOCKED, LOCKING, LOCKED, UNLOCKING}
	public TargetingUI ui;
	public float maxRange;
	public float lockDelay;
	public float unlockDelay;

	private GameObject target;
	private float currentLockDelay;
	private float currentUnlockDelay;

	private State state;

	public GameObject Target() {
		if (state == State.LOCKED || state == State.UNLOCKING) {
			return target;
		}
		else {
			return null;
		}
	}

	void Start() {
		target = null;
		state = State.UNLOCKED;
	}

	void Update() {
		switch(state) {
		case State.LOCKED:
			state = UpdateLocked();
			break;
		case State.UNLOCKED:
			state = UpdateUnlocked();
			break;
		case State.UNLOCKING:
			state = UpdateUnlocking ();
			break;
		case State.LOCKING:
			state = UpdateLocking();
			break;
		default:
			break;
		}
	}

	private State UpdateUnlocked() {
		GameObject t = GetTarget();
		if (t) {
			return Locking(t);
		}
		else {
			return State.UNLOCKED;
		}
	}

	private State UpdateUnlocking() {
		GameObject t = GetTarget();
		if (t && t == target) {
			return Lock(t);
		}
		else {
			currentUnlockDelay += Time.deltaTime;
			if (currentUnlockDelay > unlockDelay) {
				return Unlock();
			}
			else {
				return State.UNLOCKING;
			}
		}
	}

	private State UpdateLocking() {
		GameObject t = GetTarget();
		if (t == target) {
			currentLockDelay += Time.deltaTime;
			if (currentLockDelay > lockDelay) {
				return Lock(t);
			}
			else {
				return State.LOCKING;
			}
		}
		else {
			return Unlock();
		}
	}

	private State UpdateLocked() {
		GameObject t = GetTarget();
		if (!t || t != target) {
			return Unlocking();
		}
		else {
			return State.LOCKED;
		}
	}

	private State Locking(GameObject t) {
		target = t;
		currentLockDelay = 0.0f;
		ui.Locking ();
		return State.LOCKING;
	}

	private State Lock(GameObject t) {
		target = t;
		ui.Lock ();
		return State.LOCKED;
	}

	private State Unlock() {
		target = null;
		ui.Unlock ();
		return State.UNLOCKED;
	}

	private State Unlocking() {
		currentUnlockDelay = 0.0f;
		return State.UNLOCKING;
	}

	[ClientCallback]
	private void UpdateUI() {
		switch(state) {
		case State.LOCKED:
			ui.Lock();
			break;
		case State.LOCKING:
			ui.Locking();
			break;
		case State.UNLOCKED:
			ui.Unlock();
			break;
		case State.UNLOCKING:
		default:
			break;
		}
	}

	private GameObject GetTarget() {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.right, out hit, maxRange, (1 << (int)Layers.TARGETABLE))) {
			return hit.collider.gameObject;
		}

		return null;
	}
}
