using Characters;

namespace Menus.Controllers {
  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.UI;
  using Characters.Players;
  using Items;

  public class PlayerHUDController : MonoBehaviour {
    [SerializeField] private GameObject _itemStatPrefab;

    private GameObject _enemyView;
    private Slider _enemyHealthBar;
    private Text _enemyName;
    private GameObject _healthBar;
    private Slider _healthBarSlider;
    private GameObject _staminaBar;
    private Slider _staminaBarSlider;
    private GameObject _interactableView;
    private Text _helpText;
    private Text _name;
    private GameObject _statsContainer;
    private GameObject _caption;
    private Text _captionText;
    private Character _enemyCharacter;
    private float _enemyViewUpdateTime;
    private const float EnemyViewTimer = 30.0f;
    private float _healthViewUpdateTime;
    private const float HealthViewTimer = 30.0f;
    private float _staminaViewUpdateTime;
    private const float StaminaViewTimer = 30.0f;
    private float _captionViewUpdateTime;
    private const float CaptionViewTimer = 10.0f;
    public Player Player;

    // Use this for initialization
    private void Start() {
      _enemyView = transform.Find("Enemy View").gameObject;
      _enemyHealthBar = _enemyView.transform.Find("HealthBar").GetComponent<Slider>();
      _enemyName = _enemyView.transform.Find("Name").GetComponent<Text>();
      DeactivateEnemyView();
      _healthBar = transform.Find("HealthBar").gameObject;
      _healthBarSlider = _healthBar.GetComponent<Slider>();
      DeactivateHealthView();
      _staminaBar = transform.Find("StaminaBar").gameObject;
      _staminaBarSlider = _staminaBar.GetComponent<Slider>();
      DeactivateStaminaView();
      _interactableView = transform.Find("Interactable View").gameObject;
      _helpText = _interactableView.transform.Find("Help Text").GetComponent<Text>();
      _name = _interactableView.transform.Find("Name").GetComponent<Text>();
      _statsContainer = _interactableView.transform.Find("Stats").gameObject;
      DeactivateInteractableView();
      _caption = transform.Find("Caption").gameObject;
      _captionText = _caption.GetComponent<Text>();
      DeactivateCaptionView();
    }

    private void Update() {
      _healthBarSlider.value = Player.CurrentHealth;
      _staminaBarSlider.value = Player.CurrentStamina;
      if (_enemyCharacter != null) {
        _enemyHealthBar.value = _enemyCharacter.CurrentHealth;
        if (Time.time - _enemyViewUpdateTime > EnemyViewTimer) {
          DeactivateEnemyView();
        }
      }

      if (Time.time - _healthViewUpdateTime > HealthViewTimer) {
        DeactivateHealthView();
      }

      if (Time.time - _staminaViewUpdateTime > StaminaViewTimer) {
        DeactivateHealthView();
      }

      if (Time.time - _captionViewUpdateTime > CaptionViewTimer) {
        DeactivateCaptionView();
      }
    }

    public void ActivateInteractableView() {
      _interactableView.SetActive(true);
    }

    public void DeactivateInteractableView() {
      _interactableView.SetActive(false);
      foreach (Transform child in _statsContainer.transform) {
        Destroy(child.gameObject);
      }
    }

    public void SetNPCView(string npcName) {
      _helpText.text = "(E) Talk";
      _name.text = npcName;
    }

    public void SetItemView(BaseItem item) {
      _helpText.text = "(E) Take";
      _name.text = ItemList.Items[item.GetId()].GetName();
      List<GameObject> statList = new List<GameObject>();
      // Create stat object in item view for each stat on this item
      foreach (KeyValuePair<string, string> stat in item.GetStats()) {
        GameObject itemStat = Instantiate(_itemStatPrefab);
        itemStat.transform.SetParent(_statsContainer.transform, false);
        itemStat.transform.Find("Title").GetComponent<Text>().text = stat.Key;
        itemStat.transform.Find("Value").GetComponent<Text>().text = stat.Value;
        statList.Add(itemStat);
      }

      // Set x position of each stat in the item view based on the number of stats to display
      int counter = 1;
      foreach (GameObject stat in statList) {
        float width = stat.GetComponent<RectTransform>().rect.width;
        float offset = counter - (float) (statList.Count + 1) / 2;
        stat.transform.localPosition = new Vector3(width * offset, 0, 0);
        counter++;
      }
    }

    public void ActivateEnemyView(Character character) {
      _enemyView.SetActive(true);
      UpdateEnemyView(character);
    }

    public void DeactivateEnemyView() {
      _enemyView.SetActive(false);
    }

    public void UpdateEnemyView(Character character) {
      _enemyViewUpdateTime = Time.time;
      _enemyCharacter = character;
      _enemyName.text = character.Name;
    }

    public void ActivateHealthView() {
      _healthViewUpdateTime = Time.time;
      _healthBar.SetActive(true);
    }

    public void DeactivateHealthView() {
      _healthBar.SetActive(false);
    }

    public void ActivateStaminaView() {
      _staminaViewUpdateTime = Time.time;
      _staminaBar.SetActive(true);
    }

    public void DeactivateStaminaView() {
      _staminaBar.SetActive(false);
    }

    public void ActivateCaptionView(string caption) {
      _captionViewUpdateTime = Time.time;
      _captionText.text = caption;
      _caption.SetActive(true);
    }

    public void DeactivateCaptionView() {
      _caption.SetActive(false);
    }
  }
}