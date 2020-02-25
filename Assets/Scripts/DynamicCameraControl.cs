using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;
using System;

public class DynamicCameraControl : MonoBehaviour
{
    public CinemachineStateDrivenCamera stateDrivenCamera;
    public GameObject keyPointPrefab;
    public GameObject animatedTarget;
    public GameObject decorator;
    public static int idGenerator = 0;
    [HideInInspector]public List<GameObject> cameras;
    public List<DynCamera> cameraProperties;
    [HideInInspector]public int activeCameraIndex = 0;
    [HideInInspector]public int selectedCameraIndex = int.MaxValue;

    [HideInInspector]
    public static bool changingState = false;
    //private static bool m_ShuttingDown = false;
    //private static object m_Lock = new object();
    private static DynamicCameraControl m_Instance;
    public static DynamicCameraControl Instance
    {
        get
        {
            //if (m_ShuttingDown)
            //{
            //    Debug.LogWarning("[Singleton] Instance '" + typeof(DynamicCameraControl) +
            //        "' already destroyed. Returning null.");
            //    return null;
            //}

            //lock (m_Lock)
            //{
            //{
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (DynamicCameraControl)FindObjectOfType(typeof(DynamicCameraControl));

                // Create new instance if one doesn't already exist.
                //if (m_Instance == null)
                //{
                //    // Need to create a new GameObject to attach the singleton to.
                //    var singletonObject = new GameObject();
                //    m_Instance = singletonObject.AddComponent<DynamicCameraControl>();
                //    singletonObject.name = typeof(DynamicCameraControl).ToString() + " (Singleton)";

                //    // Make instance persistent.
                //    //DontDestroyOnLoad(singletonObject);
                //}
                 }
            return m_Instance;

            //}
        }
    }
    //private void OnApplicationQuit()
    //{
    //    m_ShuttingDown = true;
    //}
    //private void OnDestroy()
    //{
    //    m_ShuttingDown = true;
    //}
    [HideInInspector] public bool cleanUpColliders=false;
    [HideInInspector] public static int cleanUpID;
    [HideInInspector] public static int cleanedUpCounter = 0;

    private void OnValidate()
    {
        foreach (DynCamera dynCam in cameraProperties)
        {
            #region transposer obsolete
            //if (dynCam.camGO.transform.position != dynCam.position)
            //{
            //    CinemachineTransposer transposer;
            //    dynCam.camGO.TryGetComponent<CinemachineTransposer>(out transposer);
            //    if (transposer!=null)
            //        transposer.m_FollowOffset = dynCam.position;
            //}
            #endregion

            //if (dynCam.camGO.transform.position != dynCam.originalPosition+dynCam.positionOffset)
            //{
            //    dynCam.camGO.transform.position = dynCam.originalPosition + dynCam.positionOffset;
            //}
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
            if (dynCam.zoomIn != dynZoomScript.zoomIn || dynCam.zoomOut != dynZoomScript.zoomOut)
            {
                dynZoomScript.zoomIn = dynCam.zoomIn;
                dynZoomScript.zoomOut = dynCam.zoomOut;

                dynZoomScript.shouldZoom = true;
            }
            if(dynCam.zoomNew != dynZoomScript.zoomNew)
            {
                dynZoomScript.zoomNew = dynCam.zoomNew;
                dynZoomScript.shouldZoom = true;
            }
            if(dynCam.speedColliders.Count != dynCam.speedCollidersCount)
            {
                for(int i=0;i<dynCam.speedColliders.Count;i++)
                {
                    if (dynCam.speedColliders[i] == null || dynCam.speedColliders[i].GetComponent<SpeedCollider>().colliderID !=dynCam.camID ||dynCam.speedColliders[i].gameObject.name!= dynCam.camGO.name + "_SpeedCollider" + i)
                    {
                        if (i < dynCam.speedCollidersCount)
                        {
                            dynCam.speedColliders.RemoveAt(i);
                        }
                        else
                        {
                            var speedCol = new GameObject();
                            speedCol.transform.parent = dynCam.speedCollidersParent;
                            speedCol.AddComponent<BoxCollider>();
                            speedCol.name = dynCam.camGO.name + "_SpeedCollider" + i;
                            var colScript = speedCol.AddComponent<SpeedCollider>();
                            colScript.colliderID = dynCam.camID;
                            //speedCol.AddComponent<ColliderSelfCleanUp>();
                            dynCam.speedColliders[i] = speedCol.GetComponent<BoxCollider>();
                            dynCam.speedColliders[i].isTrigger = true;
                        }
                    }
                }
                
                if (dynCam.speedColliders.Count < dynCam.speedCollidersCount)
                {
                    cleanUpID = dynCam.camID;
                    cleanUpColliders = true;
                }
                else
                {
                    dynCam.speedCollidersCount = dynCam.speedColliders.Count;
                }
            }

            #region obsolete curve point count validating
            //else
            //{
            //    if (dynCam.keyPoints.Count != dynCam.countTracker)
            //    {
            //        for (int i = 0; i < dynCam.keyPoints.Count; i++)
            //        {
            //            if (dynCam.keyPoints[i] == null)
            //            {
            //                var keyPoint = Instantiate(keyPointPrefab, dynCam.camGO.transform.position + Vector3.forward * 5f + Vector3.forward*i, Quaternion.identity).transform;
            //                keyPoint.name = i.ToString();
            //                dynCam.keyPoints[i] = keyPoint;
            //            }
            //        }
            //    }

            //    BezierTravel travelScript = dynCam.camGO.GetComponent<BezierTravel>();
            //    if (travelScript.points == null)
            //        travelScript.points = new List<Transform>();
            //    travelScript.points.Clear();

            //    foreach (Transform t in dynCam.keyPoints)
            //    {
            //        travelScript.points.Add(t);
            //    }
            //}
            #endregion
        }
    }

    private void Awake()
    {
        #region obsolete create curve method
        ////Calculate set paths
        //foreach(DynCamera dynCam in cameraProperties)
        //{
        //    dynCam.CreateCurve();
        //}
        #endregion
        GameObject.Find("PathDecorator").GetComponent<SplineDecorator>().spline = cameraProperties[activeCameraIndex].path;
        animatedTarget = GameObject.FindGameObjectWithTag("Animated");

        //---just checking state driven cam out

        var stateDrivenCam = GetComponent<CinemachineStateDrivenCamera>();
    }

    #region obsolete key points
    public GameObject PopulateKeyPoint(Vector3 position) {
        position = transform.TransformPoint(position);
        var itemGO = Instantiate(keyPointPrefab, position, Quaternion.identity);
        return itemGO;
    }

    public void DestroyKeyPoint(Transform p)
    {
        if(p!=null)
            DestroyImmediate(p.gameObject);
    }
    #endregion

    private void Update()
    {
        if (DynamicCameraControl.changingState)
        {
            ChangeActiveCamera();
        }
    }

    public void ChangeActiveCamera()
    {
        for(int i=0; i < cameraProperties.Count; i++)
        {
            if (Camera.main.gameObject.transform.position == cameraProperties[i].camGO.transform.position && activeCameraIndex != i)
            {
                cameraProperties[activeCameraIndex].camGO.GetComponent<BezierTravel>().activeCamera = false;
                activeCameraIndex = i;
                cameraProperties[i].camGO.GetComponent<BezierTravel>().activeCamera = true;
                DynamicCameraControl.changingState = false;
                decorator.GetComponent<SplineDecorator>().Decorate();
                break;
            }   
        }

        //Debug.Log(cameraProperties[activeCameraIndex].camGO.name);
    }

    public DynCamera GetDynCamByGO(GameObject go) {
        foreach(DynCamera dynCam in cameraProperties)
        {
            if (dynCam.camGO.name == go.name)
            {
                return dynCam;
            }
        }

        return null;
    }
    public DynCamera GetDynCamByID(int id)
    {
        foreach(DynCamera dynCam in cameraProperties)
        {
            if (dynCam.camID == id)
                return dynCam;
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        if (cameraProperties.Count == 0 || selectedCameraIndex > cameraProperties.Count-1)
            return;
        if (cameraProperties[selectedCameraIndex].path != null)
        {
            int counter = 0;
            for (int i = 0; i < cameraProperties[selectedCameraIndex].path.points.Length; i++)
            {
                counter++;
                if (i == 0 || counter % 4==0)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.white;
                Gizmos.DrawSphere(cameraProperties[selectedCameraIndex].path.transform.TransformPoint(cameraProperties[selectedCameraIndex].path.points[i]), .1f);
                if (counter % 4 == 0)
                    counter++;
            }
            var points = cameraProperties[selectedCameraIndex].camGO.GetComponent<BezierTravel>().points;
            if (points != null)
            {
                foreach (Vector3 p in points)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(transform.TransformPoint(p), .1f);
                }
            }
        }
    }
}

