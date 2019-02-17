using System.Collections.Generic;
using Characters.NPC.Controllers;

namespace Characters.NPC {
  using UnityEngine;

  public class NPC : Character {
    private Dictionary<string, string> dialogues;

    public NPC(string name, GameObject avatar) : base (name, avatar){
      dialogues = new Dictionary<string, string>();
    }

    protected override void Kill() {
      Avatar.GetComponent<NPCController>().Destroy();
    }
  }
}