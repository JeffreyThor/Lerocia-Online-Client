using UnityEngine;
using Menus;
using Networking;

public class Launcher : MonoBehaviour {
  private void Start() {
    CanvasSettings.InitializeCanvases();
    NetworkSettings.InitializeNetworkTransport();
  }
}