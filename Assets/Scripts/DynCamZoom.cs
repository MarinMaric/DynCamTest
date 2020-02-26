using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DynCamZoom : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    [HideInInspector] public bool zoomIn, zoomOut = true;
    [HideInInspector] public float zoomMin, zoomMax;
    [Tooltip("The smaller the value the faster the zoom.")]
    [HideInInspector] public float speedFactor;

    [HideInInspector] public float timeStartedLerping, lerpTime;
    public bool shouldZoom = false;
    bool zoom = false;
    public float zoomNew = 50f;

    float previousValueZoom;
    [HideInInspector] public int counter = 1;
    [HideInInspector] public float percentageComplete = 0;

    [HideInInspector] public bool external = false;
    [HideInInspector] public float externalValue = int.MaxValue;
    float timeSinceStarted = 0f;

    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        previousValueZoom = zoomNew;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            shouldZoom = true;
            counter++;
        }

        if (shouldZoom)
        {
            timeStartedLerping = Time.time;
            shouldZoom = false;

            if (external)
            {
                zoomNew = externalValue;
            }
            else
            {
                if (counter % 2 == 0)
                    zoomNew = zoomMin;
                else zoomNew = zoomMax;
            }

            zoom = true;
        }

        if (zoom)
            LerpZoom(zoomNew);
    }

    void LerpZoom(float targetZoom)
    {
        timeSinceStarted += Time.deltaTime;
        percentageComplete = timeSinceStarted / speedFactor;
        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, targetZoom, percentageComplete);
        if ((int)vcam.m_Lens.FieldOfView==(int)targetZoom)
        {
            zoom = false;
        }
    }

    public bool CheckZoomValue(float zoomval)
    {
        return (int)vcam.m_Lens.FieldOfView == (int)zoomval;
    }
}
