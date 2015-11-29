using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using System.Collections;

public class ShipMessage : MessageBase {
	private JSONArray json;
	private int current;

	public JSONArray JSON {get {return json;}}

	public ShipMessage() {
		json = new JSONArray();
		current = 0;
	}

	public ShipMessage(JSONArray j) {
		json = j;
	}

	public void AddPart(JSONClass part) {
		json[json.Count] = part;
	}

	public JSONClass NextPart() {
		if (current >= json.Count) {
			return null;
		}

		return json[current++].AsObject;
	}
	
	public override void Serialize(NetworkWriter writer) {
		Debug.Log (String.Format("Serializing ship: {0}", json.SaveToBase64()));
		Debug.Log (String.Format("json.ToString(): {0}", json.ToString()));
		writer.Write(json.SaveToBase64());
	}
	
	public override void Deserialize(NetworkReader reader) {
		var str = reader.ReadString ();
		Debug.Log (String.Format ("Deserializing ship: {0}", str));
		json = (JSONArray)JSONNode.LoadFromBase64(str);
	}
}
