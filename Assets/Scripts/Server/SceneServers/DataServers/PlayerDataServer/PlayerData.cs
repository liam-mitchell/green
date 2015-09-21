using UnityEngine;
using System.Collections;

public class PlayerData {
	public Player player;
	public Vector3 position;
	
	public PlayerData(Player p, Vector3 pos) {
		player = p;
		position = pos;
	}
}
