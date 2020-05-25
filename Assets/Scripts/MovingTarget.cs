using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    Rigidbody rb;
    float speed = 10f;
    public GameObject[] waypoints;
    int currentWaypoint = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Vector3.Distance(waypoints[currentWaypoint].transform.position, transform.position) < .1f && currentWaypoint<waypoints.Length)
        {
            currentWaypoint++;
        }
    }

    void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.LookRotation(waypoints[currentWaypoint].transform.position - transform.position, Vector3.up));
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }
}
