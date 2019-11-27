using UnityEngine.SceneManagement;
using UnityEngine;

public class InputListener : MonoBehaviour {

    public Frog frog;
    public KeyCode up;
    public KeyCode left;
    public KeyCode right;

    void Update() {
        if (!frog.isMoving && !frog.isDead) {
            if (Input.GetKeyDown(up)) {
                TryMoveFrog(1, 0);
            } else if (Input.GetKeyDown(left)) {
                TryMoveFrog(1, -1);
            } else if (Input.GetKeyDown(right)) {
                TryMoveFrog(1, 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            Tutorial.GetSingleton().ResetTutorial();
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            Tutorial.GetSingleton().ResetTutorial();
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    private void TryMoveFrog(int rowBy, int colBy) {
        if (Tutorial.GetSingleton().IsTutorialEnded()) {
            // Cannot move before tutorial starts
            if (!Tutorial.GetSingleton().IsTutorialStarted()) {
                return;
            }

            // Cannot make failure move while in tutorial.
            if (!Level.GetSingleton().HasLilypadAt(frog.currentRow + rowBy, frog.currentCol + colBy)) {
                return;
            }
            frog.Move(rowBy, colBy);
            Tutorial.GetSingleton().IncrementTutorialStep();
            return;
        }

        // Frog can only move within the lane bounds.
        if (Level.GetSingleton().IsWithinLaneBounds(frog.currentCol + colBy)) {
            frog.Move(rowBy, colBy);
        }
    }
}
