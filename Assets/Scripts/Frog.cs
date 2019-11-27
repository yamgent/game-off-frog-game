using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    // Let frog's jump time as a function of speed be f(spd) = 1/(spd^2).
    public float speed;

    private float jumpTime;
    private float timer;
    private Vector2 startPosition;
    private Vector2 targetPosition;

    public bool isMoving { get; private set; }
    public bool isDead { get; private set; }
    public int currentRow { get; private set; }
    public int currentCol { get; private set; }

    private Animator animator;

    public AudioSource jumpSound;
    public AudioSource deathSound;
    private bool deathAnimationPlayed;

    private const float JUMP_SOUND_MINIMUM_PITCH = 0.5f;
    private const float JUMP_SOUND_MAXIMUM_PITCH = 2.0f;

    private Environment.BiomeType frogCurrentBiome = Environment.BiomeType.Water;

    // Start is called before the first frame update
    void Start() {
        isMoving = false;
        isDead = false;
        timer = 0;
        startPosition = transform.position;
        CalculateNewJumpTime();

        currentRow = 0;
        currentCol = 2;

        animator = GetComponent<Animator>();

        deathAnimationPlayed = false;

        jumpSound.pitch = JUMP_SOUND_MINIMUM_PITCH;
    }

    // Update is called once per frame
    void Update() {
        if (isMoving) {
            if (timer > jumpTime) {
                isMoving = false;
                timer = 0;
                startPosition = targetPosition;
            } else {
                timer += Time.deltaTime;
                float time = Mathf.Min(1, timer / jumpTime);
                transform.position = Vector2.Lerp(startPosition, targetPosition, time);
                SelectSprite(time);
            }
        } else if (isDead) {
            if (!deathAnimationPlayed) {
                deathAnimationPlayed = true;

                animator.SetTrigger("Die");
                CameraController.GetSingleton().StopCameraMovement();
                deathSound.Play();
            }
        }
    }

    public void Move(int rowBy, int colBy) {
        currentRow += rowBy;
        currentCol += colBy;

        jumpSound.Play();

        if (Level.GetSingleton().HasLilypadAt(currentRow, currentCol)) {
            Score.GetSingleton().AddToCurrentScore(rowBy);
        } else {
            Die();
        }
        targetPosition = Level.GetSingleton().GetLilypadOriginWorldCoordinate(currentRow, currentCol);
        isMoving = true;
        Level.GetSingleton().UpdateFrogPosition(currentRow);

        Environment.BiomeType frogNewBiome = Environment.GetSingleton().GetBiomeAt(currentRow);
        if (frogNewBiome != frogCurrentBiome) {
            MusicManager.GetSingleton().ChangeBiomeMusic(frogNewBiome);
        }
        frogCurrentBiome = frogNewBiome;
    }

    public void Die() {
        isDead = true;
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

    public void CalculateNewJumpTime() {
        float jumpingAnimationRatio = Mathf.Min(1, timer / jumpTime);
        jumpTime = 1.0f / (speed * speed);
        if (jumpingAnimationRatio < 1.0f) {
            timer = jumpingAnimationRatio * jumpTime;
        }
    }

    public void CalculateNewJumpSoundPitch() {
        float pitchRatio = Mathf.Min(1.0f, speed - 1.0f);

        jumpSound.pitch = Mathf.Lerp(JUMP_SOUND_MINIMUM_PITCH, 
            JUMP_SOUND_MAXIMUM_PITCH, pitchRatio);
    }
}
