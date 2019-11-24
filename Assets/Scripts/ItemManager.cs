using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ItemType { SpeedUpItem, };

    private static ItemManager singleton;

    public GameObject speedUpItem;

    void Awake() {
        if (singleton != null) {
            Debug.LogError("Multiple ItemManagers found but should only have one!");
        }
        singleton = this;
    }

    public static ItemManager GetSingleton() {
        return singleton;
    }

    public void CreateItemAtPosition(ItemType itemType, Vector3 position) {
        GameObject item = null;
        switch (itemType) {
            case ItemType.SpeedUpItem:
                item = Instantiate(speedUpItem, position, Quaternion.identity);
                break;
            default:
                Debug.LogError("ItemManager#CreateItemAtPosition: Unhandled item type!");
                return;
        }
        item.GetComponent<BoxCollider2D>().enabled = true;
    }
}
