using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager singleton;

    // different biome music
    public AudioSource waterMusic;
    public AudioSource landMusic;
    public AudioSource skyMusic;
    public AudioSource spaceMusic;

    // transitioning state variables
    private bool transitioning = false;
    private AudioSource transitionFromMusic = null;
    private AudioSource transitionToMusic = null;
    private float transitionTime = 0.0f;
    private const float TOTAL_TRANSITION_DURATION = 2.0f;

    void Awake() {
        if (singleton != null) {
            Debug.LogError("Multiple Music managers found but should only have one!");
        }
        singleton = this;
    }

    public static MusicManager GetSingleton() {
        return singleton;
    }

    // Update is called once per frame
    void Update()
    {
        if (transitioning) {
            transitionTime += Time.deltaTime;

            float fromVolume = Mathf.Max(0.0f,
                (TOTAL_TRANSITION_DURATION - transitionTime) / TOTAL_TRANSITION_DURATION);
            float toVolume = Mathf.Min(1.0f,
                transitionTime / TOTAL_TRANSITION_DURATION);

            transitionFromMusic.volume = fromVolume;
            transitionToMusic.volume = toVolume;

            if (transitionTime >= TOTAL_TRANSITION_DURATION) {
                transitioning = false;
            }
        }
    }

    // Called when the frog enters the newBiome, change the music
    // accordingly
    public void ChangeBiomeMusic(Environment.BiomeType newBiome) {
        switch (newBiome) {
            case Environment.BiomeType.WaterLand:
                StartTransition(waterMusic, landMusic);
                break;
            case Environment.BiomeType.LandSky:
                StartTransition(landMusic, skyMusic);
                break;
            case Environment.BiomeType.SkySpace:
                StartTransition(skyMusic, spaceMusic);
                break;
        }
    }

    // Activate the transitioning state
    private void StartTransition(AudioSource from, AudioSource to) {
        transitioning = true;
        transitionFromMusic = from;
        transitionToMusic = to;
        transitionTime = 0.0f;
    }
}
