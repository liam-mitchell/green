using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

public class EditorPartsMenu : MonoBehaviour {
	public List<GameObject> parts;
	public Image menu;
	public int partsShown;

	public int startOffset;
	public int partSpacing;

//	public ShipPartSpawner spawner;
	public EditorShip editorShip;

//	private List<ClickablePart> parts;
	private List<GameObject> clickable;
	private PlayerDataClient dataClient;
	private PlayerClient playerClient;

//	private class ClickablePart {
//		public GameObject obj;
//		public string name;
//		public ClickablePart(string n, GameObject o) {
//			name = n;
//			obj = o;
//			Debug.Log (String.Format ("Created clickable part with name {0}", name));
//		}
//	}

	// Use this for initialization
	void Start () {
		clickable = new List<GameObject>();

		var pc = GameObject.Find ("PlayerClient");
		if (pc != null) {
			playerClient = pc.GetComponent<PlayerClient>();
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
			dataClient.ChangePlayerShip(playerClient.player, editorShip.ship);
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

		for (int i = 0; i < partsShown && i < parts.Count; ++i, pos += step) {
			var obj = (GameObject)GameObject.Instantiate(parts[i]);
			Assert.IsNotNull(obj.GetComponent<ShipPart>());

			obj.transform.SetParent(menu.rectTransform.parent);
			obj.layer |= (int)Layers.IGNORE_RAYCAST;
			
			var size = obj.GetComponentInChildren<Renderer>().bounds.size.magnitude + partSpacing;
			var scale = maxSize / size;

			obj.transform.localScale *= scale;
			obj.transform.position = pos;
			obj.transform.rotation = Quaternion.identity;

			clickable.Add (obj);
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var obj in clickable) {
			if (Cursor.ClickDown(obj)) {
//				Debug.Log ("Clicking object");
				var newObj = GameObject.Instantiate(obj);
				var part = newObj.GetComponent<ShipPart>();

				newObj.transform.localScale = Vector3.one;
				newObj.transform.rotation = Quaternion.identity;

				var editorPart = newObj.AddComponent<EditorPart>();
				editorPart.Activate();
				editorPart.ship = editorShip.ship;
				editorPart.part = part;
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			SaveShip();
		}
	}
}
