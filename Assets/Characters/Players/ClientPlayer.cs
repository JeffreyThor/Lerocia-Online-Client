namespace Characters.Players {
  using UnityEngine;
  using Lerocia.Characters.Players;

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
      weaponId,
      apparelId,
      dialogueId
    ) { }
  }
}