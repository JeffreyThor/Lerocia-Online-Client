using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwing : MonoBehaviour {

	private GameObject rightArm;
	private GameObject leftArm;
	private bool attacking;
	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		attacking = false;
		rightArm = transform.Find("RightArmPlayer").gameObject;
		leftArm = transform.Find("LeftArmPlayer").gameObject;
		startPosition = rightArm.transform.localPosition;
		Debug.Log("x: " + startPosition.x + " y: " + startPosition.y + " z: " + startPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (!attacking) {
			if (Input.GetButton("Fire1")) {
				Debug.Log("Attack!");
				attacking = true;
			}
		}

		if (attacking) {
			rightArm.transform.localPosition = new Vector3(rightArm.transform.localPosition.x, rightArm.transform.localPosition.y, rightArm.transform.localPosition.z + 0.05f);
			if (rightArm.transform.localPosition.z >= startPosition.z + 0.5f) {
				rightArm.transform.localPosition = startPosition;
				Debug.Log("Done.");
				attacking = false;
			}
		}
	}
}
