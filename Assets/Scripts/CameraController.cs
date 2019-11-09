using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Vector3 initialMovement;
    public Vector3 increaseMovement;
    public float increaseInterval;

    private float timer;
    private Vector3 currentMovement;

    // Start is called before the first frame update
    void Start() {
        currentMovement = initialMovement;
    }

    // Update is called once per frame
    void Update() {
        transform.position += (currentMovement * Time.deltaTime);

        if (timer > increaseInterval) {
            timer = 0;
            currentMovement += increaseMovement;
        } else {
            timer += Time.deltaTime;
        }
    }
}
