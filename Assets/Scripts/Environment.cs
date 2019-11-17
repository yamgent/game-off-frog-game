using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public enum BiomeType {
        Water = 0,
        Rock,
        Sky,
        COUNT
    }

    // See GetBiomeIndex(int) to see how this is used
    public const int BIOME_SIZE = 9;

    private int rockBiomeStartIndex;
    private int skyBiomeStartIndex;

    private static Environment singleton;

    void Awake() {
        if (singleton != null) {
            Debug.LogError("Multiple environment managers found but should only have one!");
        }
        singleton = this;
    }

    void Start() {
        // TODO refactor the magic number out
        rockBiomeStartIndex = Random.Range(2, 3);
        skyBiomeStartIndex = rockBiomeStartIndex + Random.Range(1, 2);
    }

    public static Environment GetSingleton() {
        return singleton;
    }

    // Row 0 to 5 is in the 0th biome, row 6 to 10 is
    // in the 1st biome, etc
    private int GetBiomeIndex(int row) {
        return row / BIOME_SIZE;
    }

    // Get the biome at a particular row. If the biome does
    // not exist for a particular row, then generate the
    // biome for that
    public BiomeType GetBiomeAt(int row) {
        int biomeIndex = GetBiomeIndex(row);
        if (biomeIndex < rockBiomeStartIndex) {
            return BiomeType.Water;
        } else if (biomeIndex < skyBiomeStartIndex) {
            return BiomeType.Rock;
        } else {
            return BiomeType.Sky;
        }
    }
}
