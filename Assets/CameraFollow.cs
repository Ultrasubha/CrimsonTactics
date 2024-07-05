using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public string playerTag = "Player"; // Tag assigned to the player prefab
    private Transform player; // Reference to the player's transform
    private Vector3 offset; // Offset value to keep the camera above and at a distance from the player
    private bool playerFound = false;

    void Start()
    {
        // Start the coroutine to find the player
        StartCoroutine(FindPlayer());
    }

    void LateUpdate()
    {
        if (playerFound && player != null)
        {
            // Set the camera's position to the player's position plus the offset
            transform.position = player.position + offset;
        }
    }

    private IEnumerator FindPlayer()
    {
        // Wait until the player is instantiated
        while (!playerFound)
        {
            GameObject playerObject = GameObject.FindWithTag(playerTag);
            if (playerObject != null)
            {
                player = playerObject.transform;
                offset = transform.position - player.position;
                playerFound = true;
            }
            yield return null; // Wait for the next frame and try again
        }
    }
}
