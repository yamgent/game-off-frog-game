﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private static CameraController singleton;

    public Vector3 initialMovement;
    public Vector3 increaseMovement;
    public float increaseInterval;
    public Vector3 boostAmount;
    public float boostDuration;

    private bool isCameraMovementEnabled;
    private float timer;
    private Vector3 currentMovement;
    private bool isBoost;
    private float boostTimer;

    void Awake() {
        if (singleton != null) {
            Debug.LogError("Multiple CameraControllers found but should only have one!");
        }
        singleton = this;
    }

    // Start is called before the first frame update
    void Start() {
        isCameraMovementEnabled = Tutorial.GetSingleton().IsTutorialEnded();
        currentMovement = initialMovement;
    }

    // Update is called once per frame
    void Update() {
        if (!isCameraMovementEnabled) {
            return;
        }

        Vector3 moveAmount = currentMovement;
        if (isBoost) {
            moveAmount += boostAmount;
            if (boostTimer > boostDuration) {
                boostTimer = 0;
                isBoost = false;
            } else {
                boostTimer += Time.deltaTime;
            }
        }
        transform.position += (moveAmount * Time.deltaTime);

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

    public void StartBoostCameraSpeed() {
        isBoost = true;
    }

    public float GetCameraMovementSpeed() {
        return currentMovement.y;
    }
}
