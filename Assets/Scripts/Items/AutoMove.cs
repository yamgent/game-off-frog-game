using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Script which alternates between auto-moving and pausing the game object. */
public class AutoMove : MonoBehaviour
{
    public float moveTime;
    public float idleTimeFactor;
    public float distanceToStart;

    private bool isAutoMoveStarted;
    private bool isMoving;
    private float idleTime;
    private float timer;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private int currentRow;
    private int currentCol;

    private Animator animator;

    private CameraController cameraController;
    private Level level;

    // Start is called before the first frame update
    void Start() {
        cameraController = CameraController.GetSingleton();
        level = Level.GetSingleton();

        isAutoMoveStarted = false;
        isMoving = false;

        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (!isAutoMoveStarted) {
            if (cameraController.GetCameraPosition().y + distanceToStart >= transform.position.y) {
                StartAutoMove();
            }
            return;
        }

        if (!level.HasLilypadAt(currentRow, currentCol)) {
            Destroy(gameObject);
            return;
        }

        timer += Time.deltaTime;
        if (isMoving) {
            if (timer > moveTime) {
                isMoving = false;
                timer = 0;
            } else {
                float time = Mathf.Min(1, timer / moveTime);
                transform.position = Vector2.Lerp(startPosition, targetPosition, time);
                //SelectSprite(time);
            }
        } else {
            if (timer > idleTime) {
                isMoving = true;
                timer = 0;
                startPosition = transform.position;
                CalculateTargetPosition();
            }
        }
    }

    public void SpawnAt(int row, int col) {
        currentRow = row;
        currentCol = col;
    }

    private void StartAutoMove() {
        if (isAutoMoveStarted) {
            return;
        }
        isAutoMoveStarted = true;
        // Pause time decreases as camera speed increases.
        idleTime = idleTimeFactor / cameraController.GetCameraMovementSpeed();
    }

    private void CalculateTargetPosition() {
        // Pick the next possible path randomly.
        List<int> nextPossibleCols = new List<int>();
        foreach (int nextLilypadLane in level.GetRowLilypadLanes(currentRow + 1)) {
            if (System.Math.Abs(nextLilypadLane - currentCol) <= 1) {
                nextPossibleCols.Add(nextLilypadLane);
            }
        }
        currentRow++;
        currentCol = nextPossibleCols[Random.Range(0, nextPossibleCols.Count)];
        targetPosition = level.GetLilypadOriginWorldCoordinate(currentRow, currentCol);
    }

    private void SelectSprite(float time) {
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
