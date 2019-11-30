using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour {

    public Button startButton;
    public GameObject frog;
    public Vector3[] frogPositions;
    public GameObject bird;
    public Vector3 birdFinalPosition;
    public float frogShockTime;
    public float frogJumpTime;
    public float changeSceneTime;
    
    private bool isAnimationStart;
    private float timer;
    private int jumpIndex;
    
    private Animator frogAnimator;

    public StartScreenMusic startScreenMusic;

    // Start is called before the first frame update
    void Start() {
        isAnimationStart = false;
        timer = 0;
        jumpIndex = 0;

        frogAnimator = frog.GetComponent<Animator>();

        startButton.onClick.AddListener(StartButtonClick);
    }

    // Update is called once per frame
    void Update() {
        if (isAnimationStart) {
            timer += Time.deltaTime;
            bird.transform.position = Vector3.MoveTowards(bird.transform.position, birdFinalPosition, 0.05f);
            if (timer > changeSceneTime) {
                SceneManager.LoadScene("MainScene");
            } else if (timer > frogJumpTime && jumpIndex < frogPositions.Length - 1) {
                float lerpAmount = (timer - frogJumpTime - jumpIndex / 2) * 2;
                frog.transform.position = Vector3.Lerp(frogPositions[jumpIndex], frogPositions[jumpIndex + 1], lerpAmount);
                frogAnimator.SetTrigger("Jump");
                if (lerpAmount > 1) {
                    ++jumpIndex;
                }
            } else if (timer > frogShockTime) {
                frogAnimator.SetTrigger("Shock");
            }
        }
    }

    void StartButtonClick() {
        isAnimationStart = true;
        startButton.gameObject.SetActive(false);
        startScreenMusic.StartFadeOut();
    }
}
