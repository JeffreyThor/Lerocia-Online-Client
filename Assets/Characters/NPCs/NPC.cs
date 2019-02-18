namespace Characters.NPCs {
  using UnityEngine;
  using System.Collections.Generic;
  using Menus;
  using Networking;

  public class NPC : Character {
    private Dictionary<string, Dialogue> _dialogues;

    public NPC(string name, GameObject avatar, int maxHealth, int maxStamina, int baseDamage, int baseArmor,
      Dictionary<string, Dialogue> dialogues) : base(name, avatar, maxHealth, maxStamina, baseDamage, baseArmor) {
      _dialogues = dialogues;
    }

    public string[] Interact(string prompt) {
      Dialogue dialogue;
      if (_dialogues.TryGetValue(prompt, out dialogue)) {
        // "Say" response
        CanvasSettings.PlayerHudController.ActivateCaptionView(dialogue.response);
        // Return options
        return dialogue.options;
      }

      return null;
    }

    protected override void Kill() {
      //TODO Handle NPC death
      NetworkSend.Reliable("NPCITEMS|" + Avatar.GetComponent<NPCReference>().NPCId);
    }
  }
}