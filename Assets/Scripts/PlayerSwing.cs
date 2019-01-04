using System;
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
  delegate void AttackDelegate();
  Queue<AttackDelegate> attackQueue = new Queue<AttackDelegate>();

  void Start() {
    attacking = false;
    retreating = false;
    rightArm = transform.Find("RightArm").gameObject;
    leftArm = transform.Find("LeftArm").gameObject;
    attackArm = leftArm;
    startPosition = attackArm.transform.localPosition;
  }

  void Update() {
    if (attacking) {
      Debug.Log("Attacking");
      attackArm.transform.localPosition = new Vector3(attackArm.transform.localPosition.x,
        attackArm.transform.localPosition.y, attackArm.transform.localPosition.z + (4 * Time.deltaTime));
      if (attackArm.transform.localPosition.z >= startPosition.z + 0.5f) {
        retreating = true;
        attacking = false;
      }
    } else if (retreating) {
      Debug.Log("Retreating");
      attackArm.transform.localPosition = new Vector3(attackArm.transform.localPosition.x,
        attackArm.transform.localPosition.y, attackArm.transform.localPosition.z - (2 * Time.deltaTime));
      if (attackArm.transform.localPosition.z <= startPosition.z) {
        attackArm.transform.localPosition = startPosition;
        retreating = false;
      }
    } else if (attackQueue.Count > 0) {
      Debug.Log("Dequeuing");
      attackQueue.Dequeue()();
    }
  }

  public void Attack() {
    if (attacking) {
      Debug.Log("Hmmmmmmm... suspicious, but I'll still queue an attack.");
    }
    AttackDelegate attackDelegate = RealAttack;
    attackQueue.Enqueue(attackDelegate);
  }

  public void RealAttack() {
    Debug.Log("Attacking yay!");
    if (attackArm == rightArm) {
      attackArm = leftArm;
    } else {
      attackArm = rightArm;
    }
    startPosition = attackArm.transform.localPosition;
    attacking = true;
  }
}