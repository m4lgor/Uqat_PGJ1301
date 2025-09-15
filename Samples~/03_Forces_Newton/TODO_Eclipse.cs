using UnityEngine;

public class TODO_Eclipse : MonoBehaviour
{
    [SerializeField] Transform LightSource;
    [SerializeField] Transform ShadowSource;

    private Color _InitColor;
    private Color _EclipseColor = Color.black;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store the initial color to allow blending later on
        var rendererComponent = GetComponent<Renderer>();
        if (rendererComponent)
        {
            _InitColor = rendererComponent.material.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ----------- TODO Students ------------
        // Change the color to blend between _InitColor and _EclipseColor.
        // Use the position of both the LightSource and the ShadowSource to determine the blend value.
        // Simple blend between 0 and 1, no need for real shadowing here :) 
        // Use Color.Lerp is recommended


        // --------------------------------------
    }
}