[Serializable]
public class DynCamera
{
    [Header("General Settings")]
    [Tooltip("Camera game object")]
    public GameObject camGO;
    [Tooltip("Collider used for triggering the camera")]
    public BoxCollider changeCollider;
    public List<BoxCollider> speedColliders;
    [HideInInspector] public Transform cameraParent;
    [HideInInspector] public Transform speedCollidersParent;
    [HideInInspector] public int speedCollidersCount = 0;
    [Tooltip("Mark for deletion.")]
    public bool delete = false;
    [HideInInspector]public Vector3 positionOffset;
    [HideInInspector]public Vector3 originalPosition;
    /*[HideInInspector] */public int camID;

    [Header("Zoom Settings")]
    [Tooltip("The closest the camera can zoom in at the target.")]
    /*[HideInInspector]*/ public float zoomMin;
    [Tooltip("The farthest the camera can zoom out from the target.")]
    /*[HideInInspector] */public float zoomMax;
    
    [Tooltip("Current value for the camera zoom to adjust to. Must be between zoomMin and zoomMax.")]
    /*[HideInInspector]*/ public float zoomAmount;
    [HideInInspector] public float zoomNew;
    [Tooltip("The smaller the value the faster the zoom.")]
    [Range(1, 1000)]
    public float zoomSpeedFactor;
    [Tooltip("If true, camera field of view will be adjusted to zoomMin. If false, camera field of view will be adjusted to zoomMax.")]
    [HideInInspector] public bool zoomIn = false, zoomOut = false;
    
