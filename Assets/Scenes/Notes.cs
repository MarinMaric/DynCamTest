using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    //Auto generating one point finished. May need a quadratic mode.   <--trivial

    //Setting a position via transposer offset or Do Nothing with basic transform   <--trivial

    //Make singleton    <--done

    //Give full control over control points instead of auto-generating  <--done

    //Cleanup waypoint items automatically  <--done

    //Incorporate splines to DynamicCameraControl   <--done

    //Collider trigger  <--done

    //Fix buttons

    //Camera moving back??
}

/*
    ************************************************************************
    
    Currently making curves by:
        1)  inserting a number of points in camera properties
        2)  creating objects for each point for camera properties
        3)  setting path points every time camera property count changes

    This would've gone anyway when I decided to remove the manual hassle.

    New way of making splines by:
        1) pressing a button to add a new curve
        2) curve being generated on the illustrator
        3) individual transforms being assigned to the camera properties
     

     
*/