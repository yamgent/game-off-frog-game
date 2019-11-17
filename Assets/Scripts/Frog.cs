using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public float jumpTime;

    private float timer;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    public bool isMoving { get; private set; }
    public bool isDead { get; private set; }
    public int currentRow { get; private set; }
    public int currentCol { get; private set; }

    private Animator animator;

    // Start is called before the first frame update
    void Start() {
        isMoving = false;
        isDead = false;
        timer = 0;
        startPosition = transform.position;

        currentRow = 0;
        currentCol = 2;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (isMoving) {
            if (timer > jumpTime) {
                isMoving = false;
                timer = 0;
                startPosition = targetPosition;
                if (isDead) {
                    
                }
            } else {
                timer += Time.deltaTime;
                float time = Mathf.Min(1, timer / jumpTime);
                transform.position = Vector2.Lerp(startPosition, targetPosition, time);
                selectSprite(time);
            }
        }
    }

    public void move(int rowBy, int colBy) {
        currentRow += rowBy;
        currentCol += colBy;

        if (Level.GetSingleton().HasLilypadAt(currentRow, currentCol)) {
            Score.GetSingleton().AddToCurrentScore(rowBy);
        } else {
            isDead = true;
        }
        targetPosition = Level.GetSingleton().GetLilypadOriginWorldCoordinate(currentRow, currentCol);
        isMoving = true;
        Level.GetSingleton().UpdateFrogPosition(currentRow);
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
