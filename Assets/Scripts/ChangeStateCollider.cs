using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeStateCollider : MonoBehaviour
{
    bool triggered = false;
    public int cameraID;
    DynCamZoom zoomScript;
    public float zoomAmount = 0;

    private void Update()
    {
        if (triggered)
        {
            if (zoomScript.CheckZoomValue(zoomAmount))
            {
                ChangeState();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            var dynCam = DynamicCameraControl.Instance.GetDynCamByID(cameraID);
            zoomScript = dynCam.camGO.GetComponent<DynCamZoom>();
            zoomScript.external = true;
            zoomScript.externalValue = zoomAmount;
            zoomScript.shouldZoom = true;
            zoomScript.counter++;
            triggered = true;
        }
    }

    void ChangeState()
    {
        var anim = DynamicCameraControl.Instance.animatedTarget.GetComponent<Animator>();
        anim.SetInteger("StateCounter", anim.GetInteger("StateCounter") + 1);
        DynamicCameraControl.changingState = true;
        gameObject.SetActive(false);
    }
}
