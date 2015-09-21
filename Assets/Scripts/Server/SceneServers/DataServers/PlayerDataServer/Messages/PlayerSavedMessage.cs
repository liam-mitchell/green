using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSavedMessage : PlayerMessage {
	public PlayerSavedMessage() {}
	public PlayerSavedMessage(Player p) : base(p) {}
}
