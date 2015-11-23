using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RequestPlayerShipMessage : PlayerMessage {
	public RequestPlayerShipMessage() : base() {}
	public RequestPlayerShipMessage(Player p) : base(p) {}
}
