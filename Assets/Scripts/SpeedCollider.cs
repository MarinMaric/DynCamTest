using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpeedCollider : MonoBehaviour
{
    public int colliderID;
    public float speedChange = 5f;
    [Range(1,20000)]
    [Tooltip("The smaller the value, the faster the interpolation.")]
    public float speedFactor = 1f;
    bool triggered = false;

    void OnDrawGizmos()
    {
        if (DynamicCameraControl.Instance.cleanUpColliders)
        {
            if (colliderID == DynamicCameraControl.cleanUpID)
            {

                DynCamera dynCam = null;
                foreach (DynCamera cam in DynamicCameraControl.Instance.cameraProperties)
                {
                    if (cam.camID == DynamicCameraControl.cleanUpID)
                    {
                        dynCam = cam;
                        break;
                    }
                }

                var nameNumber = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1, 1));

                if (nameNumber > dynCam.speedColliders.Count-1)
                    DynamicCameraControl.cleanedUpCounter++;
                else return;

                if (dynCam.speedColliders.Count < dynCam.speedCollidersCount)
                {
                    if (Mathf.Abs(dynCam.speedColliders.Count - dynCam.speedCollidersCount) == DynamicCameraControl.cleanedUpCounter)
                    {
                        dynCam.speedCollidersCount = dynCam.speedColliders.Count;
                        DynamicCameraControl.cleanedUpCounter = 0;
                        DynamicCameraControl.Instance.cleanUpColliders = false;
                    }
                }

                DestroyImmediate(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            var movementScript = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.activeCameraIndex].camGO.GetComponent<BezierTravel>();
            movementScript.speedChange = speedChange;
            movementScript.speedFactor = speedFactor;
            movementScript.interpolateSpeed = true;
            movementScript.timeStartedLerping = Time.time;
            triggered = true;
        }
    }
}
