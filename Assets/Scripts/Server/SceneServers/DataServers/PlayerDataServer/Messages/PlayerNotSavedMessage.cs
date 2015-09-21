using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNotSavedMessage : PlayerMessage {
	public PlayerNotSavedMessage() {}
	public PlayerNotSavedMessage(Player p) : base(p) {}
}
