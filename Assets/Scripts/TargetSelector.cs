using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetSelector : MonoBehaviour
{
    public Transform target;
    public LayerMask targetLayer;
    public CinemachineStateDrivenCamera stateDrivenCamera;

    private void Start()
    {
        stateDrivenCamera = GameObject.FindGameObjectWithTag("DynCamManager").GetComponent<CinemachineStateDrivenCamera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit rayInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayInfo, Mathf.Infinity, targetLayer))
            {
                target = rayInfo.transform;
                stateDrivenCamera.LookAt = target;
                stateDrivenCamera.Follow = target;
            }
        }
    }
}
