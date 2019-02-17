namespace Characters.Players {
  using UnityEngine;

  public class Player : Character {
    public Player(string name, GameObject avatar) : base(name, avatar) { }

    protected override void Kill() {
      // Reset players health
      CurrentHealth = MaxHealth;
      // Move them back to "spawn" point
      Avatar.transform.position = new Vector3(0, 1, 0);
    }
  }
}