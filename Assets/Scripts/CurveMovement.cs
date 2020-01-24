using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveMovement : MonoBehaviour
{
    float count = 0.0f;
    public Transform targetPoint;
    public Transform[] points;
    float displacementPerpendicular = 5f;
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
            //pastStart = currentStart;
            //midControlPoint = points[pointA].position + (points[pointB].position - points[pointA].position) / 2 + Vector3.forward * displacementForward;
            //var midGO = Instantiate(waypointPrefab, midControlPoint, Quaternion.identity);
            //midGO.name = "midPoint";

            pastStart = currentStart;
            midControlPoint = (points[pointB].position - points[pointA].position) / 2;
            var midGO = Instantiate(waypointPrefab, midControlPoint, Quaternion.LookRotation(points[pointB].position));
            midGO.name = "midPoint";

            int sign;
            if (points[pointB].position.z < points[pointA].position.z || points[pointB].position.x > points[pointA].position.x)
                sign = 1;
            else sign = 1;

            midGO.transform.localPosition += midGO.transform.right * sign * displacementPerpendicular;
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
