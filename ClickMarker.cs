using UnityEngine;

/// Creates a simple marker at the click location to provide visual feedback.
/// This marker will fade out over time.
/// </summary>
public class ClickMarker : MonoBehaviour
{
    [Tooltip("How quickly the marker fades out")]
    [SerializeField] private float fadeSpeed = 1.0f;
    
    // References to components
    private Renderer markerRenderer;
    private Material markerMaterial;
    private Color originalColor;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get the renderer component
        markerRenderer = GetComponent<Renderer>();
        
        if (markerRenderer != null)
        {
            // Create a material instance to avoid affecting other objects using the same material
            markerMaterial = markerRenderer.material;
            originalColor = markerMaterial.color;
        }
        else
        {
            Debug.LogWarning("Renderer component is missing from click marker!");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (markerMaterial != null)
        {
            // Get the current color
            Color currentColor = markerMaterial.color;
            
            // Reduce alpha value over time
            float newAlpha = currentColor.a - (fadeSpeed * Time.deltaTime);
            
            // Set the new color with reduced alpha
            markerMaterial.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            
            // Destroy when fully transparent
            if (newAlpha <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
