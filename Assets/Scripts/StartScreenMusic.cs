using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenMusic : MonoBehaviour
{
    private AudioSource audioSource;

    private bool isFading = false;
    private float fadeStartTime = 0.0f;
    private const float FADE_DURATION = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isFading) {
            float timePassed = Time.time - fadeStartTime;

            if (timePassed >= FADE_DURATION) {
                isFading = false;
                audioSource.Stop();
            } else {
                float ratio = (FADE_DURATION - timePassed) / FADE_DURATION;
                audioSource.volume = ratio;
                audioSource.pitch = Mathf.Lerp(0.75f, 1.0f, ratio);
            }
        }
    }

    public void StartFadeOut()
    {
        isFading = true;
        fadeStartTime = Time.time;
    }
}
