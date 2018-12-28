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
		startPosition = rightArm.transform.position;
		attacking = false;
		//TODO: These aren't finding the arms
		rightArm = GameObject.Find("RightArmPlayer");
		leftArm = GameObject.Find("LeftArmPlayer");
	}
	
	// Update is called once per frame
	void Update () {
		if (!attacking) {
			if (Input.GetButton("Fire1")) {
				attacking = true;
			}
		}

		if (attacking) {
			rightArm.transform.position = new Vector3(rightArm.transform.position.x, rightArm.transform.position.y, rightArm.transform.position.z + 0.05f);
			if (rightArm.transform.position.z >= 1.0f) {
				rightArm.transform.position = startPosition;
				attacking = false;
			}
		}
	}
}
