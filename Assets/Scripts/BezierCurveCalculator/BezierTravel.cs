using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTravel : MonoBehaviour
{
    public List<Vector3> points;
    public float speed = 5f;
    public int counter = 0;
    private bool check=false;
    public bool activeCamera = false;
    private void Start()
    {
        if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.activeCameraIndex].camGO.name == gameObject.name)
            activeCamera = true;
    }

    //private void OnDrawGizmos()
    //{
    //    if (!check)
    //    {
    //        if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camGO.name == gameObject.name)
    //        {
    //            if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path == null || (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path !=null && DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path.gameObject.name != gameObject.name + "_Path"))
    //            {
    //                if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path != null)
    //                    DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path = null;
    //                var pathGO = new GameObject();
    //                pathGO.name = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camGO.name + "_Path";
    //                var splineScript = pathGO.AddComponent<BezierSpline>();
    //                splineScript.splineId = DynamicCameraControl.Instance.GetDynCamByGO(gameObject).camID;
    //                DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path = pathGO.GetComponent<BezierSpline>();
    //            }
    //            if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].collider == null || (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].collider != null && DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].collider.gameObject.name != gameObject.name + "_Collider") || DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].collider.TryGetComponent<DynCollider>(out DynCollider col)==false)
    //            {
    //                if(DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].collider != null)
    //                {
    //                    DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].collider = null;
    //                    DestroyImmediate(DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].collider);
    //                }
    //                var colliderGO = new GameObject();
    //                colliderGO.name = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camGO.name + "_Collider";
    //                colliderGO.AddComponent<DynCollider>();
    //                var collider = colliderGO.AddComponent<BoxCollider>();
    //                collider.size = new Vector3(5f, 5f, 1.5f);
    //                collider.isTrigger = true;
    //                DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].collider = collider;
    //            }
    //            check = true;
    //        }
    //    }
    //}

    private void Update()
    {
        if (activeCamera)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[counter], Time.deltaTime * speed);
            transform.LookAt(points[counter]);
            if (Vector3.Distance(transform.position, points[counter]) < 1f && counter != points.Count - 1)
                counter++;
        }

    }
}
