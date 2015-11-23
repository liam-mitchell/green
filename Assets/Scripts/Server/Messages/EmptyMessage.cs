using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class EmptyMessage : MessageBase {
	public EmptyMessage() {}
	public override void Serialize(NetworkWriter writer) {}
	public override void Deserialize(NetworkReader reader) {}
}
