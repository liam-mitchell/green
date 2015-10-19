﻿using UnityEngine;
using System.Collections;

public class Cursor {
	public static bool OnScreen() {
		Rect screen = new Rect(0, 0, Screen.width, Screen.height);
		return screen.Contains(Input.mousePosition);
	}

	public static bool HoveringOn(GameObject obj) {
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var collider = obj.GetComponent<Collider>();
		if (collider == null) {
			return false;
		}

		var hit = new RaycastHit();
		return collider.Raycast(ray, out hit, float.MaxValue);
	}

	public static bool Clicking(GameObject obj) {
		return Input.GetMouseButton(0) && HoveringOn(obj);
	}

	public static bool ClickDown(GameObject obj) {
		return Input.GetMouseButtonDown(0) && HoveringOn(obj);
	}

	public static bool ClickUp(GameObject obj) {
		return Input.GetMouseButtonUp(0) && HoveringOn(obj);
	}
}
