using Menus;

namespace Characters.NPCs {
  using UnityEngine;
  using System.Collections.Generic;

  public class NPC : Character {
    private Dictionary<string, Dialogue> _dialogues;

    public NPC(string name, GameObject avatar, int maxHealth, int maxStamina, int baseDamage, int baseArmor,
      Dictionary<string, Dialogue> dialogues) : base(name, avatar, maxHealth, maxStamina, baseDamage, baseArmor) {
      _dialogues = dialogues;
    }

    public string[] Interact(string prompt) {
      // "Say" response
      CanvasSettings.PlayerHudController.ActivateCaptionView(_dialogues[prompt].response);
      // Return options
      return _dialogues[prompt].options;
    }

    protected override void Kill() {
      //TODO Handle NPC death
    }
  }
}