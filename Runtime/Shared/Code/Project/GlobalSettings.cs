using Unity.Mathematics;
using UnityEngine;

[DefaultExecutionOrder(-1000)] // s’exécute très tôt
public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings Instance { get; private set; }

    public Plane ScrollingPlane = new Plane(Vector3.forward, Vector3.zero);
    public Vector3 WorldRight = new Vector3(1.0f, 0.0f, 0.0f); 
    public Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);
    public Vector3 WorldDepth = new Vector3(0.0f, 0.0f, 1.0f);

    [SerializeField] GameObject _ArenaBorderPrefab;

    [SerializeField] public bool EnableGravity = true;
    [SerializeField] public Vector2 ArenaSize = new Vector2(20, 20);

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Avoid duplicate singletons
            return;
        }

        Instance = this;

        if (Application.isEditor)
        {
            Application.runInBackground = true;
        }
        // Singleton setup done

        // Scene Initialization
        if (!EnableGravity)
        {
            Physics.gravity = Vector3.zero;
        }

        if(_ArenaBorderPrefab != null)
        {
            // Top Border
            var topBorder = Instantiate(_ArenaBorderPrefab, new Vector3(0.0f, ArenaSize.y * 0.5f, 0.0f), Quaternion.identity);
            topBorder.transform.localScale = new Vector3(ArenaSize.x, 1.0f, 1.0f);

            // Bottom Border
            var bottomBorder = Instantiate(_ArenaBorderPrefab, new Vector3(0.0f, -ArenaSize.y * 0.5f, 0.0f), Quaternion.identity);
            bottomBorder.transform.localScale = new Vector3(ArenaSize.x, 1.0f, 1.0f);

            // Left Border
            var leftBorder = Instantiate(_ArenaBorderPrefab, new Vector3(-ArenaSize.x * 0.5f, 0.0f, 0.0f), Quaternion.identity);
            leftBorder.transform.localScale = new Vector3(1.0f, ArenaSize.y, 1.0f);

            // Right Border
            var rightBorder = Instantiate(_ArenaBorderPrefab, new Vector3(ArenaSize.x * 0.5f, 0.0f, 0.0f), Quaternion.identity);
            rightBorder.transform.localScale = new Vector3(1.0f, ArenaSize.y, 1.0f);
        }
        else
        {
            Debug.LogWarning("GlobalSettings - No Arena Border Prefab assigned. Skipping arena border creation.");
        }
    }
}