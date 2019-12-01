using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour {
    private static PauseButton singleton;
    
    public GameObject pauseMenu;
    public Button restartButton;
    public Button homeButton;
    public Button resumeButton;
    public Text gameOverText;

    private Button pauseButton;
    
    void Awake() {
        if (singleton != null) {
            Debug.LogError("Multiple Level managers found but should only have one!");
        }
        singleton = this;
    }

    void Start() {
        pauseButton = GetComponent<Button>();
        pauseButton.onClick.AddListener(PauseButtonClick);
        restartButton.onClick.AddListener(RestartButtonClick);
        homeButton.onClick.AddListener(HomeButtonClick);
        resumeButton.onClick.AddListener(ResumeButtonClick);
    }

    public static PauseButton GetSingleton() {
        return singleton;
    }

    void PauseButtonClick() {
        Pause(false);
    }

    void RestartButtonClick() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Tutorial.GetSingleton().ResetTutorial();
        Time.timeScale = 1;
    }

    void HomeButtonClick() {
        SceneManager.LoadScene("MenuScene");
        Tutorial.GetSingleton().ResetTutorial();
        Time.timeScale = 1;
    }

    void ResumeButtonClick() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Pause(bool isGameOver) {
        if (isGameOver) {
            resumeButton.gameObject.SetActive(false);
            gameOverText.text = "Game Over!";
        } else {
            resumeButton.gameObject.SetActive(true);
            gameOverText.text = "";
        }

        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