    //[Tooltip("Smaller values will make the path resemble more of a straight line while larger values will create a more significant curve in the path.")]
    //public float curveFactor = 5f;
    //public List<Transform> keyPoints;
    [HideInInspector]public int countTracker;
    [Header("Camera Path")]
    public BezierSpline path;
    [Tooltip("Defines how many points from the curve will be sampled by the waypoint system.")]
    public int frequency = 15;
    [HideInInspector]public bool lookForward = true;
    //public CustomButton buttonAdd, buttonClear;

    #region obsolete constructor
    //public DynCamera(GameObject cam, Vector3 pos, float zMin = 0f, float zMax = 60f, float zSpeed = 0f)
    //{
    //    camGO = cam;
    //    if (pos != null)
    //        positionOffset = pos;
    //    zoomMin = zMin;
    //    zoomMax = zMax;
    //    zoomSpeedFactor = zSpeed;

    //    var vcam = cam.GetComponent<CinemachineVirtualCamera>();
    //    vcam.m_Lens.FieldOfView = zoomMax;
    //    //vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = position;
    //    originalPosition = camGO.transform.position;
    //    camGO.transform.position += pos;

    //    buttonAdd = new CustomButton("Add Curve", );
    //    buttonClear = new CustomButton("Clear Curve");
    //}
    #endregion

