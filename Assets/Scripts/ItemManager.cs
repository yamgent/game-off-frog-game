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

    public void CreateItem(ItemType itemType, int row, int col) {
        GameObject item = null;
        switch (itemType) {
            case ItemType.SpeedUpItem:
                item = Instantiate(
                    speedUpItem,
                    Level.GetSingleton().GetLilypadOriginWorldCoordinate(row, col),
                    Quaternion.identity);
                item.GetComponent<SpeedUpItem>().spawnAt(row, col);
                break;
            default:
                Debug.LogError("ItemManager#CreateItemAtPosition: Unhandled item type!");
                return;
        }
        item.GetComponent<BoxCollider2D>().enabled = true;
    }
}
