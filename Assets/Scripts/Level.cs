using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    private static Level singleton;

    private Grid levelGrid;
    private Tilemap lilypadTilemap;
    private int totalLanes = 3;
    private int firstGeneratedIndex = 0;
    private int lastGeneratedIndex = 0;

    public TileBase lilypadTile;
    public TileBase rockTile;
    public TileBase lilypadToRockTile;
    public TileBase cloudTile;
    public TileBase asteroidTile;
    public int lilypadBuffer = 10;

    private Environment environmentSingleton;
    public Background background;

    private int lastGeneratedBgRow = 0;
    private int lastRemovedBgRow = -1;

    void Awake() {
        if (singleton != null) {
            Debug.LogError("Multiple Level managers found but should only have one!");
        }
        singleton = this;
    }

    void Start() {
        environmentSingleton = Environment.GetSingleton();

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

        if (Tutorial.GetSingleton().IsInTutorial()) {
            foreach (int tutorialLane in Tutorial.GetSingleton().GetLaneSequence()) {
                lastGeneratedIndex++;
                AddLilypad(lastGeneratedIndex, tutorialLane);
            }
        }

        // spawn our starting area's background
        background.SpawnBg(0);
        lastGeneratedBgRow = 0;

        // TestStuff();
        GenerateRows(lastGeneratedIndex + 1, 20);
        lastGeneratedIndex = 20;

        /* Test items. */
        //ItemManager.GetSingleton().CreateItemAtPosition(ItemManager.ItemType.JumpSpeedDecrease, new Vector3(1.5f, 3.5f, 0.0f));
    }

    public static Level GetSingleton() {
        return singleton;
    }

    public bool HasLilypadAt(int row, int lane) {
        return lilypadTilemap.GetSprite(new Vector3Int(lane, row, 0)) != null;
    }

    public bool IsWithinLaneBounds(int lane) {
        return lane >= 0 && lane < totalLanes;
    }

    public Vector3 GetLilypadOriginWorldCoordinate(int row, int lane) {
        return levelGrid.CellToWorld(new Vector3Int(lane, row, 0))
            + new Vector3(0.5f, 0.5f, 0.0f);
    }

    // get the lane number of the lilypad for a particular row
    private int GetRowLilypadLane(int row) {
        for (int lane = 0; lane < totalLanes; lane++) {
            if (HasLilypadAt(row, lane)) {
                return lane;
            }
        }

        return -1;
    }

    private void AddLilypad(int row, int lane) {
        Environment.BiomeType rowBiome = environmentSingleton.GetBiomeAt(row);

        switch (rowBiome) {
            case Environment.BiomeType.Space:
                lilypadTilemap.SetTile(new Vector3Int(lane, row, 0), asteroidTile);
                break;

            case Environment.BiomeType.SkySpace:
            case Environment.BiomeType.Sky:
                lilypadTilemap.SetTile(new Vector3Int(lane, row, 0), cloudTile);
                break;

            case Environment.BiomeType.LandSky:
            case Environment.BiomeType.Land:
                lilypadTilemap.SetTile(new Vector3Int(lane, row, 0), rockTile);
                break;

            case Environment.BiomeType.WaterLand:
                lilypadTilemap.SetTile(new Vector3Int(lane, row, 0), lilypadToRockTile);
                break;

            case Environment.BiomeType.Water:
                lilypadTilemap.SetTile(new Vector3Int(lane, row, 0), lilypadTile);
                break;

            default:
                Debug.LogError(string.Format("Background: We did not handle '{0}'.", rowBiome));
                break;
        }
    }

    private void DeleteLilypad(int row, int lane) {
        lilypadTilemap.SetTile(new Vector3Int(lane, row, 0), null);
    }

    // generate new lilypads from startRow to endRow. The startRow - 1
    // must have a valid lilypad.
    public void GenerateRows(int startRow, int endRow) {
        int lastRowLane = GetRowLilypadLane(startRow - 1);

        if (lastRowLane < 0) {
            Debug.LogError("Row before startRow " + startRow 
                + "is deleted, but the frog may not even have reached that row!");
            return;
        }

        for (int row = startRow; row <= endRow; row++) {
            int currentRowLane = lastRowLane;

            if (currentRowLane <= 0) {
                currentRowLane = Random.Range(0, 2);
            } else if (currentRowLane >= totalLanes - 1) {
                currentRowLane = Random.Range(totalLanes - 2, totalLanes);
            } else {
                currentRowLane = Random.Range(currentRowLane - 1, currentRowLane + 2);
            }

            AddLilypad(row, currentRowLane);

            int currentRowBgRow = background.GetBgRowFromFrogRow(row);
            if (currentRowBgRow != lastGeneratedBgRow) {
                background.SpawnBg(currentRowBgRow);
                lastGeneratedBgRow = currentRowBgRow;
            }

            lastRowLane = currentRowLane;
        }
    }

    public void DeleteRows(int startRow, int endRow) {
        for (int row = startRow; row <= endRow; row++) {
            int lane = GetRowLilypadLane(row);
            if (lane == -1) {
                continue;
            }
            DeleteLilypad(row, lane);

            // the previous background will be devoid of lilypads/rocks
            if (background.GetBgRowFromFrogRow(row + 1) > lastRemovedBgRow + 1) {
                int previousBgRow = background.GetBgRowFromFrogRow(row) - 1;
                background.DespawnBg(previousBgRow);
                lastRemovedBgRow = previousBgRow;
            }
        }
    }

    public void UpdateFrogPosition(int row) {
        if (row + lilypadBuffer >= lastGeneratedIndex) {
            int generateTo = lastGeneratedIndex + lilypadBuffer;
            GenerateRows(lastGeneratedIndex + 1, generateTo);
            lastGeneratedIndex = generateTo;
        }

        if (row - (2 * lilypadBuffer) > firstGeneratedIndex) {
            int removeTo = firstGeneratedIndex + lilypadBuffer;
            DeleteRows(firstGeneratedIndex, removeTo);
            firstGeneratedIndex = removeTo;
        }
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

        for (int r = 0; r < 10; r++) {
            Debug.Log("Row " + r + ": " + GetRowLilypadLane(r));
        }
        GenerateRows(7, 12);
        DeleteRows(0, 4);
    }
    */
}
