using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float distance = 5f; // Distance from the player
    public float height = 3f; // Height above the player
    public float rotationDamping = 5f; // Damping for camera rotation

    private void LateUpdate() {
        // Calculate the desired position of the camera based on player's position and distance
        Vector3 desiredPosition = player.position - player.forward * distance + Vector3.up * height;

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * rotationDamping);

        // Rotate the camera to match the player's rotation
        Quaternion desiredRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationDamping);
    }
}
