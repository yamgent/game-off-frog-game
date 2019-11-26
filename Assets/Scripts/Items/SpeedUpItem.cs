using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : MonoBehaviour
{
    public float speedUpFactor;

    private Level level;

    private bool isSpawned = false;
    private int row = -1;
    private int col = -1;

    void Start() {
        level = Level.GetSingleton();
    }

    void Update() {
        if (isSpawned && !level.HasLilypadAt(row, col)) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.GetComponent<Frog>().speed *= speedUpFactor;
            col.GetComponent<Frog>().CalculateNewJumpTime();
            Destroy(gameObject);
        }
    }

    public void spawnAt(int row, int col) {
        this.isSpawned = true;
        this.row = row;
        this.col = col;
    }
}
