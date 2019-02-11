namespace New {
  using UnityEngine;

  public class PlayerCameraController : MonoBehaviour {
    private RaycastHit _hit;
    private const float Range = 3f;
    private int _lastItemHit = -1;
    private PlayerHUDController _playerHudController;

    private void Start() {
      _playerHudController = CanvasSettings.PlayerHud.GetComponent<PlayerHUDController>();
    }

    private void Update() {
      if (Physics.Raycast(gameObject.transform.position, transform.forward, out _hit, Range)) {
        if (_hit.transform.CompareTag("Item")) {
          if (_hit.transform.gameObject.GetComponent<ItemReference>().ItemId != _lastItemHit) {
            _lastItemHit = _hit.transform.gameObject.GetComponent<ItemReference>().ItemId;
            _playerHudController.ActivateItemView(ItemList.Items[_lastItemHit]);
          }

          if (Input.GetKeyDown(KeyCode.E)) {
            NetworkSend.Reliable("PICKUP|" + _hit.transform.gameObject.GetComponent<ItemReference>().WorldId);
          }
        }
      } else {
        _lastItemHit = -1;
        _playerHudController.DeactivateItemView();
      }
    }
  }
}