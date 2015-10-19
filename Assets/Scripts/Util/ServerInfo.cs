using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerInfo {
	private const string LOCALHOST = "127.0.0.1";
	private static Dictionary<string, ServerData> info;
	static ServerInfo() {
		info = new Dictionary<string, ServerData>() {
			{"Region", new ServerData(LOCALHOST, 7777)},
			{"PlayerData", new ServerData(LOCALHOST, 7778)}
		};
	}

	public static ServerData GetHost(string server) {
		ServerData data;
		if (info.TryGetValue(server, out data)) {
			return data;
		}

		return null;
	}
}
