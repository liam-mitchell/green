using UnityEngine;
using System.Collections;

public class EnginePrefab {
	public static GameObject clientPrefab = (GameObject)Resources.Load("Prefabs/Parts/EngineClientPrefab");
	public static GameObject serverPrefab = (GameObject)Resources.Load("Prefabs/Parts/EngineServerPrefab");
}