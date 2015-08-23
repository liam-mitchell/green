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
			image.rectTransform.anchoredPosition = new Vector2(screen.x - image.sprite.rect.width / 2, screen.y - image.sprite.rect.height / 2);
		}
	}

	public GameObject canvasPrefab;

	public Image target;
	public Image targetLocking;
	public Image targetLocked;
	public Image reticle;

	private GameObject canvas;
	private Camera cam;
	private Image activeReticle;

	private GameObject lockTarget;
	private bool locked;

	private List<VisiblePlayer> visible;

	void OnEnable() {
		Debug.Log ("Targeting UI enabled!");

		canvas = GameObject.Instantiate(canvasPrefab);
		cam = transform.root.Find("Camera").gameObject.GetComponent<Camera>();
		canvas.GetComponent<Canvas>().worldCamera = cam;
		visible = new List<VisiblePlayer>();

		CreateReticle();

		Unlock();
	}

	private void CreateReticle() {
		var pos = Camera.main.WorldToScreenPoint(transform.root.gameObject.transform.position + transform.root.gameObject.transform.right * 100);
		activeReticle = GameObject.Instantiate (reticle);
		activeReticle.rectTransform.SetParent (canvas.GetComponent<Canvas>().transform);
		//activeReticle.rectTransform.anchoredPosition = new Vector2(pos.x - reticle.sprite.rect.width / 2, pos.y - reticle.sprite.rect.height / 2);
		activeReticle.rectTransform.anchoredPosition3D = new Vector3(pos.x - reticle.sprite.rect.width / 2, pos.y - reticle.sprite.rect.height / 2, pos.z);

		// Why does this one get scaled to 0, 0, 0? Unityyyy
		activeReticle.rectTransform.localScale = new Vector3(1, 1, 1);
		Debug.Log (String.Format ("Active reticle set to {0}, {1}", pos.x, pos.y));
	}

	void Update() {
		UpdateTargets();
	}

	private VisiblePlayer FindVisible(GameObject t) {
		if (t == null) {
			return null;
		}

		return visible.Find (v => v.player.transform.root.gameObject == t.transform.root.gameObject);
	}

	private void TargetVisible(GameObject t) {
		var v = FindVisible(t);
		if (v == null) {
			visible.Add (new VisiblePlayer(t, target, canvas.GetComponent<Canvas>()));
		}
	}

	private void TargetBehind(GameObject t) {
		visible.Remove (FindVisible(t));
	}

	private void TargetOutOfRange(GameObject t) {
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

		visible.Sort (delegate(VisiblePlayer v, VisiblePlayer o) {
			return v.image.rectTransform.anchoredPosition3D.z.CompareTo(o.image.rectTransform.anchoredPosition3D.z);
		});
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

	public GameObject GetTarget() {
		if (!enabled) return null;

		var targetDistance = activeReticle.sprite.rect.width / 2;
		var target = visible.Find (v => (activeReticle.rectTransform.anchoredPosition - v.image.rectTransform.anchoredPosition).magnitude < targetDistance);
		if (target != null) {
			Debug.Log (target.player);
			return target.player;
		}

		Debug.Log ("No target!");
		Debug.Log (String.Format ("tried to find image within {0} pixels of ({1}, {2})", targetDistance, activeReticle.rectTransform.anchoredPosition.x, activeReticle.rectTransform.anchoredPosition.y));
		return null;
	}

	private void SetImage(GameObject t, Image i) {
		var v = FindVisible(t);
		if (v != null) {
			v.SetImage (i);
		}
	}
}
