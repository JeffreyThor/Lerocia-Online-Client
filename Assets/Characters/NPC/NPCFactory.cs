namespace Characters.NPC {
	using UnityEngine;
	using Animation;
	using Characters.Controllers;

	public class NPCFactory : MonoBehaviour {
		public GameObject NPCPrefab;
		
		public void Spawn(string[] data) {
			// Spawn all NPCs
			for (int i = 1; i < data.Length; i++) {
				string[] d = data[i].Split('%');
				Spawn(d[0], int.Parse(d[1]), float.Parse(d[2]), float.Parse(d[3]), float.Parse(d[4]));
			}
		}

		public void Spawn(string npcName, int npcId, float x, float y, float z) {
			GameObject npcObject = Instantiate(NPCPrefab);
			npcObject.name = npcName;
			npcObject.transform.position = new Vector3(x, y, z);
			npcObject.AddComponent<CharacterLerpController>();
			npcObject.AddComponent<NPCReference>();
			npcObject.GetComponent<NPCReference>().NPCId = npcId;
			npcObject.AddComponent<CharacterAnimator>();
			NPC npc = new NPC(npcName, npcObject, 100, 100, 5, 0);
			ConnectedNPCs.NPCs.Add(npcId, npc);
			ConnectedNPCs.NPCs[npcId].Avatar.GetComponent<CharacterLerpController>().Character = npc;
		}
	}
}