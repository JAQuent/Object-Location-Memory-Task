using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple script to trial and compare different ways to calculate the heading direction/angle.
// Used only for development of the task.

public class headingAngle_testScript : MonoBehaviour{
	public float headingAngle_york;
	public float headingAngle_current;

    // Update is called once per frame
    void Update(){
    	// The way heading angle is calculated in the york code
    	var forward = transform.forward;
    	forward.y = 0;
    	headingAngle_york = Quaternion.LookRotation(forward).eulerAngles.y;

    	// The way it is done here
    	Vector3 r = gameObject.transform.eulerAngles;
    	headingAngle_current = r.y;
        
    }
}
