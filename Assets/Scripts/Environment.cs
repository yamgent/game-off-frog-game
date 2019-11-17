using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public enum BiomeType {
        Water,
        WaterLand,
        Land,
        LandSky,
        Sky,
        SkySpace,
        Space
    }

    // See GetBiomeIndex(int) to see how this is used
    public const int BIOME_SIZE = 6;

    public Biome[] biomes;

    private static Environment singleton;

    void Awake() {
        if (singleton != null) {
            Debug.LogError("Multiple environment managers found but should only have one!");
        }
        singleton = this;
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
        BiomeType type = BiomeType.Water;
        for (int i = 0; i < biomes.Length; i++) {
            biomeIndex -= biomes[i].repeat;
            type = biomes[i].type;
            if (biomeIndex <= 0) {
                break;
            }
        }
        return type;
    }
}

[System.Serializable]
public class Biome {
    public Environment.BiomeType type;
    public int repeat;
}
