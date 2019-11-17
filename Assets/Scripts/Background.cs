using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform bgWater;
    public Transform bgRock;
    public Transform bgWaterToRock;

    private Environment environmentSingleton;

    public const int BACKGROUND_GRAPHICS_SIZE = 6;

    // Start is called before the first frame update
    void Start()
    {
        environmentSingleton = Environment.GetSingleton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetBgRowFromFrogRow(int frogRow) {
        return frogRow / BACKGROUND_GRAPHICS_SIZE;
    }

    Vector3 GetWorldPosFromBgRow(int bgRow) {
        return new Vector3(1.5f, 3.0f + (bgRow * BACKGROUND_GRAPHICS_SIZE), 0.0f);
    }

    private void AddBgForBgRow(int bgRow, Transform bgPrefab) {
        GameObject.Instantiate(bgPrefab, GetWorldPosFromBgRow(bgRow), Quaternion.identity);
    }

    public void SpawnBg(int bgRow)
    {
        int biomeRow = bgRow * BACKGROUND_GRAPHICS_SIZE;
        int nextBiomeRow = biomeRow + BACKGROUND_GRAPHICS_SIZE;

        Environment.BiomeType biome = environmentSingleton.GetBiomeAt(biomeRow);
        Environment.BiomeType nextBiome = environmentSingleton.GetBiomeAt(nextBiomeRow);
        
        bool transitionNeeded = (biome != nextBiome);

        if (!transitionNeeded) {
            switch (biome) {
                case Environment.BiomeType.Water:
                    AddBgForBgRow(bgRow, bgWater);
                    break;

                case Environment.BiomeType.Rock:
                    AddBgForBgRow(bgRow, bgRock);
                    break;

                default:
                    Debug.LogError(string.Format("Background: We did not handle '{0}'.", biome));
                    break;
            }
        } else {
            switch (biome) {
                case Environment.BiomeType.Water:
                {
                    switch (nextBiome) {
                    case Environment.BiomeType.Rock:
                        AddBgForBgRow(bgRow, bgWaterToRock);
                    break;

                    default:
                        Debug.LogError(string.Format("Background: We did not handle '{0} to {1}'.",
                            biome, nextBiome));
                        break;
                    }
                }
                break;
                
                default:
                    Debug.LogError(string.Format("Background: We did not handle '{0} to {1}'.",
                        biome, nextBiome));
                    break;
            }
        }
    }
}
