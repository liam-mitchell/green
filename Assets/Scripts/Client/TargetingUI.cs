using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class TargetingUI : MonoBehaviour {
	private class VisiblePlayer {
		public GameObject player;
		public Image image;
		public Canvas canvas;

		public VisiblePlayer(GameObject p, Image i, Canvas c) {
			player = p;
			canvas = c;
			SetImage(i);
		}

		public void SetImage(Image i) {
			Debug.Log (String.Format ("Setting image {0}", i));
			if (image) {
				image.sprite = i.sprite;
			}
			else {
				image = GameObject.Instantiate (i);
				image.rectTransform.SetParent (canvas.transform);
				image.rectTransform.anchorMax = new Vector2(0, 0);
				image.rectTransform.anchorMin = new Vector2(0, 0);
			}

			UpdateImage();
		}

		public void UpdateImage() {
			if (!player) {
				if (image) {
					GameObject.Destroy(image);
					image = null;
				}

				return;
			}

			Vector3 screen = Camera.main.WorldToScreenPoint(player.transform.position);
			image.rectTransform.anchoredPosition = new Vector2(screen.x, screen.y);
		}
	}

	public GameObject canvasPrefab;

	public Image target;
	public Image targetLocking;
	public Image targetLocked;

	private GameObject canvas;
	private Camera cam;

	private GameObject lockTarget;
	private bool locked;

	private List<VisiblePlayer> visible;

	void OnEnable() {
		Debug.Log ("Targeting UI enabled!");

		canvas = GameObject.Instantiate(canvasPrefab);
		cam = transform.root.Find("Camera").gameObject.GetComponent<Camera>();
		canvas.GetComponent<Canvas>().worldCamera = cam;
		visible = new List<VisiblePlayer>();

		Unlock();
	}

	void Update() {
		UpdateTargets();
	}

	private VisiblePlayer FindVisible(GameObject t) {
		return visible.Find (v => v.player.transform.root.gameObject == t.transform.root.gameObject);
	}

	void TargetVisible(GameObject t) {
		var vis = FindVisible(t);
		if (vis == null) {
			visible.Add (new VisiblePlayer(t, target, canvas.GetComponent<Canvas>()));
		}
	}

	void TargetBehind(GameObject t) {
		visible.Remove (FindVisible(t));
	}

	void TargetOutOfRange(GameObject t) {
		visible.Remove (FindVisible(t));
	}

	private void UpdateTargets() {
		GameObject [] targets = GameObject.FindGameObjectsWithTag("Targetable");
		foreach (var t in targets) {
			if (t.transform.root.gameObject == transform.root.gameObject) {
				continue;
			}

			Vector3 screen = cam.WorldToScreenPoint (t.transform.position);
			if (screen.z > 0.0f) {
				TargetVisible(t);
			}
			else {
				TargetBehind(t);
			}
		}

		foreach (var v in visible) {
			v.UpdateImage();
		}
	}

	public void Lock(GameObject t) {
		if (!enabled) return;
		SetImage(t, targetLocked);
	}

	public void Locking(GameObject t) {
		if (!enabled) return;
		SetImage(t, targetLocking);
	}

	public void Unlock() {
		foreach (var v in visible) {
			v.SetImage(target);
		}
	}

	private void SetImage(GameObject t, Image i) {
		var v = FindVisible(t);
		if (v != null) {
			v.SetImage (i);
		}
	}
}
