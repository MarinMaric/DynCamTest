using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BezierTravel : MonoBehaviour
{
    public List<Vector3> points;
    public float speed = 5f;
    public int counter = 0;
    private bool check=false;
    public bool activeCamera = false;

    [HideInInspector]
    public float speedChange = 0f, zoomChange=0f;
    [HideInInspector]
    public bool interpolateSpeed = false;
    [HideInInspector]
    public float timeStartedLerping = 0f;
    [HideInInspector]
    public float speedFactor = 1f;
    [HideInInspector]
    public bool changeSpeed, changeZoom = false;

    public bool stationary = false;

    private void Start()
    {
        if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.activeCameraIndex].camGO.name == gameObject.name)
            activeCamera = true;
    }

    private void OnDrawGizmos()
    {
        if (!check && !Application.isPlaying)
        {
            if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camGO.name == gameObject.name)
            {
                #region obsolete check 
                //if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path == null || (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path != null && DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path.gameObject.name != gameObject.name + "_Path"))
                //{
                //    if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path != null)
                //        DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path = null;
                //    var pathGO = new GameObject();
                //    pathGO.transform.parent = DynamicCameraControl.Instance.GetDynCamByGO(gameObject).cameraParent.transform;
                //    pathGO.name = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camGO.name + "_Path";
                //    var splineScript = pathGO.AddComponent<BezierSpline>();
                //    splineScript.splineId = DynamicCameraControl.Instance.GetDynCamByGO(gameObject).camID;
                //    DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].path = pathGO.GetComponent<BezierSpline>();
                //} 
                //if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].changeCollider == null || (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].changeCollider != null && DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].changeCollider.gameObject.name != gameObject.name + "_Collider")
                //    || DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].changeCollider.TryGetComponent<ChangeStateCollider>(out ChangeStateCollider col) == false)
                //{
                //    if (DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].changeCollider != null)
                //    {
                //        DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].changeCollider = null;
                //        DestroyImmediate(DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].changeCollider);
                //    }
                //    var colliderGO = new GameObject();
                //    colliderGO.transform.parent = DynamicCameraControl.Instance.GetDynCamByGO(gameObject).cameraParent.transform;
                //    colliderGO.name = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].camGO.name + "_ChangeCollider";
                //    colliderGO.AddComponent<ChangeStateCollider>();
                //    var collider = colliderGO.AddComponent<BoxCollider>();
                //    collider.size = new Vector3(5f, 5f, 1.5f);
                //    collider.isTrigger = true;
                //    DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.cameraProperties.Count - 1].changeCollider = collider;
                //}
                //if (DynamicCameraControl.Instance.GetDynCamByGO(gameObject).speedColliders.Count != 0)
                //{
                //    var dyn = DynamicCameraControl.Instance.GetDynCamByGO(gameObject);
                //    dyn.speedColliders.Clear();
                //    dyn.speedCollidersCount = 0;
                //}
                #endregion

                var dynCam = DynamicCameraControl.Instance.GetDynCamByGO(gameObject);

                CheckPath(dynCam);
                CheckCollider(dynCam);
                CheckSpeedTriggers(dynCam);

                check = true;
            }
        }
    }

    private void Update()
    {
        if (activeCamera)
        {
            if (interpolateSpeed)
            {
                InterpolateSpeed(speedChange);
            }

            if (!stationary)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[counter], Time.deltaTime * speed);
                transform.LookAt(points[counter]);
                if (Vector3.Distance(transform.position, points[counter]) < 1f && counter != points.Count - 1)
                    counter++;
            }

        }
    }

    public void InterpolateSpeed(float speedChange)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / speedFactor;
        speed = Mathf.Lerp(speed, speedChange, percentageComplete);
        if (changeZoom)
        {
            float fov = gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView;
            gameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = Mathf.Lerp(fov, zoomChange, percentageComplete);
        }
        if (percentageComplete >= 1)
        {
            interpolateSpeed = false;
        }
    }

    public void CheckPath(DynCamera dynCam) 
    {
        if (dynCam.path != null && dynCam.path.splineId == dynCam.camID)
            return;
        else if(GameObject.Find(dynCam.camGO.name + "_Path")==null)
        {
            var pathGO = new GameObject();
            pathGO.name = gameObject.name + "_Path";
            pathGO.transform.parent = dynCam.cameraParent;
            var splineScript = pathGO.AddComponent<BezierSpline>();
            splineScript.splineId = dynCam.camID;
            dynCam.path = splineScript;
        }
    }
    public void CheckCollider(DynCamera dynCam)
    {
        string nameCompare = gameObject.name + "_ChangeCollider";
        if (dynCam.changeCollider != null && dynCam.changeCollider.name == nameCompare)
            return;
        else if(GameObject.Find(nameCompare)==null)
        {
            var colliderGO = new GameObject();
            colliderGO.name = nameCompare;
            var changeScript = colliderGO.AddComponent<ChangeStateCollider>();
            changeScript.cameraID = dynCam.camID;
            var col = colliderGO.AddComponent<BoxCollider>();
            col.size = new Vector3(5f, 5f, 1.5f);
            col.isTrigger = true;
            dynCam.changeCollider = col;
            colliderGO.transform.parent = dynCam.cameraParent;
        }
    }
    public void CheckSpeedTriggers(DynCamera dynCam)
    {
        if (dynCam.triggerList.speedColliders.Count > 0 && dynCam.triggerList.speedColliders[0].GetComponent<SpeedCollider>().colliderID == dynCam.camID)
            return;
        else
        {
            dynCam.triggerList.speedColliders.Clear();
            dynCam.speedCollidersCount = 0;
        }
    }
}
