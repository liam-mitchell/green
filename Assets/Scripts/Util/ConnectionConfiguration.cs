using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ConnectionConfiguration {
	public static ConnectionConfig GetConfiguration() {
		var config = new ConnectionConfig();
		config.AddChannel(QosType.ReliableSequenced);
		config.AddChannel(QosType.Unreliable);
		return config;
	}
}
