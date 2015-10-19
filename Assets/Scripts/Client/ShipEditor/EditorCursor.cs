using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditorCursor : MonoBehaviour {
	public Image cursor;

	// Update is called once per frame
	void Update () {
		Rect screen = new Rect(0, 0, Screen.width, Screen.height);
		if (!screen.Contains(Input.mousePosition)) {
			return;
		}
	
		cursor.rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y - cursor.sprite.rect.height / 2);
	}
}
