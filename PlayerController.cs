using UnityEngine;
using UnityEngine.AI;

/// Controling player movement based on mouse clicks in the game world.
/// Requiring a NavMeshAgent component on the same GameObject.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How fast the character rotates to face movement direction")]
    [SerializeField] private float rotationSpeed = 15f;
    
    [Header("Visual Feedback")]
    [Tooltip("Prefab to spawn at click location")]
    [SerializeField] private GameObject clickMarkerPrefab;
    [Tooltip("How long click markers remain visible")]
    [SerializeField] private float markerDuration = 1f;
    
    // Reference to the NavMeshAgent component
    private NavMeshAgent navAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        navAgent = GetComponent<NavMeshAgent>();
        
        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing from the player!");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            // If the ray hits something
            if (Physics.Raycast(ray, out hit))
            {
                // Set the agent's destination to the hit point
                navAgent.SetDestination(hit.point);
                
                // Show visual feedback at click location
                ShowClickMarker(hit.point);
            }
        }
        
        // Smoothly rotate the character to face the direction of movement
        if (navAgent.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(navAgent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Creates a visual marker at the click location
    /// </summary>
    /// <param name="position">World position to place the marker</param>
    private void ShowClickMarker(Vector3 position)
    {
        if (clickMarkerPrefab != null)
        {
            // Instantiate the marker prefab
            GameObject marker = Instantiate(clickMarkerPrefab, position, Quaternion.identity);
            
            // Destroy the marker after the specified duration
            Destroy(marker, markerDuration);
        }
    }
}
