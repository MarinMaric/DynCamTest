using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynCollider : MonoBehaviour
{
    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            var anim = DynamicCameraControl.Instance.animatedTarget.GetComponent<Animator>();
            anim.SetInteger("StateCounter", anim.GetInteger("StateCounter")+1);
        }
    }
}
