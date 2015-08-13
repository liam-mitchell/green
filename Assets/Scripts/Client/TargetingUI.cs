using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetingUI : MonoBehaviour {
	public GameObject canvasPrefab;
	
	public Sprite target;
	public Sprite targetLocking;
	public Sprite targetLocked;

	private GameObject canvas;
	private Image image;

	void OnEnable() {
		Debug.Log ("Targeting UI enabled!");

		canvas = GameObject.Instantiate(canvasPrefab);
		image = canvas.transform.Find ("Image").gameObject.GetComponent<Image>();

		Unlock();
	}

	public void Lock() {
		if (!enabled) return;
		image.sprite = targetLocked;
	}

	public void Locking() {
		if (!enabled) return;
		image.sprite = targetLocking;
	}

	public void Unlock() {
		if (!enabled) return;
		image.sprite = target;
	}
}
