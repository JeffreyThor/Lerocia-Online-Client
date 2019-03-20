using Items;

namespace Characters.NPCs {
  using UnityEngine;
  using Lerocia.Characters.NPCs;
  using Menus;
  using Lerocia.Characters;
  using Bodies;

  public class ClientNPC : NPC {
    public ClientNPC(
      int characterId, 
      string characterName, 
      string characterPersonality,
      GameObject avatar, 
      int maxHealth, 
      int currentHealth, 
      int maxStamina,
      int currentStamina, 
      int gold, 
      int baseWeight,
      int baseDamage,
      int baseArmor,
      int weapon,
      int apparel,
      int dialogueId
    ) : base(
      characterId, 
      characterName, 
      characterPersonality,
      avatar, 
      maxHealth, 
      currentHealth, 
      maxStamina,
      currentStamina, 
      gold,
      baseWeight,
      baseDamage,
      baseArmor,
      weapon,
      apparel,
      dialogueId
    ) { }

    public override string[] Interact(string prompt) {
      Dialogue dialogue;
      if (Dialogues.TryGetValue(prompt, out dialogue)) {
        // "Say" response
        CanvasSettings.PlayerHudController.ActivateCaptionView(dialogue.response);
        // Return options
        return dialogue.options;
      }

      return null;
    }

    public override void StartMerchant() {
      //TODO Handle ClientNPC Start Merchant
      CanvasSettings.ToggleInventoryMenu(this, "MERCHANT");
    }
  }
}