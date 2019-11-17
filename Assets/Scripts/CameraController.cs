using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private static CameraController singleton;

    public Vector3 initialMovement;
    public Vector3 increaseMovement;
    public float increaseInterval;

    private bool isCameraMovementEnabled;
    private float timer;
    private Vector3 currentMovement;

    void Awake() {
        if (singleton != null) {
            Debug.LogError("Multiple CameraControllers found but should only have one!");
        }
        singleton = this;
    }

    // Start is called before the first frame update
    void Start() {
        isCameraMovementEnabled = !Tutorial.GetSingleton().IsInTutorial();
        currentMovement = initialMovement;
    }

    // Update is called once per frame
    void Update() {
        if (!isCameraMovementEnabled) {
            return;
        }
        transform.position += (currentMovement * Time.deltaTime);

        if (timer > increaseInterval) {
            timer = 0;
            currentMovement += increaseMovement;
        } else {
            timer += Time.deltaTime;
        }
    }

    public static CameraController GetSingleton() {
        return singleton;
    }

    public void StartCameraMovement() {
        isCameraMovementEnabled = true;
    }

    public void StopCameraMovement() {
        isCameraMovementEnabled = false;
    }

    public bool GetCameraMovementEnabled() {
        return isCameraMovementEnabled;
    }

    public Vector3 GetCameraPosition() {
        return transform.position;
    }
}
