using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CreatePlayerMessage : PlayerMessage {
	public CreatePlayerMessage() {}
	public CreatePlayerMessage(Player p) : base(p) {}
}
