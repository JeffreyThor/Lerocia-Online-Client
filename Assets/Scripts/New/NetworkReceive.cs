namespace New {
  using System.Text;
  using UnityEngine;
  using UnityEngine.Networking;

  public class NetworkReceive : MonoBehaviour {
    private GameObject _factory;
    private PlayerFactory _playerFactory;
    private ItemFactory _itemFactory;

    private void Awake() {
      _factory = GameObject.Find("Factory");
      _playerFactory = _factory.GetComponent<PlayerFactory>();
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
          string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
          string[] splitData = msg.Split('|');
          switch (splitData[0]) {
            case "ASKNAME":
              OnAskName(splitData);
              break;
            case "ITEMS":
              _itemFactory.Spawn(splitData);
              break;
            case "INVENTORY":
              OnInventory(splitData);
              break;
            case "CNN":
              _playerFactory.Spawn(splitData[1], int.Parse(splitData[2]));
              break;
            case "DC":
              OnDisconnect(int.Parse(splitData[1]));
              break;
            case "ASKPOSITION":
              //TODO
              break;
            case "CHARGE":
              //TODO
              break;
            case "ATK":
              //TODO
              break;
            case "HIT":
              //TODO
              break;
            case "USE":
              OnUse(int.Parse(splitData[1]), int.Parse(splitData[2]));
              break;
            case "DROP":
              OnDrop(int.Parse(splitData[2]), int.Parse(splitData[3]), float.Parse(splitData[4]),
                float.Parse(splitData[5]), float.Parse(splitData[6]));
              break;
            case "PICKUP":
              OnPickup(int.Parse(splitData[1]), int.Parse(splitData[2]));
              break;
            default:
              Debug.Log("Invalid message : " + msg);
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
        _playerFactory.Spawn(d[0], int.Parse(d[1]));
      }
    }
    
    private void OnInventory(string[] data) {
      for (int i = 1; i < data.Length; i++) {
        ConnectedClients.MyPlayer.Inventory.Add(int.Parse(data[i]));
      }
    }

    private void OnDisconnect(int connectionId) {
      Destroy(ConnectedClients.Players[connectionId].Avatar);
      ConnectedClients.Players.Remove(connectionId);
    }
    
    private void OnUse(int connectionId, int itemId) {
      if (connectionId != ConnectedClients.MyUser.connection_id) {
        ItemList.Items[itemId].Use(ConnectedClients.Players[connectionId]);
      }
    }
    
    private void OnDrop(int worldId, int itemId, float x, float y, float z) {
      _itemFactory.Spawn(worldId, itemId, x, y, z);
      //TODO Refresh menu if open
    }
    
    private void OnPickup(int connectionId, int worldId) {
      ConnectedClients.Players[connectionId].Inventory.Add(ItemList.WorldItems[worldId].GetComponent<ItemController>().ItemId);
      Destroy(ItemList.WorldItems[worldId]);
      ItemList.WorldItems.Remove(worldId);
    }
  }
}