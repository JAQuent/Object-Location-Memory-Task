using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will automatically righten the player after falling down. 
public class selfRightingScript : MonoBehaviour{
	// public static
	public static float tolerance = 3.0f;

	// private vars
	private float x_pos;
	private float y_pos;
	private float z_pos;
	private float x_rot;
	private float y_rot;
	private float z_rot;
	private float diff_x;
	private float diff_z;

    // Update is called once per frame
    void Update(){
    	// Access rotation
    	x_rot = Mathf.Round(gameObject.transform.eulerAngles.x);
    	z_rot = Mathf.Round(gameObject.transform.eulerAngles.z);

    	// Calculate difference
    	diff_x = Mathf.Abs(Mathf.DeltaAngle(x_rot, 0.0f));
    	diff_z = Mathf.Abs(Mathf.DeltaAngle(z_rot, 0.0f));

    	// Check the current x_rot & z_rot
    	if(diff_x > tolerance | diff_z > tolerance){
    		// Log Entry
    		Debug.Log("Attempting to righten the player.");

    		// Get y rotation
    		y_rot = gameObject.transform.eulerAngles.y;

    		// Reset movement
    		ThreeButtonMovement.reset = true;

    		// Set player rotation
			gameObject.transform.rotation = Quaternion.Euler(0, y_rot, 0);
    	}       
    }
}