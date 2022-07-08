using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to the game object that you want to move. There are two modes, 
// which are controlled by the boolean variable actionNeedToBeEnded. If true, then each 
// action has be started & ended with a button press. This is expecially for situation in
// an MRI scanner where button press are not transmitted continuously. If false, then that 
// is not necessary the movement happens as long as button is pressed, which is also allows
// rotation & translation at the same time. 
// The static variables allow you to change speed (e.g. ThreeButtonMovement.forwardSpeed = 20.0f),
// to access whether there the character is currently moving forward. This was added so that we can
// track which time segements include forward movement at which direction for grid-like activity studies
// Further more, you can reset every movement and change the keys assignment
// (e.g. ThreeButtonMovement.leftTurn = (KeyCode) System.Enum.Parse(typeof(KeyCode), newKeys[0]);). 

public class ThreeButtonMovement : MonoBehaviour{
    // Static variables 
    public static bool movementAllowed = true;
    public static float forwardSpeed = 15.0f;
    public static float rotationSpeed = 50.0f;
    public static bool reset = false;
    public static KeyCode leftTurn = KeyCode.A;
    public static KeyCode forwardKey = KeyCode.W;
    public static KeyCode rightTurn = KeyCode.D;
    // Thse can be accessed by other scripts to disable movement functionality
    // For instance it can be used to change the speed etc.

    // Public vars
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
    	// Rest to no movement
        if(reset){
            movingForward = false;
            turnignLeft = false;
            turningRight = false;
            reset = false; // Turn of again
        }

        // Movement control
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
        }
    }
}
