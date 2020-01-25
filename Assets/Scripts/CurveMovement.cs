using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveMovement : MonoBehaviour
{
    float count = 0.0f;
    public Transform targetPoint;
    public Transform[] points;
    public float smoothFactor = 5f;
    public GameObject waypointPrefab;

    int currentStart = 0, currentGoal = 1;
    int pastStart;
    Vector3 midControlPoint;

    void Start()
    {
        pastStart = int.MaxValue;
    }

    void Update()
    {
        if (currentStart < points.Length-1)
            LerpPoints(currentStart, currentGoal);
    }

    void LerpPoints(int pointA, int pointB)
    {
        //I need to set the speed therefore determining it by interpolation is unacceptable. The time for count should be time necesarry to cover the distance with such a speed.
        if (pastStart != currentStart)
        {
            if (currentStart != 0)
            {
                Destroy(GameObject.Find("midPoint"));
            }

            pastStart = currentStart;
            midControlPoint = (points[pointA].position + points[pointB].position) / 2;
            var midGO = Instantiate(waypointPrefab, midControlPoint, Quaternion.identity);
            midGO.transform.LookAt(points[pointB].position);
            midGO.name = "midPoint";

            midGO.transform.localPosition += midGO.transform.right * smoothFactor;
            midControlPoint = midGO.transform.position;
        }

        if (count < 1.0f)
        {
            count += 1.0f * Time.deltaTime;
            Vector3 m1 = Vector3.Lerp(points[pointA].position, midControlPoint, count);
            Vector3 m2 = Vector3.Lerp(midControlPoint, points[pointB].position, count);
            transform.position = Vector3.Lerp(m1, m2, count);
        }
        else if (currentGoal < points.Length)
        {
            currentStart = currentGoal;
            currentGoal = currentStart + 1;
            count = 0f;
        }
    }
}
