using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTravel : MonoBehaviour
{
    public List<Vector3> points;
    public float speed = 5f;
    public int counter = 0;
    private bool check=false;

    private void OnDrawGizmos()
    {
        if (!check)
        {
            if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camGO.name == gameObject.name)
            {
                DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path = null;
                var colliderGO = new GameObject();
                colliderGO.name = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camGO.name + "_Collider";
                var collider = colliderGO.AddComponent<BoxCollider>();
                collider.size = new Vector3(10, 10, 5);
                DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].collider = collider;
                check = true;
            }
        }
        
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[counter], Time.deltaTime * speed);
        transform.LookAt(points[counter]);
        if (Vector3.Distance(transform.position, points[counter]) < 1f && counter != points.Count - 1)
            counter++;
    }
}
