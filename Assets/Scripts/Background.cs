using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the background graphics of the level
// Each background graphic spans across 6 rows
// so:
//  - bgRow = 0 covers frogRow = 0 to 5
//  - bgRow = 1 covers frogRow = 6 to 11
//  - bgRow = 2 covers frogRow = 12 to 17
//  - etc.
public class Background : MonoBehaviour
{
    public Transform bgWater;
    public Transform bgWaterToRock;
    public Transform bgRock;
    public Transform bgRockToSky;
    public Transform bgSky;

    private Environment environmentSingleton;

    public const int BACKGROUND_GRAPHICS_SIZE = 6;

    private Dictionary<int, Transform> bgRowSprites = new Dictionary<int, Transform>();

    // Start is called before the first frame update
    void Start()
    {
        environmentSingleton = Environment.GetSingleton();
    }

    // Get the corresponding bgRow given the frog's row
    public int GetBgRowFromFrogRow(int frogRow) {
        return frogRow / BACKGROUND_GRAPHICS_SIZE;
    }

    // Get the actual sprite's world coordinate given the bgRow
    Vector3 GetWorldPosFromBgRow(int bgRow) {
        return new Vector3(1.5f, 3.0f + (bgRow * BACKGROUND_GRAPHICS_SIZE), 0.0f);
    }

    // Create a new bg sprite, and associate the sprite with the bgRow
    private void AddSpriteForBgRow(int bgRow, Transform bgPrefab) {
        if (bgRowSprites.ContainsKey(bgRow)) {
            // we already have a background for this bgRow
            return;
        }

        Transform bgTransform = GameObject.Instantiate(bgPrefab, 
            GetWorldPosFromBgRow(bgRow), 
            Quaternion.identity, 
            transform);

        bgRowSprites.Add(bgRow, bgTransform);
    }

    // Delete the sprite for a corresponding bgRow
    private void DeleteSpriteForBgRow(int bgRow) {
        if (!bgRowSprites.ContainsKey(bgRow)) {
            // this bgRow does not exist
            return;
        }

        Destroy(bgRowSprites[bgRow].gameObject);
        bgRowSprites.Remove(bgRow);
    }

    // Spawn in the bg for a particular bgRow
    public void SpawnBg(int bgRow)
    {
        if (bgRowSprites.ContainsKey(bgRow)) {
            // we already have a background for this bgRow
            return;
        }

        int biomeRow = bgRow * BACKGROUND_GRAPHICS_SIZE;

        Environment.BiomeType biome = environmentSingleton.GetBiomeAt(biomeRow);

        switch (biome) {
            case Environment.BiomeType.Water:
                AddSpriteForBgRow(bgRow, bgWater);
                break;

            case Environment.BiomeType.WaterLand:
                AddSpriteForBgRow(bgRow, bgWaterToRock);
                break;

            case Environment.BiomeType.Land:
                AddSpriteForBgRow(bgRow, bgRock);
                break;

            case Environment.BiomeType.LandSky:
                AddSpriteForBgRow(bgRow, bgRockToSky);
                break;

            case Environment.BiomeType.Sky:
                AddSpriteForBgRow(bgRow, bgSky);
                break;

            default:
                Debug.LogError(string.Format("Background: We did not handle '{0}'.", biome));
                break;
        }
    }
    
    // Delete the bg for a particular bgRow
    public void DespawnBg(int bgRow) {
        DeleteSpriteForBgRow(bgRow);
    }
}
