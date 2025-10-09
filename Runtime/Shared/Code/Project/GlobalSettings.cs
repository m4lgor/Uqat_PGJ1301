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

    /// <summary>
    /// Arena
    /// </summary>
    [SerializeField, Min(3)] private int _ArenaSides = 4; 
    [SerializeField] private float BorderThickness = 1f;

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
            BuildArena();
        }
        else
        {
            Debug.LogWarning("GlobalSettings - No Arena Border Prefab assigned. Skipping arena border creation.");
        }
    }

    void BuildArena()
    {
        if (!_ArenaBorderPrefab) return;

        int n = Mathf.Max(3, _ArenaSides);

        if (n == 4)
        {
            BuildRectangle();
            return;
        }

        // -------- Regular N-gon (inscribed in circle) --------
        float R = 0.5f * Mathf.Min(ArenaSize.x, ArenaSize.y);   // circumradius (fits inside the given rect)
        float a = R * Mathf.Cos(Mathf.PI / n);                  // apothem (center -> side center)
        float L = 2f * R * Mathf.Sin(Mathf.PI / n);             // side length

        for (int i = 0; i < n; i++)
        {
            float ang = i * Mathf.PI * 2f / n;

            // Outward normal of side i in 2D
            Vector2 normal2 = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));

            // Center of side i is apothem units along the normal
            Vector3 pos = new Vector3(normal2.x * a, normal2.y * a, 0f);

            // Rotate so local +X aligns with the side's tangent (perpendicular to normal)
            // Tangent angle is ang + 90°
            float rotDeg = ang * Mathf.Rad2Deg + 90f;
            Quaternion rot = Quaternion.Euler(0f, 0f, rotDeg); // rotate around Z for XY plane

            var t = Instantiate(_ArenaBorderPrefab, pos, rot, transform);

            // Scale: length along local X; thickness along the axis normal to the side within the plane
            var s = t.transform.localScale;
            s.x = L;
            // In XY, the inward/outward thickness axis is local Y
            s.y = BorderThickness;

            t.transform.localScale = s;
            t.name = $"Border_{i}";
        }
    }
    private void BuildRectangle()
    {
        // Exactly replicates your original 4-border behavior (XY version),
        // and adapts properly if you're on the XZ plane.
        // XY plane (2D top-down)
        var top = Instantiate(_ArenaBorderPrefab, new Vector3(0f, +ArenaSize.y * 0.5f, 0f), Quaternion.identity, transform);
        var bottom = Instantiate(_ArenaBorderPrefab, new Vector3(0f, -ArenaSize.y * 0.5f, 0f), Quaternion.identity, transform);
        var left = Instantiate(_ArenaBorderPrefab, new Vector3(-ArenaSize.x * 0.5f, 0f, 0f), Quaternion.identity, transform);
        var right = Instantiate(_ArenaBorderPrefab, new Vector3(+ArenaSize.x * 0.5f, 0f, 0f), Quaternion.identity, transform);

        top.transform.localScale = new Vector3(ArenaSize.x, BorderThickness, top.transform.localScale.z);
        bottom.transform.localScale = new Vector3(ArenaSize.x, BorderThickness, bottom.transform.localScale.z);
        left.transform.localScale = new Vector3(BorderThickness, ArenaSize.y, left.transform.localScale.z);
        right.transform.localScale = new Vector3(BorderThickness, ArenaSize.y, right.transform.localScale.z);

        top.name = "Border_Top"; bottom.name = "Border_Bottom";
        left.name = "Border_Left"; right.name = "Border_Right";
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