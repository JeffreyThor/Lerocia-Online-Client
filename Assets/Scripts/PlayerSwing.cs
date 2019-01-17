using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwing : MonoBehaviour {
  public bool attacking;
  delegate void AttackDelegate();
  Queue<AttackDelegate> attackQueue = new Queue<AttackDelegate>();
  private Animator anim;

  void Start() {
    attacking = false;
    anim = GetComponent<Animator>();
  }

  void Update() {
    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
      attacking = false;
    }
    if (!attacking && attackQueue.Count > 0) {
      attackQueue.Dequeue()();
    }
  }

  public void Attack() {
    AttackDelegate attackDelegate = RealAttack;
    attackQueue.Enqueue(attackDelegate);
  }

  public void RealAttack() {
    anim.Play("Attack");
    attacking = true;
  }
}