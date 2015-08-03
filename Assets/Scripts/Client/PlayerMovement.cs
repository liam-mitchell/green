using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float velocity;
	public float lateralVelocity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		Debug.Log ("PlayerMovement updating!");
		if (Input.GetKey(KeyCode.W)) {
			transform.position += velocity * transform.forward * Time.fixedDeltaTime;
		}
		else if (Input.GetKey (KeyCode.S)) {
			transform.position -= velocity * transform.forward * Time.fixedDeltaTime;
		}
		else if (Input.GetKey (KeyCode.A)) {
			transform.position += lateralVelocity * transform.right * Time.fixedDeltaTime;
		}
		else if (Input.GetKey (KeyCode.D)) {
			transform.position -= lateralVelocity * transform.right * Time.fixedDeltaTime;
		}
	}
}
