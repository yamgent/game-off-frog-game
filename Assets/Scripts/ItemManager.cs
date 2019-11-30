using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ItemType { SpeedUpItem, SpeedUpItemSpace };

    private static ItemManager singleton;

    public GameObject speedUpItem;
    public GameObject speedUpItemSpace;

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
            case ItemType.SpeedUpItemSpace:
                GameObject toSpawn = speedUpItem;
                if (itemType == ItemType.SpeedUpItemSpace) {
                    toSpawn = speedUpItemSpace;
                }
                item = Instantiate(
                    toSpawn,
                    Level.GetSingleton().GetLilypadOriginWorldCoordinate(row, col),
                    Quaternion.identity);
                SpeedUpItem itemScript = item.GetComponent<SpeedUpItem>();
                itemScript.spawnAt(row, col);
                itemScript.enabled = true;
                break;
            default:
                Debug.LogError("ItemManager#CreateItemAtPosition: Unhandled item type!");
                return;
        }
    }
}
