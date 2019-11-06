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
                frog.move(1, 0);
            } else if (Input.GetKeyDown(left)) {
                // Ignore user input if he goes out of bounds
                if (frog.currentCol > 0) {
                    frog.move(1, -1);
                }
            } else if (Input.GetKeyDown(right)) {
                // Ignore user input if he goes out of bounds
                if (frog.currentCol < Level.GetSingleton().GetTotalLanes() - 1) {
                    frog.move(1, 1);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
