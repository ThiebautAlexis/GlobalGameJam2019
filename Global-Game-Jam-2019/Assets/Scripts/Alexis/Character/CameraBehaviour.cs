using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] Transform target; 
    public Vector3 CameraPosition { get { return target.position - new Vector3(0, 0, 10);  } }

    void FollowTarget()
    {
        if (!target) return;
        transform.position = CameraPosition; 
    }
    private void Update()
    {
        FollowTarget(); 
    }
}
