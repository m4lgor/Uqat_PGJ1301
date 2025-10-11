using UnityEngine;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

public interface ISensor 
{
    /// Effectue la mesure (raycast / overlap / sweep) avec un contexte fourni par la base.
    SensorOutput Sense(in SensorInput ctx);

    public class SensorInput
    {
        public Vector3 origin = Vector3.zero;           // Position to start the query
        public Vector3 direction = Vector3.zero;          // Direction for the query (normalized)
        public float maxDistance = 1.0f;      // Max Range for the query
        public LayerMask mask = LayerMask.GetMask("Default");           // Layers
    }

    /// <summary>
    /// This class holds the results of a sensing operation.
    /// It can store multiple hit points, normals, distances, colliders, and rigidbodies.
    /// Use RaycastAll or similar methods to populate these lists.
    /// </summary>
    public class SensorOutput
    {
        public bool hasHit = false;
         
        public List<Vector3> points = new List<Vector3>();
        public List<Vector3> normals = new List<Vector3>(); 
        public List<float> distances = new List<float>();
        public List<Collider> colliders = new List<Collider>();
        public List<Rigidbody> rigidbodies = new List<Rigidbody>();

        public void Cleanup()
        {
            points.Clear();
            normals.Clear();
            distances.Clear();
            colliders.Clear();
            rigidbodies.Clear();
        }
    }
}
