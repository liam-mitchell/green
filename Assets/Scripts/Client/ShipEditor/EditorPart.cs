using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EditorPart : MonoBehaviour {
	private const float SnapDistance = 0.001f;

	public static EditorPart activePart;
	
	public Ship ship;
	public ShipPart part;

	public void SnapTo(EditorPart other, Vector3 point) {
		var otherCollider = other.gameObject.GetComponentInChildren<Collider>();

		var projection = GetProjection(otherCollider, point);

		Debug.Log (String.Format ("Snapping part to mouse: point {0}, projection {1}", point, projection));
		gameObject.transform.position = point + projection;
	}
	
	void Update() {
		if (activePart == this) {
			MovePart();
		}
		else if (activePart == null && Cursor.Clicking (gameObject)) {
			Activate();
		}
	}

	public void Activate() {
		Debug.Log (String.Format ("Activating part {0}", this));
		activePart = this;
		MovePartToMouse();

		if (ship != null) {
			ship.RemovePart(part);
		}
	}

	public void Deactivate() {
		if (activePart == this) {
			activePart = null;
		}

		if (ship != null) {
			ship.AddPart (part);
		}
	}

	private void ClearLayer(GameObject obj, int layer) {
		foreach (var transform in obj.GetComponentsInChildren<Transform>()) {
			transform.gameObject.layer &= ~layer;
		}
	}

	private void SetLayer(GameObject obj, int layer) {
		foreach (var transform in obj.GetComponentsInChildren<Transform>()) {
			transform.gameObject.layer |= layer;
		}
	}

	private Vector3 GetProjection(Collider collider, Vector3 point) {
		var left    = new Vector3(collider.bounds.extents.x, 0, 0);
		var up      = new Vector3(0, collider.bounds.extents.y, 0);
		var forward = new Vector3(0, 0, collider.bounds.extents.z);

		var leftSide = collider.bounds.center + left;
		var rightSide = collider.bounds.center - left;

		var frontSide = collider.bounds.center + forward;
		var backSide = collider.bounds.center - forward;

		var topSide = collider.bounds.center + up;
		var bottomSide = collider.bounds.center - up;

		var sides = new Vector3[6] {rightSide, frontSide, topSide, leftSide, backSide, bottomSide};

		SetLayer(gameObject.transform.root.gameObject, (int)Layers.IGNORE_RAYCAST);

		foreach (var side in sides) {
			var position = Camera.main.transform.position;
			var direction = side - position;

			if (Physics.Raycast (position, direction, (direction.magnitude - SnapDistance))) {
				continue;
			}

			if (NumberUtil.FloatsEqual(point.x, side.x, SnapDistance)
			    || NumberUtil.FloatsEqual(point.y, side.y, SnapDistance)
			    || NumberUtil.FloatsEqual(point.z, side.z, SnapDistance))
			{
				ClearLayer(gameObject.transform.root.gameObject, (int)Layers.IGNORE_RAYCAST);
				return side - collider.bounds.center;
			}
		}
		
		ClearLayer(gameObject.transform.root.gameObject, (int)Layers.IGNORE_RAYCAST);
		return Vector3.zero;
	}

	private void MovePart() {
		if (Cursor.Clicking(gameObject)) {
			MovePartToMouse();
		}
		else if (Cursor.ClickUp (gameObject)) {
			Debug.Log (String.Format ("Adding part {0} to ship {1}", part, ship));
			Deactivate();
		}
	}
	
	private void MovePartToMouse() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit [] hits = Physics.RaycastAll (ray);
		
		Array.Sort (hits, CompareHitDistance);
		
		foreach (var hit in hits) {
			if (hit.collider.transform.root.gameObject == transform.root.gameObject) {
				continue;
			}
			
			var other = hit.collider.transform.root.gameObject.GetComponentInChildren<EditorPart>();
			if (other != null) {
				SnapTo(other, hit.point);
				return;
			}
		}

		var distance = Camera.main.transform.position.magnitude;
		var direction = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
		Debug.Log (String.Format ("Moving part to mouse: distance {0}, direction {1}", distance, direction));
		transform.position = Camera.main.transform.position + direction * distance;
	}
	
	private static float CameraDistance(RaycastHit hit) {
		return (Camera.main.transform.position - hit.collider.transform.position).magnitude;
	}
	
	private static int CompareHitDistance(RaycastHit x, RaycastHit y) {
		if (CameraDistance(x) < CameraDistance(y)) {
			return 1;
		}
		else {
			return -1;
		}
	}
}
