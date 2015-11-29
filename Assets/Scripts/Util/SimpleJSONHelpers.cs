using UnityEngine;
using System.Collections;

namespace SimpleJSON {
	public class Helpers {
		public static JSONArray ToJSON(Vector3 v) {
			var json = new JSONArray();
			json[0].AsFloat = v.x;
			json[1].AsFloat = v.y;
			json[2].AsFloat = v.z;
			return json;
		}

		public static Vector3 Vector3FromJSON(JSONArray json) {
			return new Vector3(json[0].AsFloat, json[1].AsFloat, json[2].AsFloat);
		}

		public static JSONArray ToJSON(Quaternion q) {
			var json = new JSONArray();
			json[0].AsFloat = q.x;
			json[1].AsFloat = q.y;
			json[2].AsFloat = q.z;
			json[3].AsFloat = q.w;
			return json;
		}

		public static Quaternion QuaternionFromJSON(JSONArray json) {
			return new Quaternion(json[0].AsFloat, json[1].AsFloat, json[2].AsFloat, json[3].AsFloat);
		}
	}
}