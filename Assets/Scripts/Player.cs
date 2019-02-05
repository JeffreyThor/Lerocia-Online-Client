using System.Collections.Generic;
using UnityEngine;

public class Player {
	public string playerName;
	public GameObject avatar;
	public int connectionId;

	public bool isLerpingPosition;
	public bool isLerpingRotation;
	public Vector3 realPosition;
	public Quaternion realRotation;
	public Vector3 lastRealPosition;
	public Quaternion lastRealRotation;
	public float timeStartedLerping;
	public float timeToLerp;

	public int maxHealth;
	public int currentHealth;
	public int maxStamina;
	public int currentStamina;
	public int gold;
	public int weight;
	public int armor;
	public int damage;

	public List<int> inventory;
}