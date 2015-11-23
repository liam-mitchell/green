using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChangeSceneMessage : MessageBase {
	public string scene;
	public ChangeSceneMessage() {}
	public ChangeSceneMessage(string s) {scene = s;}

	public override void Serialize(NetworkWriter writer) {
		writer.Write (scene);
	}

	public override void Deserialize(NetworkReader reader) {
		scene = reader.ReadString ();
	}
}
