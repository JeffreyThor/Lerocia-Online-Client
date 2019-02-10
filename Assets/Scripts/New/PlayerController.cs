namespace New {
  using UnityEngine;

  public class PlayerController : MonoBehaviour {
    private void Update() {
      if (Input.GetKeyDown(KeyCode.F)) {
        CanvasSettings.ToggleInventoryMenu();
      }

      if (Input.GetKeyDown(KeyCode.Escape)) {
        CanvasSettings.TogglePauseMenu();
      }
    }
  }
}