namespace New {
  using UnityEngine;

  public class PlayerController : MonoBehaviour {
    private PlayerAnimator _playerAnimator;
    private float _chargeStartTime;
    private float _chargeEndTime;
    private const float Range = 10f;

    private void Start() {
      _playerAnimator = gameObject.GetComponent<PlayerAnimator>();
    }

    private void Update() {
      if (Input.GetKeyDown(KeyCode.F)) {
        CanvasSettings.ToggleInventoryMenu();
      }

      if (Input.GetKeyDown(KeyCode.Escape)) {
        CanvasSettings.TogglePauseMenu();
      }

      if (!_playerAnimator.Attacking) {
        if (Input.GetButtonDown("Fire1")) {
          NetworkSend.Reliable("CHARGE|");
          _chargeStartTime = Time.time;
          _playerAnimator.Charge();
        }
      }

      if (_playerAnimator.Charging) {
        if (Input.GetButtonUp("Fire1")) {
          NetworkSend.Reliable("ATK|");
          _chargeEndTime = Time.time;
          _playerAnimator.Attack();
          Attack(_chargeEndTime - _chargeStartTime);
        }
      }
    }

    private void Attack(float chargeTime) {
      int damageBoost = Mathf.FloorToInt(chargeTime);
      RaycastHit hit;
      if (Physics.Raycast(gameObject.transform.position, transform.forward, out hit, Range)) {
        if (hit.transform.CompareTag("Player")) {
          //TODO Send HIT with hit players id (Need some way to reference that ID from the gameobject
          NetworkSend.Reliable("HIT|" + hit.transform.gameObject.GetComponent<PlayerReference>().ConnectionId + "|" +
                               (ConnectedClients.MyPlayer.Damage + damageBoost));
        }
      }
    }
  }
}