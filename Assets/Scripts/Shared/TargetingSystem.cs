using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class TargetingSystem : NetworkBehaviour {
	private enum State {UNLOCKED, LOCKING, LOCKED}

	public TargetingUI ui;
	public PlayerInput input;

	public float maxRange;
	public float lockDelay;
	public float unlockDelay;

	private GameObject target;
	private float currentLockDelay;
	private float currentUnlockDelay;

	private State state;

	public GameObject Target() {
		if (state == State.LOCKED && target) {
			return target.transform.root.gameObject;
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
			UpdateLocked();
			break;
		case State.UNLOCKED:
			UpdateUnlocked();
			break;
		case State.LOCKING:
			UpdateLocking();
			break;
		default:
			break;
		}
	}

	[ClientCallback]
	private void UpdateUnlocked() {
		if (input.state.locking) {
			GameObject t = GetTarget();
			if (t) {
				Locking(t);
			}
		}
	}

	[ServerCallback]
	private void UpdateLocking() {
		currentLockDelay += Time.deltaTime;
		if (currentLockDelay > lockDelay) {
			Lock();
		}
	}

	[ClientCallback]
	private void UpdateLocked() {
		// Potentially range check, unlock if necessary, etc.
		if (input.state.locking) {
			Debug.Log ("Locking!");
			GameObject t = GetTarget();
			if (!t) {
				Unlock();
			}
			else if (t != target) {
				Locking(t);
			}
		}
	}

	[ClientCallback]
	private void Locking(GameObject t) {
		CmdLocking (t.transform.root.gameObject.GetComponent<NetworkIdentity>().netId);
		Debug.Log (String.Format ("Locking onto {0}", t.transform.root.gameObject.GetComponent<NetworkIdentity>().netId));
		ui.Locking (t);
		target = t;
		Debug.Log (String.Format("Locking: {0}", t));
		state = State.LOCKING;
	}
	
	[Command]
	private void CmdLocking(NetworkInstanceId netId) {
		Debug.Log (String.Format ("Locking onto {0}", netId));
		target = NetworkServer.FindLocalObject(netId);
		Debug.Log (target);
		if (state == State.UNLOCKED) {
			currentLockDelay = 0.0f;
			state = State.LOCKING;
		}
	}

	[ServerCallback]
	private void Lock() {
		RpcLock();
		state = State.LOCKED;
	}

	[ClientRpc]
	private void RpcLock() {
		state = State.LOCKED;
		ui.Lock (target);
	}

	[ClientCallback]
	private void Unlock() {
		ui.Unlock ();
		CmdUnlock();
		state = State.UNLOCKED;
	}

	[Command]
	private void CmdUnlock() {
		target = null;
		state = State.UNLOCKED;
	}

	private GameObject GetTarget() {
		return ui.GetTarget ();
	}
}
