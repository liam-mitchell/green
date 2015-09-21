using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class CraftingSystem : NetworkBehaviour {
	public PlayerInput input;
	
	// Update is called once per frame
	void Update () {
		if (input.state.craftingMode) {
			var craftingLocation = FindCraftingLocation();
			var player = GameObject.Find("PlayerClient").GetComponent<PlayerClient>().player;

			if (craftingLocation == null) {
				Debug.Log ("No crafting location found");
				return;
			}

			if (player == null) {
				Debug.Log ("No player client found");
				return;
			}

			Debug.Log ("Entering crafting mode");
			CmdEnter(player.Username);
		}
	}

	[Command]
	private void CmdEnter(string username) {
		var craftingLocation = FindCraftingLocation();
		
		if (craftingLocation == null) {
			Debug.Log ("No crafting location found");
			return;
		}

		var player = new Player(username);
		Debug.Log ("Entering crafting mode");
		craftingLocation.Enter(player);
	}

	private CraftingLocation FindCraftingLocation() {
		var objects = new List<GameObject>(GameObject.FindGameObjectsWithTag("CraftingLocation"));
		objects.RemoveAll (o => o.GetComponent<CraftingLocation>() == null || Distance(o) > o.GetComponent<CraftingLocation>().entryDistance);
		if (objects.Count == 0) {
			return null;
		}

		objects.Sort ((o1, o2) => (int)(Math.Ceiling (Distance(o2) - Distance(o1))));
		return objects[0].GetComponent<CraftingLocation>();
	}

	private float Distance(GameObject o) {
		Debug.Log (String.Format ("Checking distance vs {0}: {1}", o, (o.transform.position - transform.position).magnitude));
		return (o.transform.position - transform.position).magnitude;
	}
}
