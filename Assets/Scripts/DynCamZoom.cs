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

    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (shouldZoom)
        {
            timeStartedLerping = Time.time;
            shouldZoom = false;
        }
        if (zoomIn) //zooming in means decreasing field of view
        {
            LerpZoom(ref zoomIn, zoomMin);

        }
        else if(zoomOut) //zooming out means increasing field of view
        {
            LerpZoom(ref zoomOut, zoomMax);
        }
    }

    void LerpZoom(ref bool zoomMode, float targetZoom)
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / speedFactor;
        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, targetZoom, percentageComplete);
        if (percentageComplete >= 1)
            zoomIn = false;
    }
}
