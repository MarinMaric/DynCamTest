using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveDecorator : MonoBehaviour
{
    public BezierCurve curve;
    public int frequency;
    public bool lookForward = true;
    public Transform item;
    BezierTravel travelScript;

    private void Start()
    {
        travelScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BezierTravel>();

        if(frequency<=0 ||item == null)
        {
            return;
        }
        float stepSize = frequency;
        if (stepSize == 1)
        {
            stepSize = 1f / stepSize;
        }
        else
        {
            stepSize = 1f / (stepSize - 1);
        }
        for(int f=0; f < frequency; f++)
        {
            Transform itemGO = Instantiate(item) as Transform;
            Vector3 position = curve.GetPoint(f * stepSize);
            itemGO.gameObject.name = f.ToString();
            itemGO.transform.localPosition = position;
            if (lookForward)
            {
                itemGO.transform.LookAt(position + curve.GetDirection(f * stepSize));
            }

            travelScript.points.Add(itemGO.transform);
        }
    }
}
