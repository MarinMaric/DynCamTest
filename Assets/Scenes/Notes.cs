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

    //Fix buttons   <-- replaced with basic path 

    //Camera moving back??  <-- done

    //Gizmos for selected path  <--  NOT DONE

    //Gizmos move with path object (not just handles)   <--done

    //Hijerarhija   <--done

    //Mjenjanje brzine colliderima  <--done

    //Vise collidera    <--done

    //Stop relying on draw gizmos for inspector setting

    //Hierarchy fix     <--done

    //Speed fix         (Speed controller interpolation?)   <--done

    //zoom dovrsiti     (Change collider makes zooming happen?)     <--done

    //Snimiti test scenu kako se koristi sa OBS 
    
    //Dodati brzinu tranzicije za kamere (Change collider?)
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
     
    =========================================================================
    
    ---Changing zoom---

    There has to be a defined period of transition (zoom speed factor).
    In this period of transition the zoom will be done and upon reaching that 
    full percentage the camera will switch.

    The camera needs to know to what field of view it needs to change. That will be
    set in the inspector. If there is an external variable that forces the min or
    max value then it can be used for determining the right mode instead of a counter.

*/
