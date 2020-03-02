using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpeedCollider))]
public class ColliderSelfCleanUp : MonoBehaviour
{
    private void OnDestroy()
    {
        var dynCam = DynamicCameraControl.Instance.GetDynCamByID(GetComponent<SpeedCollider>().colliderID);
        if (dynCam != null)
        {
            foreach (BoxCollider speedCol in dynCam.triggerList.speedColliders)
            {
                if (speedCol.gameObject.name == gameObject.name)
                {
                    dynCam.triggerList.speedColliders.Remove(speedCol);
                    dynCam.speedCollidersCount--;
                    break;
                }
            }
        }
    }
}
