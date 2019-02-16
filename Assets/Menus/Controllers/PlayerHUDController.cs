namespace Menus.Controllers {
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using Players;
	using Items;

	public class PlayerHUDController : MonoBehaviour {
		[SerializeField]
		private GameObject _itemStatPrefab;

		private GameObject _enemyView;
		private Slider _enemyHealthBar;
		private Text _enemyName;
		private GameObject _healthBar;
		private Slider _healthBarSlider;
		private GameObject _interactableView;
		private Text _helpText;
		private Text _name;
		private GameObject _itemStatsContainer;
		private Player _enemyPlayer;
		private float _enemyViewUpdateTime;
		private const float EnemyViewTimer = 30.0f;
		private float _healthViewUpdateTime;
		private const float HealthViewTimer = 30.0f;
		public Player Player;

		// Use this for initialization
		private void Start () {
			_enemyView = transform.Find("Enemy View").gameObject;
			_enemyHealthBar = _enemyView.transform.Find("HealthBar").GetComponent<Slider>();
			_enemyName = _enemyView.transform.Find("Name").GetComponent<Text>();
			DeactivateEnemyView();
			_healthBar = transform.Find("HealthBar").gameObject;
			_healthBarSlider = _healthBar.GetComponent<Slider>();
			DeactivateHealthView();
			_interactableView = transform.Find("Interactable View").gameObject;
			_helpText = _interactableView.transform.Find("Help Text").GetComponent<Text>();
			_name = _interactableView.transform.Find("Name").GetComponent<Text>();
			_itemStatsContainer = _interactableView.transform.Find("Stats").gameObject;
			DeactivateItemView();
		}

		private void Update() {
			_healthBarSlider.value = Player.CurrentHealth;
			if (_enemyPlayer != null) {
				_enemyHealthBar.value = _enemyPlayer.CurrentHealth;
				if (Time.time - _enemyViewUpdateTime > EnemyViewTimer) {
					DeactivateEnemyView();
				}
			}

			if (Time.time - _healthViewUpdateTime > HealthViewTimer) {
				DeactivateHealthView();
			}
		}

		public void ActivateNPCView(string npcName) {
			_interactableView.SetActive(true);
			CreateNPCView(npcName);
		}

		public void DeactivateNPCView()
		{
			_interactableView.SetActive(false);
		}

		private void CreateNPCView(string npcName)
		{
			_helpText.text = "(E) Talk";
			_name.text = npcName;
		}

		public void ActivateItemView(BaseItem item) {
			_interactableView.SetActive(true);
			UpdateItemView(item);
		}
		
		public void DeactivateItemView() {
			_interactableView.SetActive(false);
		}

		private void UpdateItemView(BaseItem item) {
			DestroyItemView();
			CreateItemView(item);
		}

		private void CreateItemView(BaseItem item)
		{
			_helpText.text = "(E) Take";
			_name.text = ItemList.Items[item.GetId()].GetName();
			List<GameObject> statList = new List<GameObject>();
			// Create stat object in item view for each stat on this item
			foreach (KeyValuePair<string, string> stat in item.GetStats()) {
				GameObject itemStat = Instantiate(_itemStatPrefab);
				itemStat.transform.SetParent(_itemStatsContainer.transform, false);
				itemStat.transform.Find("Title").GetComponent<Text>().text = stat.Key;
				itemStat.transform.Find("Value").GetComponent<Text>().text = stat.Value;
				statList.Add(itemStat);
			}

			// Set x position of each stat in the item view based on the number of stats to display
			int counter = 1;
			foreach (GameObject stat in statList) {
				float width = stat.GetComponent<RectTransform>().rect.width;
				float offset = counter - (float)(statList.Count + 1) / 2;
				stat.transform.localPosition = new Vector3(width * offset, 0, 0);
				counter++;
			}
		}
	
		private void DestroyItemView() {
			Transform panel = _interactableView.transform.Find("Item Stats");
			foreach (Transform child in panel) {
				Destroy(child.gameObject);
			}
		}
		
		public void ActivateEnemyView(Player player) {
			_enemyView.SetActive(true);
			UpdateEnemyView(player);
		}

		public void DeactivateEnemyView() {
			_enemyView.SetActive(false);
		}

		public void UpdateEnemyView(Player player) {
			_enemyViewUpdateTime = Time.time;
			_enemyPlayer = player;
			_enemyName.text = player.Name;
		}

		public void ActivateHealthView() {
			_healthViewUpdateTime = Time.time;
			_healthBar.SetActive(true);
		}

		public void DeactivateHealthView() {
			_healthBar.SetActive(false);
		}
	}

}