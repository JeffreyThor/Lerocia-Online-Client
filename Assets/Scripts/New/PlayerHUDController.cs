namespace New {
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class PlayerHUDController : MonoBehaviour {
		[SerializeField]
		private GameObject _itemStatPrefab;
		private Slider _healthBarSlider;
		private GameObject _itemView;
		private Text _itemName;
		private GameObject _itemStatsContainer;
		public Player Player;

		// Use this for initialization
		void Start () {
			_healthBarSlider = transform.Find("HealthBar").GetComponent<Slider>();
			_itemView = transform.Find("Item View").gameObject;
			_itemName = _itemView.transform.Find("Item Name").GetComponent<Text>();
			_itemStatsContainer = _itemView.transform.Find("Item Stats").gameObject;
			DeactivateItemView();
		}

		void Update() {
			_healthBarSlider.value = Player.CurrentHealth;
		}

		public void ActivateItemView(Item item) {
			_itemView.SetActive(true);
			UpdateItemView(item);
		}
		
		public void DeactivateItemView() {
			_itemView.SetActive(false);
		}

		private void UpdateItemView(Item item) {
			DestroyItemView();
			CreateItemView(item);
		}

		private void CreateItemView(Item item) {
			_itemName.text = ItemList.Items[item.GetId()].GetName();
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
			Transform panel = transform.Find("Item View").transform.Find("Item Stats");
			foreach (Transform child in panel) {
				Destroy(child.gameObject);
			}
		}
	}

}