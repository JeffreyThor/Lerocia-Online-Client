using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwing : MonoBehaviour {
  private GameObject rightArm;
  private GameObject leftArm;
  private GameObject attackArm;
  private Vector3 startPosition;
  private bool attacking;
  private bool retreating;

  // Use this for initialization
  void Start() {
    attacking = false;
    rightArm = transform.Find("RightArmPlayer").gameObject;
    leftArm = transform.Find("LeftArmPlayer").gameObject;
    attackArm = leftArm;
    startPosition = attackArm.transform.localPosition;
  }

  // Update is called once per frame
  void Update() {
    if (!attacking && !retreating) {
      if (Input.GetButton("Fire1")) {
        Debug.Log("Attacking");
        attacking = true;
        if (attackArm == rightArm) {
          attackArm = leftArm;
        } else {
          attackArm = rightArm;
        }
        startPosition = attackArm.transform.localPosition;
      }
    }

    if (attacking) {
      attackArm.transform.localPosition = new Vector3(attackArm.transform.localPosition.x,
        attackArm.transform.localPosition.y, attackArm.transform.localPosition.z + (4 * Time.deltaTime));
      if (attackArm.transform.localPosition.z >= startPosition.z + 0.5f) {
        attacking = false;
        Debug.Log("Retreating");
        retreating = true;
      }
    } else if (retreating) {
      attackArm.transform.localPosition = new Vector3(attackArm.transform.localPosition.x,
        attackArm.transform.localPosition.y, attackArm.transform.localPosition.z - (2 * Time.deltaTime));
      if (attackArm.transform.localPosition.z <= startPosition.z) {
        attackArm.transform.localPosition = startPosition;
        retreating = false;
      }
    }
  }
}