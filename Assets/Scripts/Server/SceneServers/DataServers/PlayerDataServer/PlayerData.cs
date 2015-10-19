using UnityEngine;
using System.Collections;

public class PlayerData {
	public Player player;
	public Vector3 position;
	public Ship ship;
	
	public PlayerData(Player p, Vector3 pos) {
		player = p;
		position = pos;
	}

	public PlayerData(Player p, Vector3 pos, Ship s) {
		player = p;
		position = pos;
		ship = s;
	}
}
