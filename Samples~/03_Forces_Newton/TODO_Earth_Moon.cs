using UnityEngine;

public class TODO_Earth_Moon : MonoBehaviour
{
    [SerializeField] Transform Target;

    [SerializeField] float RotateAroundSpeed = 45.0f;
    [SerializeField] float RotateSpeed = 0.0f;
    [SerializeField] bool LookAtTarget = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTransform();
    }

    void UpdateTransform()
    {
        // ----------- TODO Students ------------
        // Make the object rotate around the Target
        // Allow the object to always look at the Target if LookAtTarget is true


        // --------------------------------------
    }
}
