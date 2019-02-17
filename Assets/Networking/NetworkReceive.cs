
namespace Networking {
  using System.Text;
  using UnityEngine;
  using UnityEngine.Networking;
  using Characters.Players;
  using Characters.NPC;
  using Characters.Animation;
  using Items;
  using Menus;

  public class NetworkReceive : MonoBehaviour {
    private GameObject _factory;
    private PlayerFactory _playerFactory;
    private ItemFactory _itemFactory;
    private NPCFactory _npcFactory;

    private void Awake() {
      _factory = GameObject.Find("Factory");
      _playerFactory = _factory.GetComponent<PlayerFactory>();
      _npcFactory = _factory.GetComponent<NPCFactory>();
      _itemFactory = _factory.GetComponent<ItemFactory>();
    }

    private void Update() {
      if (!NetworkSettings.IsConnected) {
        return;
      }

      int recHostId;
      int connectionId;
      int channelId;
      byte[] recBuffer = new byte[1024];
      int bufferSize = 1024;
      int dataSize;
      byte error;
      NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
        bufferSize, out dataSize, out error);
      switch (recData) {
        case NetworkEventType.DataEvent:
          string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
          string[] splitData = message.Split('|');
          switch (splitData[0]) {
            case "ASKNAME":
              OnAskName(splitData);
              break;
            case "NPCS":
              OnNPCs(splitData);
              break;
            case "ITEMS":
              OnItems(splitData);
              break;
            case "INVENTORY":
              OnInventory(splitData);
              break;
            case "CNN":
              OnConnect(splitData);
              break;
            case "DC":
              OnDisconnect(int.Parse(splitData[1]));
              break;
            case "ASKPOSITION":
              OnAskPosition(splitData);
              break;
            case "ATK":
              OnAttack(int.Parse(splitData[1]));
              break;
            case "HIT":
              OnHit(int.Parse(splitData[1]), int.Parse(splitData[2]), int.Parse(splitData[3]));
              break;
            case "USE":
              OnUse(int.Parse(splitData[1]), int.Parse(splitData[2]));
              break;
            case "DROP":
              OnDrop(int.Parse(splitData[1]), int.Parse(splitData[2]), int.Parse(splitData[3]), float.Parse(splitData[4]),
                float.Parse(splitData[5]), float.Parse(splitData[6]));
              break;
            case "PICKUP":
              OnPickup(int.Parse(splitData[1]), int.Parse(splitData[2]));
              break;
            default:
              Debug.Log("Invalid message : " + message);
              break;
          }

          break;
      }
    }

    private void OnAskName(string[] data) {
      // Set this client's ID
      ConnectedClients.MyUser.connection_id = int.Parse(data[1]);

      // Send our name to the server
      NetworkSend.Reliable("NAMEIS|" + ConnectedClients.MyUser.username + "|" + ConnectedClients.MyUser.user_id);

      // Create all the other players
      for (int i = 2; i < data.Length - 1; i++) {
        string[] d = data[i].Split('%');
        //TODO Store and retrieve position to set
        _playerFactory.Spawn(d[0], int.Parse(d[1]), 0, 0, 0);
      }
    }

    private void OnItems(string[] data) {
      _itemFactory.Spawn(data);
    }
    
    private void OnNPCs(string[] data) {
      _npcFactory.Spawn(data);
    }

    private void OnInventory(string[] data) {
      for (int i = 1; i < data.Length; i++) {
        ConnectedClients.MyPlayer.Inventory.Add(int.Parse(data[i]));
      }
    }

    private void OnConnect(string[] data) {
      //TODO Store and retrieve position to set
      _playerFactory.Spawn(data[1], int.Parse(data[2]), 0, 0, 0);
    }

    private void OnDisconnect(int connectionId) {
      Destroy(ConnectedClients.Players[connectionId].Avatar);
      ConnectedClients.Players.Remove(connectionId);
    }

