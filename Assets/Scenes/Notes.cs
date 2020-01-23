using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notes : MonoBehaviour
{
    //The camera needs to constantly target the first one which can be changed as the order changes and needs to be tracked (code already exists)
    //The zoom needs to be fixed while active but while blending to a different camera needs to slowly adapt
    //The camera needs to follow along a fixed path of keypoints with a set curve (or direct line depending on the smooth parameter)

    //The camera needs to be positioned according to a transform or set coordinates?

    //WHAT IT CAN DO NOW:
    //  - easily add and remove cameras
    //  - access all relevant properties without going through the hierarchy
    //  - set field of view for proper fixed zoom
    //  - set camera offset by coordinates 

    //WHAT NEEDS TO BE DONE:
    //  - add blending/transitioning according to the zoom speed parameter
    //  - moving between key points based on a smooth factor
    //  - set camera offset (or position?) based on transform.. or maybe not?

    //How? Transitions are handled by the state driven camera referencing an animator which is ran by an in variable.
    //At any point the user should be able to move to a different state (camera) and the transition can be done
    //either automatically or manually by animating the field of view to adjust to the next camera to which we're transitioning
    //based on the speed property. 
}