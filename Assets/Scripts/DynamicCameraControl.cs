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
    public List<GameObject> cameras;
    public List<DynCamera> cameraProperties;

    private void OnValidate()
    {
        foreach(DynCamera dynCam in cameraProperties) {
            if(dynCam.camGO.transform.position != dynCam.position)
            {
                dynCam.camGO.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = dynCam.position;
            }
            if(dynCam.camGO.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView != dynCam.zoomAmount)
            {
                dynCam.camGO.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = dynCam.zoomAmount;
            }
        }
    }
}

[Serializable]
public class DynCamera
{
    public GameObject camGO;
    public bool delete = false;
    public Vector3 position;
    public float zoomAmount, zoomSpeed;

    public DynCamera(GameObject cam, Vector3 pos, float zAmount = 60f, float zSpeed = 0f)
    {
        camGO = cam;
        if (pos != null)
            position = pos;
        zoomAmount = zAmount;
        zoomSpeed = zSpeed;

        var vcam = cam.GetComponent<CinemachineVirtualCamera>();
        vcam.m_Lens.FieldOfView = zoomAmount;
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = position;
    }
}