using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    private static Level singleton;

    private Grid levelGrid;
    private Tilemap lilypadTilemap;

    void Start() {
        if (singleton != null) {
            Debug.LogError("Multiple Level managers found but should only have one!");
        }
        singleton = this;

        levelGrid = GetComponent<Grid>();
        if (levelGrid == null) {
            Debug.LogError("Cannot find Grid component!");
        }

        lilypadTilemap = null;
        foreach (Tilemap childTilemap in GetComponentsInChildren<Tilemap>()) {
            if (childTilemap.CompareTag("LilypadLayer")) {
                lilypadTilemap = childTilemap;
                break;
            }
        }
        if (lilypadTilemap == null) {
            Debug.LogError("Cannot find Lilypad tilemap layer!");
        }
    }

    public static Level GetSingleton() {
        return singleton;
    }

    public bool HasLilypadAt(int row, int lane) {
        return lilypadTilemap.GetSprite(new Vector3Int(lane, row, 0)) != null;
    }

    public Vector3 GetLilypadOriginWorldCoordinate(int row, int lane) {
        return levelGrid.CellToWorld(new Vector3Int(lane, row, 0))
            + new Vector3(0.5f, 0.5f, 0.0f);
    }

    /*
    void TestStuff() {
        for (int r = 0; r <= 6; r++) {
            for (int l = 0; l <= 2; l++) {
                Debug.Log("HasLilypadAt(" + r + ", " + l + "): " + HasLilypadAt(r, l));
            }
        }

        for (int r = 0; r <= 6; r++) {
            for (int l = 0; l <= 2; l++) {
                Debug.Log("Origin(" + r + ", " + l + "): " + GetLilypadOriginWorldCoordinate(r, l));
            }
        }
    }
    */
}
