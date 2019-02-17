namespace Characters.NPC {
	using UnityEngine;
	using Animation;
	using Characters.Controllers;

	public class NPCFactory : MonoBehaviour {
		public GameObject NPCPrefab;

		public void Spawn(string npcName, int npcId) {
			// Create player object
			GameObject npcObject = Instantiate(NPCPrefab);
			npcObject.name = npcName;
			// Add non-MyPlayer specific components
			npcObject.AddComponent<CharacterLerpController>();
			// Add universal player components
			npcObject.AddComponent<NPCReference>();
			npcObject.GetComponent<NPCReference>().NPCId = npcId;
			npcObject.AddComponent<CharacterAnimator>();
			// Create new player
			NPC npc = new NPC(npcName, npcObject);
			// Add player to players dictionary
			ConnectedNPCs.NPCs.Add(npcId, npc);
			// Set player references
			ConnectedNPCs.NPCs[npcId].Avatar.GetComponent<CharacterLerpController>().Character = npc;
		}
	}
}