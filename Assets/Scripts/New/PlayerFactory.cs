namespace New {
	using UnityEngine;

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
			// Create my player
			GameObject playerObject = Instantiate(MyPlayerPrefab);
			playerObject.name = playerName;
			playerObject.AddComponent<PlayerController>();
			playerObject.transform.Find("FirstPersonCharacter").gameObject.AddComponent<PlayerCameraController>();
			ConnectedClients.MyPlayer = new Player(playerName, playerObject);
			// Add my player to players dictionary
			ConnectedClients.Players.Add(connectionId, ConnectedClients.MyPlayer);
			//Disable login menu
			CanvasSettings.LoginMenu.SetActive(false);
			// Active player HUD
			CanvasSettings.PlayerHUD.GetComponent<PlayerHUDController>().Player = ConnectedClients.MyPlayer;
			CanvasSettings.PlayerHUD.SetActive(true);
			NetworkSettings.IsStarted = true;
		}

		private void SpawnPlayer(string playerName, int connectionId) {
			GameObject playerObject = Instantiate(PlayerPrefab);
			playerObject.name = playerName;
			Player player = new Player(playerName, playerObject);
			ConnectedClients.Players.Add(connectionId, player);
			ConnectedClients.Players[connectionId].Avatar.GetComponent<PlayerLerpController>().Player = player;

		}
	}
}