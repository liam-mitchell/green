using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerInfo {
	private const string LOCALHOST = "127.0.0.1";
	private static Dictionary<string, ServerData> info;
	static ServerInfo() {
		info = new Dictionary<string, ServerData>() {
			{"region", new ServerData(LOCALHOST, 7777)},
			{"player-data", new ServerData(LOCALHOST, 7778)}
		};
	}

	public static ServerData GetHost(string server) {
		ServerData data;
		info.TryGetValue(server, out data);
		return data;
	}
}
