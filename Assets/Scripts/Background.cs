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

    private Dictionary<int, Transform> bgRowObject = new Dictionary<int, Transform>();

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
        if (bgRowObject.ContainsKey(bgRow)) {
            // we already have a background for this bgRow
            return;
        }

        Transform bgTransform = GameObject.Instantiate(bgPrefab, 
            GetWorldPosFromBgRow(bgRow), 
            Quaternion.identity, 
            transform);

        bgRowObject.Add(bgRow, bgTransform);
    }

    private void DeleteBgForBgRow(int bgRow) {
        if (!bgRowObject.ContainsKey(bgRow)) {
            // this bgRow does not exist
            return;
        }

        Destroy(bgRowObject[bgRow].gameObject);
        bgRowObject.Remove(bgRow);
    }

    public void DespawnBg(int bgRow) {
        DeleteBgForBgRow(bgRow);
    }

    public void SpawnBg(int bgRow)
    {
        if (bgRowObject.ContainsKey(bgRow)) {
            // we already have a background for this bgRow
            return;
        }

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