    public void AddCurve()
    {
        if (path == null)
        {
            var pathGO = new GameObject();
            pathGO.name = camGO.name + "_Path";
            pathGO.AddComponent<BezierSpline>();
            path = pathGO.GetComponent<BezierSpline>();
        }

        if (path.ControlPointCount == 0)
        {
            path.Reset();
            #region obsolete key points 
            //for (int i = 0; i < path.ControlPointCount; i++)
            //{
            //    var itemGO = DynamicCameraControl.Instance.PopulateKeyPoint(path.GetControlPoint(i));
            //    itemGO.name = i.ToString();
            //    keyPoints.Add(itemGO.transform);
            //}
            #endregion
        }
        else
        {
            path.AddCurve();
            #region obsolete key points
            //for (int i = keyPoints.Count; i < path.ControlPointCount; i++)
            //{
            //    var itemGO = DynamicCameraControl.Instance.PopulateKeyPoint(path.GetControlPoint(i));
            //    itemGO.name = i.ToString();
            //    keyPoints.Add(itemGO.transform);
            //}
            #endregion
        }
    }

    public void ClearCurve()
    {
        if (path == null)
            return;
        else {
            path.Clear();
            #region obsolete key points
            //foreach(Transform p in keyPoints)
            //{
            //    DynamicCameraControl.Instance.DestroyKeyPoint(p);
            //}
            //keyPoints.Clear();
            #endregion
        }
    }

    #region obsolete curve create method
    //public void CreateCurve()
    //{
    //    path = GameObject.FindGameObjectWithTag("Illustrator").GetComponent<BezierSpline>();
    //    lookForward = true; 
    //    BezierTravel travelScript = camGO.GetComponent<BezierTravel>();
    //    travelScript.points.Clear();
    //    travelScript.points.Add(keyPoints[0]);
    //    travelScript.points.Add(CalculateMidpoint(0, 1));
    //    travelScript.points.Add(keyPoints[1]);

    //    path.points[0] = travelScript.points[0].position;
    //    path.points[1] = travelScript.points[1].position;
    //    path.points[2] = travelScript.points[2].position;

    //    path.points[0] = path.transform.InverseTransformPoint(path.points[0]);
    //    path.points[1] = path.transform.InverseTransformPoint(path.points[1]);
    //    path.points[2] = path.transform.InverseTransformPoint(path.points[2]);

    //    if (frequency <= 0)
    //    {
    //        return;
    //    }
    //    float stepSize = frequency;
    //    if (stepSize == 1)
    //    {
    //        stepSize = 1f / stepSize;
    //    }
    //    else
    //    {
    //        stepSize = 1f / (stepSize - 1);
    //    }

    //    for (int f = 0; f < frequency; f++)
    //    {
    //        Transform itemGO = new GameObject().transform;
    //        Vector3 position = path.GetPoint(f * stepSize);
    //        itemGO.gameObject.name = f.ToString();
    //        itemGO.transform.localPosition = position;
    //        if (lookForward)
    //        {
    //            if(f<3)
    //            {
    //                travelScript.points[f] = itemGO.transform;
    //            }
    //            itemGO.transform.LookAt(position + path.GetDirection(f * stepSize));
    //        }
    //        if(f>2)
    //            travelScript.points.Add(itemGO.transform);
    //    }
    //}
    #endregion
    #region obsolete calculated control point
    //public Transform CalculateMidpoint(int start, int end)
    //{
    //    Vector3 midPoint = (keyPoints[start].position + keyPoints[end].position) / 2;
    //    var pointGO = new GameObject();
    //    pointGO.transform.position = midPoint;
    //    pointGO.name = "midPoint";
    //    pointGO.transform.LookAt(keyPoints[1]);
    //    pointGO.transform.localPosition += pointGO.transform.right * curveFactor;

    //    return pointGO.transform;
    //}
    #endregion
}

