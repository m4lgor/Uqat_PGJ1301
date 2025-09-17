using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings Instance { get; private set; }

    public Plane ScrollingPlane = new Plane(Vector3.forward, Vector3.zero);
    public static Vector3 WorldRight = new Vector3(1.0f, 0.0f, 0.0f); 
    public static Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);

    void Start()
    {
        // Enforce singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Avoid duplicate singletons
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes

        if (Application.isEditor)
        {
            Application.runInBackground = true;
        }
    }
}