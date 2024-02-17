using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    #region Private
    Animator animator;
    Rigidbody rgbd;
    float verticalInput;
    float horizontalInput;
    #endregion


    #region Serialized Field
    [SerializeField] float speed, rotationSpeed;

    #endregion

    private void Start() {
       // animator = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {

        if (PlayerManager.instance.getPlayerCanMove()) {
            moveAround();
            return;
        }
        rgbd.velocity = Vector2.zero;
    }

    void moveAround() {

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //PlayerManager.instance.getAnimator().SetFloat("xMove", horizontalInput);
        //PlayerManager.instance.getAnimator().SetFloat("yMove", verticalInput);

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);

        if (movement != Vector3.zero) {
            // Normalize the movement vector to ensure consistent speed regardless of direction
            movement.Normalize();

            // Convert local movement vector to world space
            Vector3 moveInWorld = transform.TransformDirection(movement);

            // Calculate velocity based on speed
            Vector3 velocity = moveInWorld * speed;
            rgbd.velocity = velocity;

            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveInWorld, Vector3.up);

            if(verticalInput < 0) {
                PlayerManager.instance.ChangePlayerState(PlayerState.RunningBack);
                if (verticalInput < 0) {
                    targetRotation *= Quaternion.Euler(0, 180, 0);
                }
                // Smoothly rotate towards the target rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                return;
            }
            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Change player state to running
            PlayerManager.instance.ChangePlayerState(PlayerState.Running);
        } else {
            // If there's no movement input, change player state to idle
            PlayerManager.instance.ChangePlayerState(PlayerState.Idle);
        }

    }

}
