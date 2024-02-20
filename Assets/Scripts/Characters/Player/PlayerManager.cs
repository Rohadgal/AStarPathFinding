using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum PlayerState {
    None,
    Idle,
    Running,
    RunningBack,
    Caught,
    Safe
}

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    public static PlayerManager instance;
    PlayerState playerState;
    bool playerCanMove;
    Vector3 startPos = Vector3.zero;
    Quaternion rotPos;
    //Transform startPos;

   // Vector3 playerPos;
    

    private void Awake() {
        instance = this;
        playerState = PlayerState.None;
    }
    void Start() {
        startPos.y = transform.position.y;
        animator = GetComponent<Animator>();
        rotPos = transform.transform.rotation;
       // playerPos = transform.position;
        //startPos = transform.position;
    }

    private void Update() {

  

        if(LevelManager.s_instance.GetLevelState() == LevelState.Playing) {
            playerCanMove = true;
        }

        if (playerState == PlayerState.Caught) {
            ChangePlayerState(PlayerState.None);
            //LevelManager.s_instance.changeLevelState(LevelState.GameOver);
        }

        if(playerState == PlayerState.Safe) {   
            ChangePlayerState(PlayerState.None);
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
                //Debug.Log("Idle");
                animator.SetBool("isIdle", true);
                break;
            case PlayerState.Running:
                //Debug.Log("Running");
                animator.SetBool("isRunning", true);
                break;
            case PlayerState.RunningBack:
                //Debug.Log("RunningBack");
                animator.SetBool("isRunningBack", true);
                break;
            case PlayerState.Caught:
                transform.position = startPos;
                transform.rotation = rotPos;
                playerCanMove = false;
                //animator.SetBool("isDead", true);
                break;
            case PlayerState.Safe:
                transform.position = startPos;
                transform.rotation = rotPos;
                playerCanMove = false;
                //animator.SetBool("isDead", true);
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
        if (PlayerManager.instance.GetState() != PlayerState.Caught) {
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
            //Debug.Log("GameFinished");
            ChangePlayerState(PlayerState.Safe);
            LevelManager.s_instance.changeLevelState(LevelState.LevelFinished);
        }
    }

    public bool getPlayerCanMove() {  return playerCanMove; }
}
