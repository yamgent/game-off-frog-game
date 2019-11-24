using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    private static Level singleton;

    private Grid levelGrid;
    private Tilemap lilypadTilemap;
    private int totalLanes = 5;
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

    // the row where we split from n -> (n + 1) paths
    private int splitPathRow = 0;
    // the row where we start to merge all of the paths into 1 single path
    private int startMergePathRow = 0;

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

        SetUpNextSplitPath(0);

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
    private int[] GetRowLilypadLanes(int row) {
        List<int> results = new List<int>();

        for (int lane = 0; lane < totalLanes; lane++) {
            if (HasLilypadAt(row, lane)) {
                results.Add(lane);
            }
        }

        return results.ToArray();
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

    // given the position of the previous row's lilypad's lane, generate a new
    // lilypad at the current row (either directly in front, or to the left,
    // or to the right)
    private void GenerateRandomLilypad(int currentRow, 
        int prevRowLilypadLane) {

        List<int> possibleLanes = new List<int>();

        int leftLane = prevRowLilypadLane - 1;
        if (IsWithinLaneBounds(leftLane) 
            && !HasLilypadAt(currentRow, leftLane)) {

            possibleLanes.Add(leftLane);
        }

        int rightLane = prevRowLilypadLane + 1;
        if (IsWithinLaneBounds(rightLane)
            && !HasLilypadAt(currentRow, rightLane)) {
            
            possibleLanes.Add(rightLane);
        }

        int straight = prevRowLilypadLane;
        if (IsWithinLaneBounds(straight) 
            && !HasLilypadAt(currentRow, straight)) {

            possibleLanes.Add(straight);
        }

        Debug.Assert(possibleLanes.Count > 0);

        int chosenLaneIndex = Random.Range(0, possibleLanes.Count);
        AddLilypad(currentRow, possibleLanes[chosenLaneIndex]);
    }

    public void SetUpNextSplitPath(int baseRow) {
        // set up the next split path

        // TODO: Randomize this?
        splitPathRow = baseRow + 10;
        startMergePathRow = splitPathRow + 10;
    }

    // generate new lilypads from startRow to endRow. The startRow - 1
    // must have a valid lilypad.
    public void GenerateRows(int startRow, int endRow) {
        int[] lastRowLanes = GetRowLilypadLanes(startRow - 1);

        if (lastRowLanes.Length == 0) {
            Debug.LogError("Row before startRow " + startRow 
                + "is deleted, but the frog may not even have reached that row!");
            return;
        }

        for (int row = startRow; row <= endRow; row++) {
            // lilypad generation

            // merging phase
            if (row >= startMergePathRow) {
                // "gravitate" the lilypads towards the centre point
                // of the group of lilypads
                int midPoint = 0;
                foreach (int lane in lastRowLanes) {
                    midPoint += lane;
                }
                midPoint /= lastRowLanes.Length;
                
                // only add the lilypad if they do not
                // move beyond the centre point
                foreach (int lane in lastRowLanes) {
                    bool goRight = (midPoint - lane) > 0;
                    
                    if (goRight) {
                        if (lane + 1 <= midPoint) {
                            AddLilypad(row, lane + 1);
                        }
                    } else {
                        if (lane - 1 >= midPoint) {
                            AddLilypad(row, lane - 1);
                        }
                    }
                }

                // check if merging ended, if so, start preparing
                // for the next split path
                if (GetRowLilypadLanes(row).Length == 1) {
                    SetUpNextSplitPath(row);
                }

            // normal / splitting phase
            } else {
                // generate next lilypad for each path
                foreach (int lane in lastRowLanes) {
                    GenerateRandomLilypad(row, lane);
                }

                // do we need to split?
                if (row == splitPathRow) {
                    // generate a new lilypad for the new path
                    // by branching off the 0th path into two
                    GenerateRandomLilypad(row, lastRowLanes[0]);
                }
            }
                
            lastRowLanes = GetRowLilypadLanes(row);

            // background generation
            int currentRowBgRow = background.GetBgRowFromFrogRow(row);
            if (currentRowBgRow != lastGeneratedBgRow) {
                background.SpawnBg(currentRowBgRow);
                lastGeneratedBgRow = currentRowBgRow;
            }
        }
    }

    public void DeleteRows(int startRow, int endRow) {
        for (int row = startRow; row <= endRow; row++) {
            foreach (int lane in GetRowLilypadLanes(row)) {
                DeleteLilypad(row, lane);
            }

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
            Debug.Log("Row " + r + ": " + GetRowLilypadLanes(r).ToString());
        }
        GenerateRows(7, 12);
        DeleteRows(0, 4);
    }
    */
}
