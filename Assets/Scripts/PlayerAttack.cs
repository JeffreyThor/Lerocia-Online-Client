using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
  private float damage = 10f;
  private float range = 100f;
  private GameObject arms;

  private void Start() {
    
  }

  public void Attack() {
    RaycastHit hit;
    if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, range)) {
      Debug.Log(hit.transform.name);
    }
  }
}
