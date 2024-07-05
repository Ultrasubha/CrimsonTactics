using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab of the player
    public GameObject enemyPrefab;  // Prefab of the enemy
    public GridManagerScript gridManager; // Reference to the grid manager script

    private GameObject player;       // Instance of the player
    private GameObject enemy;        // Instance of the enemy
    private EnemyAI enemyAI;         // Reference to the enemy AI script
    private bool isMoving = false;   // Flag to check if the player is moving
    private List<Vector3> path;      // Path the player will follow
    private Animator playerAnimator; // Reference to the player's Animator component
    private Animator enemyAnimator;  // Reference to the enemy's Animator component

    void Start()
    {
        // Initialize the player at the starting position
        Vector3 playerStartPos = new Vector3(0, 0.5f, 0); 
        player = Instantiate(playerPrefab, playerStartPos, Quaternion.identity);
        playerAnimator = player.GetComponent<Animator>(); // Get the Animator component from the player

        // Initialize the enemy at the starting position
        Vector3 enemyStartPos = new Vector3(5, 0.5f, 5); 
        enemy = Instantiate(enemyPrefab, enemyStartPos, Quaternion.identity);
        enemyAI = enemy.GetComponent<EnemyAI>(); // Get the EnemyAI component from the enemy
        enemyAI.gridManager = gridManager; // Assign the gridManager to the enemyAI
        enemyAnimator = enemy.GetComponent<Animator>(); // Get the Animator component from the enemy

        // Ensure the enemy uses the same Animator Controller as the player
        if (playerAnimator != null && enemyAnimator != null)
        {
            enemyAnimator.runtimeAnimatorController = playerAnimator.runtimeAnimatorController;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MovePlayerAlongPath();
        }
        else
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform a raycast to check what the mouse is pointing at
            if (Physics.Raycast(ray, out hit))
            {
                TileInfo tileInfo = hit.transform.GetComponent<TileInfo>();
                if (tileInfo != null && !gridManager.IsTileBlocked(tileInfo.x, tileInfo.y))
                {
                    // If the tile is not blocked, calculate the path to the destination
                    Vector3 destination = new Vector3(tileInfo.x, 0.5f, tileInfo.y);
                    path = Pathfinding.FindPath(player.transform.position, destination, gridManager);
                    isMoving = true; // Set the flag to indicate the player is moving
                    playerAnimator.SetBool("isWalking", true); // Start the walking animation
                }
            }
        }
    }

    private void MovePlayerAlongPath()
    {
        // Check if the path is not null and has points
        if (path != null && path.Count > 0)
        {
            // Get the next target position from the path
            Vector3 targetPosition = path[0];
            // Move the player towards the target position
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, Time.deltaTime * 2);

            // Check if the player has reached the target position
            if (player.transform.position == targetPosition)
            {
                path.RemoveAt(0); // Remove the reached position from the path
            }

            // If the path is empty, stop moving and trigger the enemy's movement
            if (path.Count == 0)
            {
                isMoving = false;
                playerAnimator.SetBool("isWalking", false); // Stop the walking animation
                enemyAI.MoveTowardsPlayer(player.transform.position);
            }
        }
        else
        {
            // If the path is null or empty, stop moving and trigger the enemy's movement
            isMoving = false;
            playerAnimator.SetBool("isWalking", false); // Stop the walking animation
            enemyAI.MoveTowardsPlayer(player.transform.position);
        }
    }

    // Public method to start enemy movement
    public void StartEnemyMovement()
    {
        enemyAnimator.SetBool("isWalking", true); // Start the enemy walking animation
    }

    // Public method to stop enemy movement
    public void StopEnemyMovement()
    {
        enemyAnimator.SetBool("isWalking", false); // Stop the enemy walking animation
    }
}
