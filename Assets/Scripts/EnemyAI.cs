using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GridManagerScript gridManager; // Reference to the grid manager script
    private PlayerManager playerManager;  // Reference to the player manager script

    private bool isMoving = false;        // Flag to check if the enemy is moving
    private List<Vector3> path;           // Path the enemy will follow

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>(); // Find the PlayerManager in the scene
    }

    public void MoveTowardsPlayer(Vector3 playerPosition)
    {
        if (!isMoving)
        {
            // Get the player's current tile position
            Vector3 playerTilePosition = new Vector3(Mathf.Round(playerPosition.x), 0.5f, Mathf.Round(playerPosition.z));

            // Find the best adjacent tile to move to
            Vector3 bestTile = GetBestAdjacentTile(playerTilePosition);
            
            if (bestTile != Vector3.zero) // If a valid adjacent tile is found
            {
                // Calculate the path to the selected adjacent tile
                path = Pathfinding.FindPath(transform.position, bestTile, gridManager);
                isMoving = true; // Set the flag to indicate the enemy is moving
                playerManager.StartEnemyMovement(); // Start the walking animation
                StartCoroutine(MoveEnemyAlongPath());
            }
        }
    }

    private Vector3 GetBestAdjacentTile(Vector3 playerTilePosition)
    {
        // Define possible adjacent positions (up, down, left, right)
        Vector3[] adjacentOffsets = {
            new Vector3(1, 0, 0),  // Right
            new Vector3(-1, 0, 0), // Left
            new Vector3(0, 0, 1),  // Up
            new Vector3(0, 0, -1)  // Down
        };

        Vector3 bestTile = Vector3.zero;
        float closestDistance = float.MaxValue;

        foreach (var offset in adjacentOffsets)
        {
            Vector3 adjacentTile = playerTilePosition + offset;
            if (!gridManager.IsTileBlocked((int)adjacentTile.x, (int)adjacentTile.z))
            {
                // Calculate distance from the enemy to this adjacent tile
                float distance = Vector3.Distance(transform.position, adjacentTile);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    bestTile = adjacentTile;
                }
            }
        }

        return bestTile;
    }

    private IEnumerator MoveEnemyAlongPath()
    {
        // Move the enemy along the path
        while (path != null && path.Count > 0)
        {
            Vector3 targetPosition = path[0];
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2);
                yield return null;
            }
            path.RemoveAt(0);
        }

        // Stop the enemy movement
        isMoving = false;
        playerManager.StopEnemyMovement(); // Stop the walking animation
    }
}
