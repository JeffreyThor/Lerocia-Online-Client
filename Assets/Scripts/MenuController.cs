using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
  public GameObject categoryTextPrefab;
  public GameObject itemTextPrefab;
  public GameObject itemImagePrefab;
  public GameObject itemNamePrefab;
  public GameObject itemStatPrefab;
  public GameObject itemDescriptionPrefab;
  private Client client;
  private Dictionary<GameObject, List<GameObject>> itemDictionary;
  private GameObject currentCategory;
  private float scrollDelay;
  private float lastScrollTime;
  private int currentCategoryIndex;
  private List<int> currentItemIndexes;
  private bool isItemView;

  // Use this for initialization
  void Start() {
    client = GameObject.Find("Client").GetComponent<Client>();
    scrollDelay = 0.25f;
    lastScrollTime = 0;
    currentCategoryIndex = 0;
    isItemView = false;
  }

  // Update is called once per frame
  void Update() {
    if (client.inMenu && Time.time - lastScrollTime > scrollDelay) {
      if (Input.GetAxis("Vertical") > 0) {
        MoveUp();
      } else if (Input.GetAxis("Vertical") < 0) {
        MoveDown();
      }

      if (Input.GetAxis("Horizontal") > 0) {
        MoveRight();
      } else if (Input.GetAxis("Horizontal") < 0) {
        MoveLeft();
      }
    }
  }

  public void OpenMenu() {
    // Initialize dictionary of all items with key->value being category->items
    itemDictionary = new Dictionary<GameObject, List<GameObject>>();
    
    currentItemIndexes = new List<int>();

    // Initialize categories for each item in inventory
    List<string> distinctCategories = new List<string>();
    foreach (int item_id in client.players[client.ourClientId].inventory) {
      distinctCategories.Add(client.items[item_id].GetType().Name);
    }

    // Remove all duplicate categories from list
    distinctCategories = distinctCategories.Distinct().ToList();

    // Initialize list of category GameObject's, create Text object for each category, and place them in the menu
    List<GameObject> categoryList = new List<GameObject>();
    Vector3 nextPosition = Vector3.zero;
    foreach (string category in distinctCategories) {
      GameObject categoryText = Instantiate(categoryTextPrefab);
      categoryText.name = category;
      categoryText.GetComponent<Text>().text = category;
      categoryText.transform.SetParent(transform.Find("Categories Selector Panel"));
      categoryText.transform.localPosition = nextPosition;
      nextPosition = new Vector3(0, nextPosition.y - categoryText.GetComponent<RectTransform>().rect.height, 0);
      categoryList.Add(categoryText);
      currentItemIndexes.Add(0);
    }

    // Initialize list of item GameObject's, create Text objects for each item, and separate them into lists by category
    foreach (GameObject category in categoryList) {
      nextPosition = Vector3.zero;
      itemDictionary[category] = new List<GameObject>();
      foreach (int item_id in client.players[client.ourClientId].inventory) {
        if (client.items[item_id].GetType().Name == category.GetComponent<Text>().text) {
          GameObject itemText = Instantiate(itemTextPrefab);
          itemText.name = client.items[item_id].getName();
          itemText.GetComponent<Text>().text = client.items[item_id].getName();
          itemText.transform.SetParent(transform.Find("Items Selector Panel"));
          itemText.transform.localPosition = nextPosition;
          nextPosition = new Vector3(0, nextPosition.y - itemText.GetComponent<RectTransform>().rect.height, 0);
          itemText.GetComponent<ItemTextController>().id = item_id;
          itemDictionary[category].Add(itemText);
        }
      }
    }

    SetCurrentCategory();
    DisableAllItems();
    UpdateItemList();
    ToggleItemView(false);
  }

  public void CloseMenu() {
    foreach (KeyValuePair<GameObject, List<GameObject>> entry in itemDictionary) {
      Destroy(entry.Key);
      foreach (GameObject go in entry.Value) {
        Destroy(go);
      }
    }

    itemDictionary.Clear();
    currentItemIndexes.Clear();
    currentCategoryIndex = 0;
  }

  private void MoveUp() {
    if (!isItemView && currentCategoryIndex > 0) {
      MoveVertical(-1);
    } else if (isItemView && currentItemIndexes[currentCategoryIndex] > 0) {
      MoveVertical(-1);
    }
  }

  private void MoveDown() {
    if (!isItemView && currentCategoryIndex < itemDictionary.Count - 1) {
      MoveVertical(1);
    } else if (isItemView && currentItemIndexes[currentCategoryIndex] < itemDictionary[currentCategory].Count - 1) {
      MoveVertical(1);
    }
  }

  private void MoveVertical(int directionMultiplier) {
    lastScrollTime = Time.time;

    if (isItemView) {
      currentItemIndexes[currentCategoryIndex] += directionMultiplier;
      foreach (GameObject item in itemDictionary[currentCategory]) {
        item.transform.localPosition = new Vector3(0,
          item.transform.localPosition.y + item.GetComponent<RectTransform>().rect.height * directionMultiplier,
          0);
      }

      UpdateItemView();
    } else {
      currentCategoryIndex += directionMultiplier;
      // Move all text upwards
      foreach (GameObject category in itemDictionary.Keys) {
        category.transform.localPosition = new Vector3(0,
          category.transform.localPosition.y + category.GetComponent<RectTransform>().rect.height * directionMultiplier,
          0);
      }

      // Update which item list based on new category
      UpdateItemList();
    }
  }

  private void MoveRight() {
    if (!isItemView) {
      ToggleItemView(true);
      UpdateItemView();
    }
  }

  private void MoveLeft() {
    if (isItemView) {
      ToggleItemView(false);
    }
  }

  private void ToggleItemView(bool visible) {
    isItemView = visible;
    transform.Find("Items Selector Panel").gameObject.SetActive(visible);
    transform.Find("Item Panel").gameObject.SetActive(visible);
  }

  private void UpdateItemList() {
    // hide items for old category
    foreach (GameObject item in itemDictionary[currentCategory]) {
      item.GetComponent<Text>().enabled = false;
    }
    
    // switch category to current selected category
    SetCurrentCategory();
    
    // show items for new category
    foreach (GameObject item in itemDictionary[currentCategory]) {
      item.GetComponent<Text>().enabled = true;
    }
  }

  private void UpdateItemView() {
    DestroyItemView();
    CreateItemView();
  }

  private void CreateItemView() {
    Item item = GetCurrentSelectedItem();
    GameObject panel = transform.Find("Item Panel").gameObject;

    GameObject image = Instantiate(itemImagePrefab);
    image.transform.SetParent(panel.transform, false);

    GameObject name = Instantiate(itemNamePrefab);
    name.transform.SetParent(panel.transform, false);
    name.GetComponent<Text>().text = item.getName();
    
    GameObject weightStat = Instantiate(itemStatPrefab);
    weightStat.transform.SetParent(panel.transform, false);
    weightStat.transform.Find("Title").GetComponent<Text>().text = "Weight";
    weightStat.transform.Find("Value").GetComponent<Text>().text = item.getWeight().ToString();
    
    GameObject valueStat = Instantiate(itemStatPrefab);
    valueStat.transform.SetParent(panel.transform, false);
    weightStat.transform.Find("Title").GetComponent<Text>().text = "Value";
    valueStat.transform.Find("Value").GetComponent<Text>().text = item.getValue().ToString();

    switch (item.GetType().Name) {
      case "Weapon":
        GameObject damageStat = Instantiate(itemStatPrefab);
        damageStat.transform.SetParent(panel.transform, false);
        //TODO Set damage stat text to items damage
        break;
      case "Apparel":
        GameObject armorStat = Instantiate(itemStatPrefab);
        armorStat.transform.SetParent(panel.transform, false);
        //TODO Set armor stat text to items armor
        break;
      case "Potion":
        GameObject descriptionStat = Instantiate(itemDescriptionPrefab);
        descriptionStat.transform.SetParent(panel.transform, false);
        //TODO Set description stat text to items description
        break;
    }
  }

  private void DestroyItemView() {
    Transform panel = transform.Find("Item Panel");
    foreach (Transform child in panel) {
      Destroy(child.gameObject);
    }
  }

  private Item GetCurrentSelectedItem() {
    foreach (GameObject item in itemDictionary[currentCategory]) {
      if (item.transform.localPosition.y == 0) {
        return client.items[item.GetComponent<ItemTextController>().id];
      }
    }

    return null;
  }

  private void SetCurrentCategory() {
    // Set category based on height in list (in line with selector)
    foreach (GameObject category in itemDictionary.Keys) {
      if (category.transform.localPosition.y == 0) {
        currentCategory = category;
      }
    }
  }

  private void DisableAllItems() {
    foreach (List<GameObject> items in itemDictionary.Values) {
      foreach (GameObject item in items) {
        item.GetComponent<Text>().enabled = false;
      }
    }
  }
}