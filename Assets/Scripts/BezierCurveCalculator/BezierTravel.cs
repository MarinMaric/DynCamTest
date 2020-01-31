using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierTravel : MonoBehaviour
{
    public List<Transform> points;
    public float speed = 5f;
    public int counter = 0;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[counter].position, Time.deltaTime * speed);
        //transform.LookAt(points[counter]);
        if (Vector3.Distance(transform.position, points[counter].position) < 1f && counter != points.Count - 1)
            counter++;
    }
}
