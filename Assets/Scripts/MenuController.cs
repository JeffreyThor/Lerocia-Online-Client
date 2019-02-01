using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
  private Client client;
  private Dictionary<GameObject, List<GameObject>> itemDictionary;
  private float scrollDelay;
  private float lastScrollTime;
  private List<GameObject> currentList;

  // Use this for initialization
  void Start() {
    client = GameObject.Find("Client").GetComponent<Client>();
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
    // Initialize dictionary of all items with key->value being category->items
    itemDictionary = new Dictionary<GameObject, List<GameObject>>();

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
      categoryText.GetComponent<Text>().text = category;
      categoryText.transform.SetParent(transform.Find("Categories Selector Panel"));
      categoryText.transform.localPosition = nextPosition;
      nextPosition = new Vector3(0, nextPosition.y - categoryText.GetComponent<RectTransform>().rect.height, 0);
      categoryList.Add(categoryText);
    }

    // Initialize list of item GameObject's, create Text objects for each item, and separate them into lists by category
    List<GameObject> itemList = new List<GameObject>();
    foreach (var category in categoryList) {
      itemList.Clear();
      nextPosition = Vector3.zero;
      foreach (Item item in client.players[client.ourClientId].inventory) {
        if (item.GetType().Name == category.GetComponent<Text>().text) {
          GameObject itemText = Instantiate(Resources.Load("Item Text")) as GameObject;
          itemText.GetComponent<Text>().text = item.getName();
          itemText.transform.SetParent(transform.Find("Items Selector Panel"));
          itemText.transform.localPosition = nextPosition;
          nextPosition = new Vector3(0, nextPosition.y - itemText.GetComponent<RectTransform>().rect.height, 0);
          Debug.Log("Adding " + item.getName() + " to " + item.GetType().Name + " list.");
          itemList.Add(itemText);
        }
      }

      // Add list for category to dictionary
      itemDictionary.Add(category, itemList);
    }
  }

  public void CloseMenu() {
    foreach (KeyValuePair<GameObject, List<GameObject>> kvp in itemDictionary) {
      Destroy(kvp.Key);
      foreach (GameObject go in kvp.Value) {
        Destroy(go);
      }
    }

    itemDictionary.Clear();
  }

  private void MoveUp() {
    lastScrollTime = Time.time;
  }

  private void MoveDown() {
    lastScrollTime = Time.time;
  }
}