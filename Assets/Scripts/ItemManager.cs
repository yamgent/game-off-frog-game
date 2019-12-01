using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public enum ItemType { SpeedUpItem, SpeedUpItemSpace, DragonflyItem, DragonflyItemSpace };

    private static ItemManager singleton;

    public GameObject speedUpItem;
    public GameObject speedUpItemSpace;
    public GameObject dragonflyItem;
    public GameObject dragonflyItemSpace;

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
        bool isSpeedUp = false;
        bool isAutoMove = false;

        GameObject toSpawn = null;
        switch (itemType) {
            case ItemType.SpeedUpItem:
                toSpawn = speedUpItem;
                isSpeedUp = true;
                break;
            case ItemType.SpeedUpItemSpace:
                toSpawn = speedUpItemSpace;
                isSpeedUp = true;
                break;
            case ItemType.DragonflyItem:
                toSpawn = dragonflyItem;
                isSpeedUp = true;
                isAutoMove = true;
                break;
            case ItemType.DragonflyItemSpace:
                toSpawn = dragonflyItemSpace;
                isSpeedUp = true;
                isAutoMove = true;
                break;
            default:
                Debug.LogError("ItemManager#CreateItemAtPosition: Unhandled item type!");
                return;
        }

        GameObject item = Instantiate(
            toSpawn,
            Level.GetSingleton().GetLilypadOriginWorldCoordinate(row, col),
            Quaternion.identity);
        if (isSpeedUp) {
            SpeedUpItem speedUpItemScript = item.GetComponent<SpeedUpItem>();
            speedUpItemScript.SpawnAt(row, col);
            speedUpItemScript.enabled = true;
        }
        if (isAutoMove) {
            AutoMove autoMoveScript = item.GetComponent<AutoMove>();
            autoMoveScript.SpawnAt(row, col);
        }
    }
}
