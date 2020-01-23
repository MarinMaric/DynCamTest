using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GroupControl : MonoBehaviour
{
    public GameObject[] group;
    GameObject activeTarget;
    public LayerMask targetLayer;

    void Start()
    {
        group = GameObject.FindGameObjectsWithTag("Target");   
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, targetLayer))
            {
                activeTarget = hitInfo.collider.gameObject;
            }
        }

        if(Input.GetAxisRaw("Horizontal")!=0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if(activeTarget!=null)
                activeTarget.transform.Translate(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        }
    }
}