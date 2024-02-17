using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum PlayerState {
    None,
    Idle,
    Running,
    RunningBack,
    Dead
}

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    public static PlayerManager instance;
    PlayerState playerState;

    private void Awake() {
        instance = this;
        playerState = PlayerState.None;
    }
    void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (playerState == PlayerState.Dead) {
            LevelManager.s_instance.changeLevelState(LevelState.GameOver);
        }
    }
    public void ChangePlayerState(PlayerState newState) {
        if (playerState == newState) {

            return;
        }

        resetAnimatorParameters();

        playerState = newState;
        switch (playerState) {
            case PlayerState.None:
                break;
            case PlayerState.Idle:
                Debug.Log("Idle");
                animator.SetBool("isIdle", true);
                break;
            case PlayerState.Running:
                Debug.Log("Running");
                animator.SetBool("isRunning", true);
                break;
            case PlayerState.RunningBack:
                Debug.Log("RunningBack");
                animator.SetBool("isRunningBack", true);
                break;
            case PlayerState.Dead:
                animator.SetBool("isDead", true);
                break;
            default: break;
        }
    }
    private void resetAnimatorParameters() {
        foreach (AnimatorControllerParameter parameter in animator.parameters) {
            if (parameter.type == AnimatorControllerParameterType.Bool) {
                animator.SetBool(parameter.name, false);
            }
        }
    }
    public PlayerState GetState() { return playerState; }

    public Animator getAnimator() { return animator; }

    private bool IsDead() {
        if (PlayerManager.instance.GetState() != PlayerState.Dead) {
            return false;
        }
        StartCoroutine(DestroyPlayer());
        return true;
    }

    IEnumerator DestroyPlayer() {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "SafeZone") {
            Debug.Log("GameFinished");
            GameManager.s_instance.changeGameSate(GameState.GameFinished);
        }
    }
}
