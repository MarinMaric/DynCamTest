using UnityEngine;

public class SplineDecorator : MonoBehaviour
{
    public BezierSpline spline;

    [HideInInspector] public int frequency;

    public bool lookForward;

    public Transform[] items;

    BezierTravel travelScript;

    public void Start()
    {
        Decorate();
    }

    public void Decorate()
    {
        BezierTravel travelScript = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.activeCameraIndex].camGO.GetComponent<BezierTravel>();
        frequency = DynamicCameraControl.Instance.cameraProperties[DynamicCameraControl.Instance.activeCameraIndex].frequency;

        if (frequency <= 0 || items == null || items.Length == 0)
        {
            return;
        }
        float stepSize = frequency * items.Length;
        if (stepSize == 1)
        {
            stepSize = 1f / stepSize;
        }
        else
        {
            stepSize = 1f / (stepSize - 1);
        }
        for (int p = 0, f = 0; f < frequency; f++)
        {
            for (int i = 0; i < items.Length; i++, p++)
            {
                Transform item = Instantiate(items[i]) as Transform;
                Vector3 position = spline.GetPoint(p * stepSize);
                item.transform.localPosition = position;
                if (lookForward)
                {
                    item.transform.LookAt(position + spline.GetDirection(p * stepSize));
                }
                item.transform.parent = transform;
                travelScript.points.Add(item.position);
                //Destroy(item.gameObject);
            }
        }

        DynamicCameraControl.decorator = gameObject;
    }
}