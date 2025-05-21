using UnityEngine;

/// Sets up the game scene with required components.
/// Attach this to an empty GameObject in the scene.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Player Setup")]
    [Tooltip("Prefab for the player character")]
    [SerializeField] private GameObject playerPrefab;
    [Tooltip("Where to spawn the player")]
    [SerializeField] private Transform playerSpawnPoint;
    
    [Header("Camera Setup")]
    [Tooltip("How high above the player the camera should be")]
    [SerializeField] private float cameraHeight = 10f;
    [Tooltip("How far behind the player the camera should be")]
    [SerializeField] private float cameraDistance = 5f;
    [Tooltip("How quickly the camera follows the player")]
    [SerializeField] private float cameraFollowSpeed = 5f;
    
    // Reference to the player and camera
    private GameObject playerInstance;
    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in the scene!");
            return;
        }
        
        // Create the player
        SpawnPlayer();
    }
    
    // Update is called once per frame
    void Update()
    {
        // Follow the player with the camera
        if (playerInstance != null && mainCamera != null)
        {
            UpdateCameraPosition();
        }
    }
    
    /// <summary>
    /// Spawns the player character in the scene
    /// </summary>
    private void SpawnPlayer()
    {
        // If no player prefab is assigned, create a simple cylinder
        if (playerPrefab == null)
        {
            Debug.Log("No player prefab assigned. Creating a cylinder instead.");
            
            playerInstance = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            playerInstance.name = "Player";
            
            // Add NavMeshAgent component
            playerInstance.AddComponent<UnityEngine.AI.NavMeshAgent>();
            
            // Add the player controller script
            playerInstance.AddComponent<PlayerController>();
        }
        else
        {
            // Instantiate the player prefab
            playerInstance = Instantiate(playerPrefab);
        }
        
        // Position the player at the spawn point
        if (playerSpawnPoint != null)
        {
            playerInstance.transform.position = playerSpawnPoint.position;
        }
        else
        {
            playerInstance.transform.position = Vector3.zero;
        }
    }
    
    /// <summary>
    /// Updates the camera position to follow the player
    /// </summary>
    private void UpdateCameraPosition()
    {
        // Calculate the desired camera position
        Vector3 targetPosition = playerInstance.transform.position;
        targetPosition.y += cameraHeight;
        targetPosition.z -= cameraDistance;
        
        // Smoothly move the camera towards the target position
        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position, 
            targetPosition, 
            cameraFollowSpeed * Time.deltaTime
        );
        
        // Make the camera look at the player
        mainCamera.transform.LookAt(playerInstance.transform);
    }
}
