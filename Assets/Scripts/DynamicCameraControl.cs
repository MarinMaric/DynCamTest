using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using System;

public class DynamicCameraControl : MonoBehaviour
{
    [HideInInspector]
    public CinemachineStateDrivenCamera stateDrivenCamera;
    public GameObject keyPointPrefab;
    public List<GameObject> cameras;
    public List<DynCamera> cameraProperties;

    private void OnValidate()
    {
        foreach (DynCamera dynCam in cameraProperties)
        {
            //if (dynCam.camGO.transform.position != dynCam.position)
            //{
            //    CinemachineTransposer transposer;
            //    dynCam.camGO.TryGetComponent<CinemachineTransposer>(out transposer);
            //    if (transposer!=null)
            //        transposer.m_FollowOffset = dynCam.position;
            //}
            
            if(dynCam.camGO.transform.position != dynCam.originalPosition+dynCam.positionOffset)
            {
                dynCam.camGO.transform.position = dynCam.originalPosition + dynCam.positionOffset;
            }
            if (dynCam.camGO.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView != dynCam.zoomAmount)
            {
                dynCam.camGO.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = dynCam.zoomAmount;
            }
            DynCamZoom dynZoomScript = dynCam.camGO.GetComponent<DynCamZoom>();
            if (dynCam.zoomMin != dynZoomScript.zoomMin || dynCam.zoomMax != dynZoomScript.zoomMax || dynCam.zoomSpeedFactor!=dynZoomScript.speedFactor)
            {
                dynZoomScript.zoomMin = dynCam.zoomMin;
                dynZoomScript.zoomMax = dynCam.zoomMax;
                dynZoomScript.speedFactor = dynCam.zoomSpeedFactor;
            }
            else
            {
                if (dynCam.keyPoints.Count != dynCam.countTracker)
                {
                    for (int i = 0; i < dynCam.keyPoints.Count; i++)
                    {
                        if (dynCam.keyPoints[i] == null)
                        {
                            var keyPoint = Instantiate(keyPointPrefab, dynCam.camGO.transform.position + Vector3.forward * 5f + Vector3.forward*i, Quaternion.identity).transform;
                            keyPoint.name = i.ToString();
                            dynCam.keyPoints[i] = keyPoint;
                        }
                    }
                }

                BezierTravel travelScript = dynCam.camGO.GetComponent<BezierTravel>();
                if (travelScript.points == null)
                    travelScript.points = new List<Transform>();
                travelScript.points.Clear();

                foreach (Transform t in dynCam.keyPoints)
                {
                    travelScript.points.Add(t);
                }
            }
        }
    }

    private void Start()
    {
        //Calculate set paths
        foreach(DynCamera dynCam in cameraProperties)
        {
            dynCam.CreateCurve();
        }
    }
}

[Serializable]
public class DynCamera
{
    public GameObject camGO;
    [Tooltip("Mark for deletion.")]
    public bool delete = false;
    public Vector3 positionOffset;
    [HideInInspector]public Vector3 originalPosition;
    [Tooltip("The closest the camera can zoom in at the target.")]
    public float zoomMin;
    [Tooltip("The farthest the camera can zoom out from the target.")]
    public float zoomMax;
    [Tooltip("Current value for the camera zoom to adjust to. Must be between zoomMin and zoomMax.")]
    public float zoomAmount;
    [Tooltip("The smaller the value the faster the zoom.")]
    [Range(1, 1000)]
    public float zoomSpeedFactor;
    
    [Header("Camera Path")]
    [Tooltip("Smaller values will make the path resemble more of a straight line while larger values will create a more significant curve in the path.")]
    public float curveFactor = 5f;
    public List<Transform> keyPoints;
    [HideInInspector]public int countTracker;
    [HideInInspector]public BezierCurve path;
    [Tooltip("Defines how many points will be sampled from the curve.")]
    public int frequency = 15;
    [HideInInspector]public bool lookForward = true;

    public DynCamera(GameObject cam, Vector3 pos, float zMin = 0f, float zMax = 60f, float zSpeed = 0f)
    {
        camGO = cam;
        if (pos != null)
            positionOffset = pos;
        zoomMin = zMin;
        zoomMax = zMax;
        zoomSpeedFactor = zSpeed;

        var vcam = cam.GetComponent<CinemachineVirtualCamera>();
        vcam.m_Lens.FieldOfView = zoomMax;
        //vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = position;
        originalPosition = camGO.transform.position;
        camGO.transform.position += pos;
    }

    public void CreateCurve()
    {
        path = GameObject.FindGameObjectWithTag("Illustrator").GetComponent<BezierCurve>();
        lookForward = true; 
        BezierTravel travelScript = camGO.GetComponent<BezierTravel>();
        travelScript.points.Clear();
        travelScript.points.Add(keyPoints[0]);
        travelScript.points.Add(CalculateMidpoint(0, 1));
        travelScript.points.Add(keyPoints[1]);

        path.points[0] = travelScript.points[0].position;
        path.points[1] = travelScript.points[1].position;
        path.points[2] = travelScript.points[2].position;

        path.points[0] = path.transform.InverseTransformPoint(path.points[0]);
        path.points[1] = path.transform.InverseTransformPoint(path.points[1]);
        path.points[2] = path.transform.InverseTransformPoint(path.points[2]);

        if (frequency <= 0)
        {
            return;
        }
        float stepSize = frequency;
        if (stepSize == 1)
        {
            stepSize = 1f / stepSize;
        }
        else
        {
            stepSize = 1f / (stepSize - 1);
        }

        for (int f = 0; f < frequency; f++)
        {
            Transform itemGO = new GameObject().transform;
            Vector3 position = path.GetPoint(f * stepSize);
            itemGO.gameObject.name = f.ToString();
            itemGO.transform.localPosition = position;
            if (lookForward)
            {
                if(f<3)
                {
                    travelScript.points[f] = itemGO.transform;
                }
                itemGO.transform.LookAt(position + path.GetDirection(f * stepSize));
            }
            if(f>2)
                travelScript.points.Add(itemGO.transform);
        }
    }

    public Transform CalculateMidpoint(int start, int end)
    {
        Vector3 midPoint = (keyPoints[start].position + keyPoints[end].position) / 2;
        var pointGO = new GameObject();
        pointGO.transform.position = midPoint;
        pointGO.name = "midPoint";
        pointGO.transform.LookAt(keyPoints[1]);
        pointGO.transform.localPosition += pointGO.transform.right * curveFactor;

        return pointGO.transform;
    }
}