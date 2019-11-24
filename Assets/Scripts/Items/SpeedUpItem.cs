using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : MonoBehaviour
{
    public float speedUpAmount;

    private bool isSpawned = false;
    private int row = -1;
    private int col = -1;

    void Update() {
        if (isSpawned && !Level.GetSingleton().HasLilypadAt(row, col)) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.GetComponent<Frog>().speed += speedUpAmount;
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
