using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ItemType { JumpSpeedDecrease, };

    private static ItemManager singleton;

    public GameObject jumpSpeedDecreaseItem;

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
            case ItemType.JumpSpeedDecrease:
                item = Instantiate(jumpSpeedDecreaseItem, position, Quaternion.identity);
                break;
            default:
                Debug.LogError("ItemManager#CreateItemAtPosition: Unhandled item type!");
                return;
        }
        item.GetComponent<BoxCollider2D>().enabled = true;
    }
}
