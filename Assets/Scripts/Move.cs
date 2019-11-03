using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float jumpTime;

    private bool isMoving;
    private float timer;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        timer = 0;
        startPosition = transform.position;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) {
            if (timer > jumpTime) {
                isMoving = false;
                timer = 0;
                startPosition = targetPosition;
            } else {
                timer += Time.deltaTime;
                float time = Mathf.Min(1, timer / jumpTime);
                transform.position = Vector2.Lerp(startPosition, targetPosition, time);
                selectSprite(time);
            }
        } else {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                targetPosition = transform.position + new Vector3(0, 2, 0);
                isMoving = true;
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                targetPosition = transform.position + new Vector3(-2, 2, 0);
                isMoving = true;
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                targetPosition = transform.position + new Vector3(2, 2, 0);
                isMoving = true;
            }
        }
    }

    private void selectSprite(float time) {
        if (time >= 1) {
            animator.SetBool("Jumping", false);
        } else if (time > 0.9f) {
            animator.SetBool("Jumping", true);
            animator.SetBool("FullJump", false);
        } else if (time > 0.2f) {
            animator.SetBool("FullJump", true);
        } else if (time > 0.1f) {
            animator.SetBool("Jumping", true);
            animator.SetBool("FullJump", false);
        } else {
            animator.SetBool("Jumping", false);
        }
    }
}
