namespace Characters.Players {
  using UnityEngine;
  using Lerocia.Characters.Players;
  using Lerocia.Characters;
  using Menus;

  public class ClientPlayer : Player {
    public ClientPlayer(
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
      int weaponId, 
      int apparelId,
      int dialogueId,
      Vector3 origin
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
      weaponId,
      apparelId,
      dialogueId,
      origin
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
  }
}