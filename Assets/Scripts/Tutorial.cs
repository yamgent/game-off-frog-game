using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    // Hardcoded tutorial sequences.
    private readonly int[] TUTORIAL_LANE_SEQUENCE = { 1, 0, 1 };
    private readonly TutorialButtonKey[] TUTORIAL_BUTTON_HINT_SEQUENCE = { 
        TutorialButtonKey.Up, TutorialButtonKey.Left, TutorialButtonKey.Right };

    public enum TutorialButtonKey { Left, Up, Right };

    private static Tutorial singleton;

    private static bool isTutorialEnded = false;
    private int tutorialCurrentStep;

    public Image tutorialButtonsModalImage;
    public Image tutorialLeftButtonImage;
    public Image tutorialUpButtonImage;
    public Image tutorialRightButtonImage;

    void Start() {
        if (singleton != null) {
            Debug.LogError("Multiple Tutorial managers found but should only have one!");
        }
        singleton = this;

        if (isTutorialEnded) {
            Destroy(tutorialButtonsModalImage.gameObject);
            Destroy(tutorialLeftButtonImage.gameObject);
            Destroy(tutorialUpButtonImage.gameObject);
            Destroy(tutorialRightButtonImage.gameObject);
            return;
        }
        
        tutorialCurrentStep = 0;
        ShowNextButtonHint();
    }

    public static Tutorial GetSingleton() {
        return singleton;
    }

    public bool IsInTutorial() {
        return !isTutorialEnded;
    }

    public int[] GetLaneSequence() {
        return TUTORIAL_LANE_SEQUENCE;
    }

    public void IncrementTutorialStep() {
        if (isTutorialEnded) {
            return;
        }
        tutorialCurrentStep++;
        if (tutorialCurrentStep < TUTORIAL_BUTTON_HINT_SEQUENCE.Length) {
            ShowNextButtonHint();
        } else {
            EndTutorial();
        }
    }

    private void ShowNextButtonHint() {
        Image buttonImage = null;
        TutorialButtonKey tutorialButtonKey = TUTORIAL_BUTTON_HINT_SEQUENCE[tutorialCurrentStep];
        switch (tutorialButtonKey) {
            case TutorialButtonKey.Left:
                buttonImage = tutorialLeftButtonImage;
                break;
            case TutorialButtonKey.Up:
                buttonImage = tutorialUpButtonImage;
                break;
            case TutorialButtonKey.Right:
                buttonImage = tutorialRightButtonImage;
                break;
            default:
                Debug.LogError("ShowButtonHint: Unhandled tutorial button key!");
                return;
        }
        tutorialButtonsModalImage.transform.SetAsLastSibling();
        buttonImage.transform.SetAsLastSibling();
    }

    private void EndTutorial() {
        if (isTutorialEnded) {
            return;
        }
        isTutorialEnded = true;
        Destroy(tutorialButtonsModalImage.gameObject);
        Destroy(tutorialLeftButtonImage.gameObject);
        Destroy(tutorialUpButtonImage.gameObject);
        Destroy(tutorialRightButtonImage.gameObject);
        CameraController.GetSingleton().StartCameraMovement();
    }

    // For testing purposes.
    public void ResetTutorial() {
        isTutorialEnded = false;
    }
}
