using UnityEngine;
using System.Collections;

public class WeaponPrefab : MonoBehaviour {
	public static GameObject clientPrefab = (GameObject)Resources.Load("Prefabs/Parts/WeaponClientPrefab");
	public static GameObject serverPrefab = (GameObject)Resources.Load("Prefabs/Parts/WeaponServerPrefab");
}
