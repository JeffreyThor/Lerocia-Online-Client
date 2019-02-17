namespace Characters.NPCs {
	using System.Collections.Generic;
	using UnityEngine;
	using Animation;
	using Characters.Controllers;
	using Controllers;

	public class NPCFactory : MonoBehaviour {
		public GameObject NPCPrefab;
		
		public void Spawn(string[] data) {
			// Spawn all NPCs
			for (int i = 1; i < data.Length; i++) {
				string[] d = data[i].Split('%');
				Spawn(int.Parse(d[0]), d[1], float.Parse(d[2]), float.Parse(d[3]), float.Parse(d[4]));
			}
		}

		public void Spawn(int npcId, string npcName, float x, float y, float z) {
			GameObject npcObject = Instantiate(NPCPrefab);
			npcObject.name = npcName;
			npcObject.transform.position = new Vector3(x, y, z);
			npcObject.AddComponent<NPCController>();
			npcObject.AddComponent<CharacterLerpController>();
			npcObject.AddComponent<NPCReference>();
			npcObject.GetComponent<NPCReference>().NPCId = npcId;
			npcObject.AddComponent<CharacterAnimator>();
			Dictionary<string, Dialogue> dialogues = new Dictionary<string, Dialogue> {
				{
					"Talk",
					new Dialogue("Hi stranger.", null)
				}
			};
			NPC npc = new NPC(npcName, npcObject, 100, 100, 5, 0, dialogues);
			ConnectedCharacters.NPCs.Add(npcId, npc);
			ConnectedCharacters.NPCs[npcId].Avatar.GetComponent<CharacterLerpController>().Character = npc;
		}
	}
}