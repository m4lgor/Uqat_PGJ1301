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

    /// <summary>
    /// Display console with GlobalSettings
    /// </summary>
    [SerializeField] bool _DisplayGUI = true;

    /// <summary>
    /// Which prefab to use for the arena borders (should have a BoxCollider)
    /// </summary>
    [SerializeField] GameObject _ArenaBorderPrefab;
    [SerializeField] public Vector2 ArenaSize = new Vector2(20, 20);

    /// <summary>
    /// Rules
    /// </summary>
    [SerializeField] bool EnableGravity = false;
    [SerializeField] bool RandomizeSpaceshipZRotation = false;
    [SerializeField] bool UnlockSpaceshipZRotation = false;

    GUIStyle _Panel, _Header, _Item;

    private void Awake()
    {
        // ------------- Singleton Pattern -------------
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

        // ------------- Singleton Pattern -------------

        // ------------- Spaceship Setup -------------
        var spaceshipComponents = FindObjectsByType<SpaceshipControllerBase>(FindObjectsSortMode.None);
        if (spaceshipComponents.Length == 0)
        {
            Debug.LogError("No spaceship controller found in the scene. Please add one that inherits from SpaceshipControllerBase.");
        }
        else if (spaceshipComponents.Length > 1)
        {
            Debug.LogWarning("Multiple spaceship controllers found in the scene. Using the first one found.");
        }
        else
        {
            var spaceship = spaceshipComponents[0];

            if(RandomizeSpaceshipZRotation)
            {
                // Get current rotation
                Vector3 currentEuler = spaceship.transform.eulerAngles;

                // Replace only Z with a random value between 0 and 360
                currentEuler.z = UnityEngine.Random.Range(0f, 360f);

                // Apply new rotation
                spaceship.transform.rotation = Quaternion.Euler(currentEuler);
            }

            var rb = spaceship.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("The spaceship does not have a Rigidbody component.");
            }
            else
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

                // Lock Z rotation if needed
                if (!UnlockSpaceshipZRotation)
                {
                    rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
                }
            }
        }
        // ------------- Spaceship Setup -------------


        // ------------- Scene Initialization -------------

        // Enable/Disable gravity
        if (!EnableGravity)
        {
            Physics.gravity = Vector3.zero;
        }

        // Setup arena borders
        if (_ArenaBorderPrefab != null)
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

    void EnsureGUIStyles()
    {
        if (_Panel != null) return;

        _Panel = new GUIStyle(GUI.skin.box)
        {
            padding = new RectOffset(10, 10, 10, 10)
        };

        _Header = new GUIStyle(GUI.skin.label)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 14
        };

        _Item = new GUIStyle(GUI.skin.label)
        {
            richText = true,
            wordWrap = true
        };
    }

    void OnGUI()
    {
        if (!_DisplayGUI)
        {
            return;
        }

        EnsureGUIStyles();

        var items = new System.Collections.Generic.List<string>();
        if (EnableGravity) 
            items.Add("Gravity is <b>ON</b>");
        if (RandomizeSpaceshipZRotation) 
            items.Add("Spaceship Z rotation is <b>Randomized</b>");
        if (UnlockSpaceshipZRotation) 
            items.Add("Spaceship Z rotation is <b>Unlocked</b>");


        // Big enough height so content never clips; GUILayout handles actual sizing
        const float x = 10f, y = 10f, width = 320f;
        var rect = new Rect(x, y, width, 35 + 30 * items.Count); // TODO : Hardcoded for now

        GUILayout.BeginArea(rect, _Panel);
        GUILayout.Label("Rules", _Header);
        foreach (var s in items)
            GUILayout.Label("• " + s, _Item);
        GUILayout.EndArea();
    }
}