    private void OnAskPosition(string[] data) {
      if (!NetworkSettings.IsStarted) {
        return;
      }

      // Update everyone else
      for (int i = 1; i < data.Length; i++) {
        string[] d = data[i].Split('%');

        // Prevent the server from updating us
        if (ConnectedClients.MyUser.connection_id != int.Parse(d[0])) {
          Vector3 position = Vector3.zero;
          position.x = float.Parse(d[1]);
          position.y = float.Parse(d[2]);
          position.z = float.Parse(d[3]);

          Quaternion rotation = Quaternion.identity;
          rotation.w = float.Parse(d[4]);
          rotation.x = float.Parse(d[5]);
          rotation.y = float.Parse(d[6]);
          rotation.z = float.Parse(d[7]);

          ConnectedClients.Players[int.Parse(d[0])].LastRealPosition =
            ConnectedClients.Players[int.Parse(d[0])].RealPosition;
          ConnectedClients.Players[int.Parse(d[0])].LastRealRotation =
            ConnectedClients.Players[int.Parse(d[0])].RealRotation;

          ConnectedClients.Players[int.Parse(d[0])].RealPosition = position;
          ConnectedClients.Players[int.Parse(d[0])].RealRotation = rotation;

          ConnectedClients.Players[int.Parse(d[0])].TimeToLerp = float.Parse(d[8]);
          if (ConnectedClients.Players[int.Parse(d[0])].RealPosition !=
              ConnectedClients.Players[int.Parse(d[0])].Avatar.transform.position) {
            ConnectedClients.Players[int.Parse(d[0])].IsLerpingPosition = true;
          }

          if (ConnectedClients.Players[int.Parse(d[0])].RealRotation !=
              ConnectedClients.Players[int.Parse(d[0])].Avatar.transform.rotation) {
            ConnectedClients.Players[int.Parse(d[0])].IsLerpingRotation = true;
          }

          ConnectedClients.Players[int.Parse(d[0])].TimeStartedLerping = Time.time;
        }
      }

      // Send our own position
      Vector3 myPosition = ConnectedClients.MyPlayer.Avatar.transform.position;
      Quaternion myRotation = ConnectedClients.MyPlayer.Avatar.transform.rotation;
      ConnectedClients.MyPlayer.TimeBetweenMovementEnd = Time.time;
      string message = "MYPOSITION|" + myPosition.x + '|' + myPosition.y + '|' + myPosition.z + '|' + myRotation.w +
                       '|' + +myRotation.x + '|' + +myRotation.y + '|' + +myRotation.z + '|' +
                       (ConnectedClients.MyPlayer.TimeBetweenMovementEnd -
                        ConnectedClients.MyPlayer.TimeBetweenMovementStart);
      NetworkSend.Unreliable(message);
      ConnectedClients.MyPlayer.TimeBetweenMovementStart = Time.time;
    }
    
    private void OnAttack(int connectionId) {
      if (connectionId != ConnectedClients.MyUser.connection_id) {
        ConnectedClients.Players[connectionId].Avatar.GetComponent<CharacterAnimator>().Attack();
      }
    }
    
    private void OnHit(int connectionId, int hitId, int damage) {
      if (hitId == ConnectedClients.MyUser.connection_id) {
        CanvasSettings.PlayerHudController.ActivateHealthView();
      }
      ConnectedClients.Players[hitId].TakeDamage(damage);
    }

    private void OnUse(int connectionId, int itemId) {
      if (connectionId != ConnectedClients.MyUser.connection_id) {
        ItemList.Items[itemId].Use(ConnectedClients.Players[connectionId]);
      }
    }

    private void OnDrop(int connectionId, int worldId, int itemId, float x, float y, float z) {
      _itemFactory.Spawn(worldId, itemId, x, y, z);
      if (connectionId == ConnectedClients.MyUser.connection_id && CanvasSettings.InventoryMenu.activeSelf) {
        CanvasSettings.InventoryMenuController.RefreshMenu();
      }
    }

    private void OnPickup(int connectionId, int worldId) {
      Debug.Log("received pickup from " + connectionId + " of item " + worldId);
      ConnectedClients.Players[connectionId].Inventory
        .Add(ItemList.WorldItems[worldId].GetComponent<ItemReference>().ItemId);
      Destroy(ItemList.WorldItems[worldId]);
      ItemList.WorldItems.Remove(worldId);
    }
  }
}