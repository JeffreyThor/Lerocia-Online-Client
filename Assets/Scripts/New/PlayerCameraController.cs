namespace New {
  using UnityEngine;

  public class PlayerCameraController : MonoBehaviour {
    private RaycastHit _hit;
    private const float Range = 3f;
    private int _lastItemHit = -1;
    private PlayerHUDController _playerHudController;

    private void Start() {
      _playerHudController = CanvasSettings.PlayerHUD.GetComponent<PlayerHUDController>();
    }

    private void Update() {
      Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
      Debug.DrawRay(transform.position, forward, Color.red);
      if (Physics.Raycast(gameObject.transform.position, transform.forward, out _hit, Range)) {
        if (_hit.transform.CompareTag("Item")) {
          if (_hit.transform.gameObject.GetComponent<ItemController>().ItemId != _lastItemHit) {
            _lastItemHit = _hit.transform.gameObject.GetComponent<ItemController>().ItemId;
            _playerHudController.ActivateItemView(ItemList.Items[_lastItemHit]);
          }

          if (Input.GetKeyDown(KeyCode.E)) {
            NetworkSend.Reliable("PICKUP|" + _hit.transform.gameObject.GetComponent<ItemController>().WorldId);
          }
        }
      } else {
        _lastItemHit = -1;
        _playerHudController.DeactivateItemView();
      }
    }
  }
}