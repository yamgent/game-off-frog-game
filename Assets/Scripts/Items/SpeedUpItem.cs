using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : MonoBehaviour
{
    public float speedUpAmount;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.GetComponent<Frog>().speed += speedUpAmount;
            col.GetComponent<Frog>().CalculateNewJumpTime();
            Destroy(gameObject);
        }
    }
}
