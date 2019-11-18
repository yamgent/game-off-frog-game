using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    public GameObject target;
    public float trackingDistance;
    public Vector3 startPosition;
    public float entranceTime;
    public float flapInterval;

    private bool isChasing;
    private float timer;

    private Animator animator;

    void Start() {
        isChasing = false;
        timer = 0;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (CameraController.GetSingleton().GetCameraMovementEnabled() && !isChasing) {
            if (timer < entranceTime) {
                timer += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, CameraController.GetSingleton().GetCameraPosition() + startPosition, 0.05f);
            } else {
                isChasing = true;
                timer = 0;
            }
        }

        if (isChasing) {
            Vector3 pos = transform.position;
            if (target.transform.position.y - pos.y < trackingDistance) {
                pos.x = target.transform.position.x;
                animator.SetBool("Flapping", true);
            } else {
                pos.x = CameraController.GetSingleton().GetCameraPosition().x;
                if (timer < flapInterval) {
                    timer += Time.deltaTime;
                    animator.SetBool("Flapping", false);
                } else {
                    timer = 0;
                    animator.SetBool("Flapping", true);
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, pos, 0.05f);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            col.GetComponent<Frog>().Die();
        }
    }
}
