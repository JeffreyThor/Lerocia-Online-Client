using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
  private Client client;
  private Dictionary<string, List<GameObject>> itemDictionary;
  private string currentCategory;
  private float scrollDelay;
  private float lastScrollTime;

  // Use this for initialization
  void Start() {
    client = GameObject.Find("Client").GetComponent<Client>();
    itemDictionary = new Dictionary<string, List<GameObject>>();
    scrollDelay = 0.25f;
    lastScrollTime = 0;
  }

  // Update is called once per frame
  void Update() {
    if (client.inMenu && Time.time - lastScrollTime > scrollDelay) {
      if (Input.GetAxis("Vertical") > 0) {
        MoveUp();
      } else if (Input.GetAxis("Vertical") < 0) {
        MoveDown();
      }

      if (Input.GetAxis("Horizontal") > 0) { } else if (Input.GetAxis("Horizontal") < 0) { }
    }
  }

  public void OpenMenu() {
    // Initialize categories for each item in inventory
    List<string> distinctCategories = new List<string>();
    foreach (Item item in client.players[client.ourClientId].inventory) {
      distinctCategories.Add(item.GetType().Name);
    }

    // Remove all duplicate categories from list
    distinctCategories = distinctCategories.Distinct().ToList();

    // Initialize list of category GameObject's, create Text object for each category, and place them in the menu
    List<GameObject> categoryList = new List<GameObject>();
    Vector3 nextPosition = Vector3.zero;
    foreach (string category in distinctCategories) {
      GameObject categoryText = Instantiate(Resources.Load("Category Text")) as GameObject;
      categoryText.name = category;
      categoryText.GetComponent<Text>().text = category;
      categoryText.transform.SetParent(transform.Find("Categories Selector Panel"));
      categoryText.transform.localPosition = nextPosition;
      nextPosition = new Vector3(0, nextPosition.y - categoryText.GetComponent<RectTransform>().rect.height, 0);
      categoryList.Add(categoryText);
    }

    // Initialize list of item GameObject's, create Text objects for each item, and separate them into lists by category
    foreach (GameObject category in categoryList) {
      nextPosition = Vector3.zero;
      Debug.Log("Adding first item to list (category): " + category.GetComponent<Text>().text);
      itemDictionary[category.GetComponent<Text>().text] = new List<GameObject>();
      List<GameObject> itemList = itemDictionary[category.GetComponent<Text>().text];
      itemList.Add(category);
      foreach (Item item in client.players[client.ourClientId].inventory) {
        if (item.GetType().Name == category.GetComponent<Text>().text) {
          GameObject itemText = Instantiate(Resources.Load("Item Text")) as GameObject;
          itemText.name = item.getName();
          itemText.GetComponent<Text>().text = item.getName();
          itemText.transform.SetParent(transform.Find("Items Selector Panel"));
          itemText.transform.localPosition = nextPosition;
          nextPosition = new Vector3(0, nextPosition.y - itemText.GetComponent<RectTransform>().rect.height, 0);
          Debug.Log("Adding item to list: " + itemText.GetComponent<Text>().text);
          itemList.Add(itemText);
        }
      }
    }

    foreach (var contents in itemDictionary.Keys) {
      foreach (var listMember in itemDictionary[contents]) {
        Debug.Log("Key: " + contents + " member: " + listMember.GetComponent<Text>().text);
      }
    }

    SetCurrentCategory();
  }

  public void CloseMenu() {
    foreach (KeyValuePair<string, List<GameObject>> kvp in itemDictionary) {
      foreach (GameObject go in kvp.Value) {
        Destroy(go);
      }
    }

    itemDictionary.Clear();
  }

  private void MoveUp() {
    Move(-1);
  }

  private void MoveDown() {
    Move(1);
  }

  private void Move(int directionMultiplier) {
    lastScrollTime = Time.time;

    // Move all text upwards
    foreach (List<GameObject> list in itemDictionary.Values) {
      list[0].transform.localPosition = new Vector3(0,
        list[0].transform.localPosition.y + list[0].GetComponent<RectTransform>().rect.height * directionMultiplier,
        0);
    }

    // Update which item list based on new category
    UpdateItemList();
  }

  private void UpdateItemList() {
    // hide items for old category
    foreach (GameObject item in itemDictionary[currentCategory]) {
      Debug.Log("Disabling " + item.GetComponent<Text>().text);
      item.GetComponent<Text>().enabled = false;
    }

    // switch category to current selected category
    SetCurrentCategory();

    // show items for new category
    foreach (GameObject item in itemDictionary[currentCategory]) {
      Debug.Log("Enabling " + item.GetComponent<Text>().text);
      item.GetComponent<Text>().enabled = true;
    }
  }

  private void SetCurrentCategory() {
    // Set category based on height in list (in line with selector)
    foreach (List<GameObject> list in itemDictionary.Values) {
      if (list[0].transform.localPosition.y == 0) {
        Debug.Log("Setting category to " + list[0].GetComponent<Text>().text);
        currentCategory = list[0].GetComponent<Text>().text;
      }
    }

    Debug.Log("New current category is " + currentCategory);
  }
}