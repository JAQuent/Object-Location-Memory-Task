using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Explanation how to use this script. 

public class ThreeButtonMovement : MonoBehaviour{
	// Static variables
	public static bool movementAllowed = true;
	public static float forwardSpeed = 15.0f;
	public static float rotationSpeed = 50.0f;
	public static bool reset = false;
	// This can be accessed by other scripts to disable movement functionality
	// Or changed the speed etc.

	// Public vars
	public KeyCode forwardKey = KeyCode.W;
	public KeyCode leftTurn = KeyCode.A;
	public KeyCode rightTurn = KeyCode.D;
	public bool actionNeedToBeEnded = true; // With enabled player cannot directly switch between
	// actions. Instead they have to stop the action again. Here Input.GetKeyDown is used. 
	// If false, players can move forward and rotate at the same time as Input.GetKey is used. 

	// Private vars
	// Bools for actionNeedToBeEnded functinality
	public static bool  movingForward = false; // So it can be used by the tracker
	private bool turnignLeft = false;
	private bool turningRight = false;

    // Update is called once per frame
    void Update(){
    	if(movementAllowed){
    		if(actionNeedToBeEnded){
    			// Player need to end current action before switching.
    			// Change the action bools
    			if(Input.GetKeyDown(forwardKey) & !turnignLeft & !turningRight){
    				// Only do something if not turning left or right
    				movingForward = !movingForward; // Flip
                    Debug.Log("forwardKey was pressed.");
    			}

    			if(Input.GetKeyDown(leftTurn) & !movingForward & !turningRight){
    				// Only do something if not moving forward or turning right
    				turnignLeft = !turnignLeft; // Flip
                    Debug.Log("leftTurn was pressed.");
    			}

    			if(Input.GetKeyDown(rightTurn) & !movingForward & !turnignLeft){
    				// Only do something if not moving forward or turning left
    				turningRight = !turningRight; // Flip
                    Debug.Log("rightTurn was pressed.");
    			}

    			// Do actions
    			if(movingForward){
    				transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    			}
    			if(turnignLeft){
    				transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotationSpeed);
    			}
    			if(turningRight){
    				transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * rotationSpeed);
    			}

    		} else {
    			// Player do not need to end their response before switching. 
    		    // If forward key is pressed
                // Log entries are also recorded here but this might a large number of entries. 
		        if(Input.GetKey(forwardKey)){
		        	transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
                    Debug.Log("forwardKey was pressed.");
		        }

		        // If turn left key is pressed
		        if(Input.GetKey(leftTurn)){
		        	transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotationSpeed);
                    Debug.Log("leftTurn was pressed.");
		        }

		        // If turn left key is pressed
		        if(Input.GetKey(rightTurn)){
		        	transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * rotationSpeed);
                    Debug.Log("rightTurn was pressed.");
		        }
    		}

    		// Rest to no movement
    		if(reset){
    			movingForward = false;
    			turnignLeft = false;
    			turningRight = false;
    			reset = false; // Turn of again
    		}
    	}
    }
}
