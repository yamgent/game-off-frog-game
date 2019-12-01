using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour {

    public GameObject pauseMenu;
    public Button restartButton;
    public Button homeButton;
    public Button resumeButton;

    private Button pauseButton;

    // Start is called before the first frame update
    void Start() {
        pauseButton = GetComponent<Button>();
        pauseButton.onClick.AddListener(PauseButtonClick);
        restartButton.onClick.AddListener(RestartButtonClick);
        homeButton.onClick.AddListener(HomeButtonClick);
        resumeButton.onClick.AddListener(ResumeButtonClick);
    }

    void PauseButtonClick() {
        Debug.Log("Pause");
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    void RestartButtonClick() {
        Debug.Log("Restart");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Tutorial.GetSingleton().ResetTutorial();
        Time.timeScale = 1;
    }

    void HomeButtonClick() {
        Debug.Log("Home");
        SceneManager.LoadScene("MenuScene");
        Tutorial.GetSingleton().ResetTutorial();
        Time.timeScale = 1;
    }
    void ResumeButtonClick() {
        Debug.Log("Resume");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
