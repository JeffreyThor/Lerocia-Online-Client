using UnityStandardAssets.Characters.FirstPerson;

namespace New {
  using UnityEngine;

  public static class CanvasSettings {
    public static GameObject LoginMenu;
    public static GameObject PlayerHud;
    public static GameObject PauseMenu;
    public static GameObject InventoryMenu;
    private static InventoryMenuController _inventoryMenuController;

    public static void InitializeCanvases() {
      LoginMenu = GameObject.Find("LoginMenu");
      LoginMenu.SetActive(true);
      PlayerHud = GameObject.Find("PlayerHUD");
      PlayerHud.SetActive(false);
      PauseMenu = GameObject.Find("PauseMenu");
      PauseMenu.SetActive(false);
      InventoryMenu = GameObject.Find("InventoryMenu");
      _inventoryMenuController = InventoryMenu.GetComponent<InventoryMenuController>();
      InventoryMenu.SetActive(false);
    }

    public static void ToggleInventoryMenu() {
      if (!PauseMenu.activeSelf) {
        if (InventoryMenu.activeSelf) {
          DeactivateMenu();
        } else {
          ConnectedClients.MyPlayer.Avatar.GetComponent<FirstPersonController>().enabled = false;
          PlayerHud.SetActive(false);
          PauseMenu.SetActive(false);
          InventoryMenu.SetActive(true);
          _inventoryMenuController.OpenMenu();
        }
      }
    }
    
    public static void TogglePauseMenu() {
      if (PauseMenu.activeSelf) {
        DeactivateMenu();
      } else {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ConnectedClients.MyPlayer.Avatar.GetComponent<FirstPersonController>().enabled = false;
        PlayerHud.SetActive(false);
        if (InventoryMenu.activeSelf) {
          _inventoryMenuController.CloseMenu();
          InventoryMenu.SetActive(false);
        }
        PauseMenu.SetActive(true);
      }
    }

    private static void DeactivateMenu() {
      ConnectedClients.MyPlayer.Avatar.GetComponent<FirstPersonController>().enabled = true;
      PauseMenu.SetActive(false);
      if (InventoryMenu.activeSelf) {
        _inventoryMenuController.CloseMenu();
        InventoryMenu.SetActive(false);
      }
      PlayerHud.SetActive(true);
    }
  }
}