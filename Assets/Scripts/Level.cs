﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    private static Level singleton;

    private Grid levelGrid;
    private Tilemap lilypadTilemap;
    private int totalLanes = 3;

    public TileBase lilypadTile;

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

        // TestStuff();
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

    public int GetTotalLanes() {
        return totalLanes;
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
        lilypadTilemap.SetTile(new Vector3Int(lane, row, 0), lilypadTile);
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
            int change = Random.Range(-1, 2);

            bool outOfRange = lastRowLane + change < 0 || 
                lastRowLane + change >= totalLanes;

            if (!outOfRange) {
                lastRowLane += change;
            }

            AddLilypad(row, lastRowLane);
        }
    }

    public void DeleteRows(int startRow, int endRow) {
        for (int row = startRow; row <= endRow; row++) {
            int lane = GetRowLilypadLane(row);
            if (lane == -1) {
                continue;
            }
            DeleteLilypad(row, lane);
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