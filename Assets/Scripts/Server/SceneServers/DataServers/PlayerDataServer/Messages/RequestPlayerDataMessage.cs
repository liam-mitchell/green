using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RequestPlayerDataMessage : PlayerMessage {
	public RequestPlayerDataMessage() {}
	public RequestPlayerDataMessage(Player p) : base(p) {}
}
