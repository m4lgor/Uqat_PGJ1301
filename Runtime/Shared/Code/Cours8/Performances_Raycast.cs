 using System;
 using System.Collections.Generic;
 using Unity.Collections;
 using UnityEngine;

public class Performances_Raycast : MonoBehaviour
{
    [SerializeField] private int _RaycastCount = 100;
    [SerializeField] private UpdateType _UpdateType = UpdateType.Update;
    [SerializeField] float _MaxDistance = 100f;

    private List<Vector3> _Origins = new List<Vector3>();
    private List<Vector3> _Directions = new List<Vector3>();

    enum UpdateType
    {
        Update,
        Multithread,
        FixedUpdate,
        LateUpdate,
    }
    
    // Update is called once per frame
    void Update()
    {
        if(_UpdateType == UpdateType.Update)
        {
            QueryRaycasts();
        }
        else if (_UpdateType == UpdateType.Multithread)
        {
            var cmds = new NativeArray<RaycastCommand>(_RaycastCount, Allocator.TempJob);
            var hits = new NativeArray<RaycastHit>(_RaycastCount, Allocator.TempJob);

            _Origins.Clear();
            _Directions.Clear();
            
            float angle = 0.0f;
            for (int i = 0; i < _RaycastCount; i++)
            {
                _Origins.Add(transform.position);
                _Directions.Add(Quaternion.AngleAxis(angle, Vector3.forward) * transform.right);
                angle += 360.0f/_RaycastCount;
                
                cmds[i] = new RaycastCommand(_Origins[i], _Directions[i], _MaxDistance);
            }

            var handle = RaycastCommand.ScheduleBatch(cmds, hits, 64);
            handle.Complete();
            
            for (int i = 0; i < _Origins.Count; ++i)
            {
                var o = _Origins[i];
                var d = _Directions[i] * _MaxDistance;

                if (hits[i].collider)
                    Debug.DrawLine(o, hits[i].point, Color.green, 0f, false);
                else
                    Debug.DrawLine(o, o + d, Color.red, 0f, false);
            }

            cmds.Dispose();
            hits.Dispose();
        }
    }

    void FixedUpdate()
    {
        if(_UpdateType == UpdateType.FixedUpdate)
        {
            QueryRaycasts();
        }
    }

    private void LateUpdate()
    {
        if(_UpdateType == UpdateType.LateUpdate)
        {
            QueryRaycasts();
        }
    }

    void QueryRaycasts()
    {
        float angle = 0.0f;
        for (int i = 0; i < _RaycastCount; i++)
        {
            Vector3 direction =  Quaternion.AngleAxis(angle, Vector3.forward) * transform.right;
            Physics.Raycast(transform.position, direction * _MaxDistance, out RaycastHit hit); 
            angle += 360.0f/_RaycastCount;
        }
    }
}
