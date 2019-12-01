using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : MonoBehaviour
{
    public float speedUpFactor;
    public GameObject speedUpItemText;

    private Level level;

    private int row = -1;
    private int col = -1;

    void Start() {
        level = Level.GetSingleton();
    }

    void Update() {
        if (!level.HasLilypadAt(row, col)) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.GetComponent<Frog>().speed *= speedUpFactor;
            col.GetComponent<Frog>().CalculateNewJumpTime();
            col.GetComponent<Frog>().CalculateNewJumpSoundPitch();

            GameObject text = Instantiate(
                speedUpItemText, transform.position, transform.rotation);
            text.GetComponent<FloatUpText>().enabled = true;
            text.GetComponent<AudioSource>().Play();

            Destroy(gameObject);
        }
    }

    public void SpawnAt(int row, int col) {
        this.row = row;
        this.col = col;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
