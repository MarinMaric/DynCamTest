using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveMovement : MonoBehaviour
{
    float count = 0.0f;
    public Transform targetPoint;
    public Transform[] points;
    public float curveFactor = 5f;
    public GameObject waypointPrefab;

    int currentStart = 0, currentGoal = 1;
    int pastStart;
    Vector3 midControlPoint;
    public float speed = 5f;

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

            midGO.transform.localPosition += midGO.transform.right * curveFactor;
            midControlPoint = midGO.transform.position;
        }

        if (count < 1.0f)
        {
            count += 1.0f * Time.deltaTime;
        }
        else if (currentGoal < points.Length)
        {
            currentStart = currentGoal;
            currentGoal = currentStart + 1;
            count = 0f;
        }

        if (currentStart != pastStart)
        {
            Vector3 m1 = Vector3.MoveTowards(points[pointA].position, midControlPoint, count * speed);
            Vector3 m2 = Vector3.MoveTowards(midControlPoint, points[pointB].position, count * speed);
            transform.position = Vector3.MoveTowards(m1, m2, count * speed);
        }

    }
}
