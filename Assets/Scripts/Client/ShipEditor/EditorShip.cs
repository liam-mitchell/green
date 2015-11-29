using UnityEngine;
using System.Collections;
using System;

public class EditorShip : MonoBehaviour {
	public Ship ship;
	public ShipStatsDisplay stats;

	// Use this for initialization
	void Start () {
//		ship = gameObject.GetComponent<Ship>();
		stats.ship = ship;
		Debug.Log (String.Format("Set editor ship to {0}", ship));
	}
}