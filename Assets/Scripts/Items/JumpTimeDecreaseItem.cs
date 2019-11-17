using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTimeDecreaseItem : MonoBehaviour
{
    public float decreaseInSeconds;

    void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Player") {
            col.GetComponent<Frog>().DecreaseJumpTime(decreaseInSeconds);
            Destroy(gameObject);
        }
    }
}
