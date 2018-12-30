using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwing : MonoBehaviour {
  private GameObject rightArm;
  private GameObject leftArm;
  private GameObject attackArm;
  private Vector3 startPosition;
  public bool attacking;
  public bool retreating;

  void Start() {
    attacking = false;
    retreating = false;
    rightArm = transform.Find("RightArmPlayer").gameObject;
    leftArm = transform.Find("LeftArmPlayer").gameObject;
    attackArm = leftArm;
    startPosition = attackArm.transform.localPosition;
  }

  void Update() {
    if (attacking) {
      attackArm.transform.localPosition = new Vector3(attackArm.transform.localPosition.x,
        attackArm.transform.localPosition.y, attackArm.transform.localPosition.z + (4 * Time.deltaTime));
      if (attackArm.transform.localPosition.z >= startPosition.z + 0.5f) {
        attacking = false;
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

  public void Attack() {
    attacking = true;
    if (attackArm == rightArm) {
      attackArm = leftArm;
    } else {
      attackArm = rightArm;
    }

    startPosition = attackArm.transform.localPosition;
  }
}