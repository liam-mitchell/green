using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class EditorPartsMenu : MonoBehaviour {
	public List<GameObject> partsPrefabs;
	public Image menu;
	public int partsShown;

	public int startOffset;
	public int partSpacing;

	public ShipPartSpawner spawner;

	private List<ClickablePart> parts;
	private Ship ship;
	private PlayerDataClient dataClient;
	private PlayerClient playerClient;

	private class ClickablePart {
		public GameObject obj;
		public ShipPart part;
		public ClickablePart(ShipPart p, GameObject o) {
			part = p;
			obj = o;
			Debug.Log (String.Format ("Created clickable part with name {0}", part.name));
		}
	}

	// Use this for initialization
	void Start () {
		parts = new List<ClickablePart>();
		ship = new Ship();

		playerClient = GameObject.Find ("PlayerClient").GetComponent<PlayerClient>();
		if (playerClient != null) {
			dataClient = new PlayerDataClient();
			RegisterHandlers();
			dataClient.Connect();
		}

		CreateParts();
	}

	private void RegisterHandlers() {
		dataClient.RegisterHandlers(
			NoOpHandler.Handle,
			NoOpHandler.Handle,
			NoOpHandler.Handle,
			OnPlayerShipChanged,
			OnPlayerShipNotChanged,
			NoOpHandler.Handle,
			NoOpHandler.Handle
		);
	}

	private void SaveShip() {
		if (dataClient.Client.isConnected) {
			dataClient.ChangePlayerShip(playerClient.player, ship);
			Debug.Log ("Saved player ship");
		}
		else {
			Debug.Log ("Didn't save player ship, not connected to data server");
		}
	}

	private void OnPlayerShipChanged(NetworkMessage msg) {
		dataClient.Disconnect();
		playerClient.ConnectToScene("Region");
		Debug.Log ("Player ship changed");
	}

	private void OnPlayerShipNotChanged(NetworkMessage msg) {
		NoOpHandler.Handle(msg);
		Debug.Log ("Player ship not changed");
	}

	private void CreateParts() {
		var center = menu.rectTransform.rect.center;
		var centerBottom = new Vector3(center.x, 0);
		
		var pos = menu.rectTransform.TransformPoint (center.x, center.y * 2 - startOffset, 0);
		var step = (menu.rectTransform.TransformPoint (centerBottom) - menu.rectTransform.TransformPoint (center.x, pos.y, 0)) / partsShown;
		
		var zero = menu.rectTransform.TransformPoint (Vector3.zero);
		var right = menu.rectTransform.TransformPoint (new Vector3(center.x * 2, 0));
		var maxSize = (right - zero).magnitude;

		for (int i = 0; i < partsShown && i < spawner.parts.Count; ++i, pos += step) {
			var part = new ShipPart(spawner.parts[i].name);
			var obj = part.Spawn ();

			obj.transform.SetParent(menu.rectTransform.parent);
			obj.layer |= (int)Layers.IGNORE_RAYCAST;
			
			var size = obj.GetComponent<Renderer>().bounds.size.magnitude + partSpacing;
			var scale = maxSize / size;

			obj.transform.localScale *= scale;
			obj.transform.position = pos;
			obj.transform.rotation = Quaternion.identity;

			parts.Add (new ClickablePart(part, obj));
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var clickable in parts) {
			if (Cursor.ClickDown(clickable.obj)) {
				Debug.Log (String.Format ("Clicked something: {0}", clickable.obj));
				Debug.Log (String.Format ("part: {0}", clickable.part.name));

				var shipPart = new ShipPart(clickable.part.name);
				var newPart = shipPart.Spawn();

				newPart.transform.localScale = Vector3.one;
				newPart.transform.position = clickable.obj.transform.position;
				newPart.transform.rotation = Quaternion.identity;

				var editorPart = newPart.AddComponent<EditorPart>();
				editorPart.Activate();
				editorPart.ship = ship;
				editorPart.part = shipPart;

				Debug.Log (String.Format ("Created editor part: {0}, ship: {1}, part: {2}", editorPart, ship, shipPart));
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			SaveShip();
		}
	}
}
