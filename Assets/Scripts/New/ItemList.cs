using UnityEngine;

namespace New {
  using System.Collections.Generic;
  
  public static class ItemList {
    public static readonly List<Item> Items = new List<Item> {
      new HealthPotion(
        0,
        "health potion",
        1,
        10,
        20
      ),
      new Weapon(
        1,
        "weapon",
        10,
        20,
        15
      ),
      new Apparel(
        2,
        "armor",
        5,
        10,
        10
      )
    };
    
    public static Dictionary<int, GameObject> WorldItems = new Dictionary<int, GameObject>();
  }
}