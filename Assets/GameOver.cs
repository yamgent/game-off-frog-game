using UnityEngine;

public class GameOver : StateMachineBehaviour {
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        PauseButton.GetSingleton().Pause(true);
    }
}
