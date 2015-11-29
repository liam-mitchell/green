using UnityEngine;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

public class PlayerDataStorage {
	public Vector3 newPlayerPosition;

	private List<PlayerData> data;

	public PlayerDataStorage() {
		data = new List<PlayerData>();
	}

	public bool CreatePlayer(Player player) {
		var playerData = FindPlayerData(player.Username);
		if (playerData != null) {
			return false;
		}

		data.Add(new PlayerData(player, newPlayerPosition));
		return true;
	}

	public PlayerData GetPlayerData(string username) {
		return FindPlayerData(username);
	}

	public bool SavePlayerData(PlayerData newdata) {
		var olddata = FindPlayerData(newdata.player.Username);
		if (olddata == null) {
			return false;
		}

		data.Remove (olddata);
		data.Add (newdata);
		return true;
	}

	private PlayerData FindPlayerData(string username) {
		return data.Find (d => d.player.Username == username);
	}

	private PlayerData FindPlayerData(Player player) {
		return FindPlayerData(player.Username);
	}

	public JSONArray FindPlayerShip(Player player) {
		var data = FindPlayerData(player);
		if (data != null) {
			return data.ship;
		}
		else {
			return null;
		}
	}

	public bool ChangePlayerShip(Player player, JSONArray ship) {
		var data = FindPlayerData(player);
		if (data != null) {
			data.ship = ship;
			return true;
		}
		else {
			return false;
		}
	}
}
