using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player {
	private string username;
	public string Username { get {return username;} }

	public Player(string u) {
		username = u;
	}
}
