using System.Collections.Generic;
using Characters.NPC.Controllers;

namespace Characters.NPC {
  using UnityEngine;

  public class NPC : Character {
    private Dictionary<string, string> dialogues;

    public NPC(string name, GameObject avatar, int maxHealth, int maxStamina, int baseDamage, int baseArmor) : base(
      name, avatar, maxHealth, maxStamina, baseDamage, baseArmor) {
      dialogues = new Dictionary<string, string>();
    }

    protected override void Kill() {
      Avatar.GetComponent<NPCController>().Destroy();
    }
  }
}