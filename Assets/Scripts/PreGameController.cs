using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameController : MonoBehaviour {

    public Vector3 cameraFinalPosition;
    public float scrollTime;

    private Vector3 cameraInitialPosition;
    private float timer;

    void Start() {
        cameraInitialPosition = Camera.main.transform.position;
        timer = 0;
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer > scrollTime) {
            Tutorial.GetSingleton().StartTutorial();
            Destroy(this.gameObject);
        } else {
            Camera.main.transform.position = Vector3.Lerp(cameraInitialPosition, cameraFinalPosition, timer / scrollTime);
        }
    }
}
