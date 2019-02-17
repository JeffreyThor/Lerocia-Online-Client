namespace Characters.Players {
	using UnityEngine;
	using Menus;
	using Controllers;
	using Menus.Controllers;
	using Networking;
	using Animation;
	using Characters.Controllers;

	public class PlayerFactory : MonoBehaviour {
		public GameObject MyPlayerPrefab;
		public GameObject PlayerPrefab;

		public void Spawn(string playerName, int connectionId) {
			if (ConnectedClients.MyUser.connection_id == connectionId) {
				SpawnMyPlayer(playerName, connectionId);
			} else {
				SpawnPlayer(playerName, connectionId);
			}
		}

		private void SpawnMyPlayer(string playerName, int connectionId) {
			// Create my player object
			GameObject playerObject = Instantiate(MyPlayerPrefab);
			playerObject.name = playerName;
			// Add MyPlayer specific components
			playerObject.AddComponent<PlayerController>();
			playerObject.transform.Find("FirstPersonCharacter").gameObject.AddComponent<PlayerCameraController>();
			// Add universal player components
			playerObject.AddComponent<PlayerReference>();
			playerObject.GetComponent<PlayerReference>().ConnectionId = connectionId;
			playerObject.AddComponent<CharacterAnimator>();
			// Create new player
			ConnectedClients.MyPlayer = new Player(playerName, playerObject);
			// Add my player to players dictionary
			ConnectedClients.Players.Add(connectionId, ConnectedClients.MyPlayer);
			//Disable login menu
			CanvasSettings.LoginMenu.SetActive(false);
			// Activate player HUD
			CanvasSettings.PlayerHud.GetComponent<PlayerHUDController>().Player = ConnectedClients.MyPlayer;
			CanvasSettings.PlayerHud.SetActive(true);
			// We are now safe to start
			NetworkSettings.IsStarted = true;
		}

		private void SpawnPlayer(string playerName, int connectionId) {
			// Create player object
			GameObject playerObject = Instantiate(PlayerPrefab);
			playerObject.name = playerName;
			// Add non-MyPlayer specific components
			playerObject.AddComponent<CharacterLerpController>();
			// Add universal player components
			playerObject.AddComponent<PlayerReference>();
			playerObject.GetComponent<PlayerReference>().ConnectionId = connectionId;
			playerObject.AddComponent<CharacterAnimator>();
			// Create new player
			Player player = new Player(playerName, playerObject);
			// Add player to players dictionary
			ConnectedClients.Players.Add(connectionId, player);
			// Set player references
			ConnectedClients.Players[connectionId].Avatar.GetComponent<CharacterLerpController>().Character = player;
		}
	}
}