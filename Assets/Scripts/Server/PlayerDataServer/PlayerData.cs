using UnityEngine;
using System.Collections;
using SimpleJSON;

public class PlayerData {
	public Player player;
	public Vector3 position;
	public JSONArray ship;
	
	public PlayerData(Player p, Vector3 pos) {
		player = p;
		position = pos;
	}

	public PlayerData(Player p, Vector3 pos, JSONArray s) {
		player = p;
		position = pos;
		ship = s;
	}
}
