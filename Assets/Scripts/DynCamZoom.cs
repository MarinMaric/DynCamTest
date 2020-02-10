using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DynCamZoom : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    [HideInInspector]public bool zoomIn, zoomOut=true;
    [HideInInspector]public float zoomMin, zoomMax;
    [Tooltip("The smaller the value the faster the zoom.")]
    [HideInInspector]public float speedFactor;

    [HideInInspector]public float timeStartedLerping, lerpTime;
    public bool shouldZoom = true;
    bool zoom = false;
    public float zoomNew = 50f;

    float previousValueZoom;
    int counter = 1;

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

            if (counter % 2 == 0)
                zoomNew = zoomMin;
            else zoomNew = zoomMax;
            zoom = true;
        }

        if (zoom)
            LerpZoom(zoomNew);
    }

    void LerpZoom(float targetZoom)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / speedFactor;
        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, targetZoom, percentageComplete);
        if (percentageComplete >= 1)
        {
            zoom = false;
        }
    }
}
