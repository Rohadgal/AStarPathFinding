using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
public enum EnemyState {
    None,
    Idle,
    Chasing,
    busy
}


public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    bool isChasing, hasCaughtPlayer, hasReachedTarget;
    [SerializeField]
    GameGraph gameGraph;
    [SerializeField]
    GameObject playerTarget;
    EnemyState enemyState;
    Animator animator;

    public float moveSpeed = 5f, rotationSpeed = 3f; // Speed at which the enemy moves
    public float arrivalThreshold, catchPlayerThreshold = 0.1f; // Distance threshold to consider the enemy has reached a node

    List<GameGraph.Node> starPath;// = gameGraph.starSearchNodes;
    private int currentNodeIndex = 0; // Index of the current node the enemy is moving towards


    private void Start() {
        animator = GetComponent<Animator>();
        if (isChasing) {
            starPath = gameGraph.starSearchNodes;
        }
        // Start moving towards the first node
        MoveToNode(starPath[currentNodeIndex]);
        ChangeEnemyState(EnemyState.Chasing);
    }

    private void Update() {

        if (Vector3.Distance(playerTarget.transform.position, transform.position) <= catchPlayerThreshold){
            Debug.LogWarning("has caught player");
            hasCaughtPlayer = true;
        }

        //transform.LookAt(starPath[currentNodeIndex].gameObject.transform.position);

        if (isChasing) {
            enemyState = EnemyState.Chasing;
            MoveToNode(starPath[currentNodeIndex]);
            // If the enemy has reached the current node, move to the next node
            if (Vector3.Distance(transform.position, starPath[currentNodeIndex].position) <= arrivalThreshold) {
                currentNodeIndex++;
                // If there are no more nodes, the enemy has reached the end of the path
                if (currentNodeIndex >= starPath.Count) {
                    Debug.Log("Enemy reached the last node.");
                    ChangeEnemyState(EnemyState.Idle);
                    //enabled = false; // Disable this script
                    if (hasCaughtPlayer) {
                        enabled = false;
                        return;
                    }
                    currentNodeIndex = 0;
                    starPath = gameGraph.starSearchNodes;
                    MoveToNode(starPath[currentNodeIndex]);
                    ChangeEnemyState(EnemyState.Chasing);
                }

                // Move to the next node
                MoveToNode(starPath[currentNodeIndex]);
            }
            
        }

        
    }

    private void MoveToNode(GameGraph.Node node) {
        node.position.y = transform.position.y;
        // Calculate direction to the node
        Vector3 direction = (node.position - transform.position).normalized;

        // Rotate the enemy towards the next node's position
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Move towards the node
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    //private void OnDrawGizmos() {
    //    Gizmos.DrawLine(transform.position, new Vector3(starPath[currentNodeIndex].gameObject.transform.position.x - transform.position.x,
    //        starPath[currentNodeIndex].gameObject.transform.position.y - transform.position.y,
    //    starPath[currentNodeIndex].gameObject.transform.position.z - transform.position.z).normalized);
    //}

    public void ChangeEnemyState(EnemyState newState) {
        if (enemyState == newState) {

            return;
        }

        resetAnimatorParameters();

        enemyState = newState;
        switch (enemyState) {
            case EnemyState.None:
                break;
            case EnemyState.Idle:
                Debug.Log("Idle");
                animator.SetBool("isIdle", true);
                break;
            case EnemyState.Chasing:
                Debug.Log("Running");
                animator.SetBool("isChasing", true);
                break;
            case EnemyState.busy:
                Debug.Log("isBusy");
                animator.SetBool("isBusy", true);
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
}
