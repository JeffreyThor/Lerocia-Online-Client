namespace Characters.NPCs {
  using UnityEngine;
  using System.Collections.Generic;
  using Menus;
  using Networking;

  public class NPC : Character {
    private Dictionary<string, Dialogue> _dialogues;

    public NPC(string name, GameObject avatar, string type, int maxHealth, int currentHealth, int maxStamina, int currentStamina, int gold, int baseDamage, int baseArmor, int weapon, int apparel,
      Dictionary<string, Dialogue> dialogues) : base(name, avatar, type, maxHealth, currentHealth, maxStamina, currentStamina, gold, baseDamage, baseArmor, weapon, apparel) {
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
      IsDead = true;
      _dialogues = DialogueList.Dialogues[0];
      NetworkSend.Reliable("NPCITEMS|" + Avatar.GetComponent<NPCReference>().NPCId);
    }

    public void StartMerchant() {
      Debug.Log("Starting Merchant");
    }

    public void LootBody() {
      Debug.Log("Looting Body");
    }
  }
}