using UnityEngine;

public class outerTriggerController : MonoBehaviour{
	// Variable to decide if this script should care about entry or exit
	public bool careAboutEnter; // true for OnTriggerEnter & false for OnTriggerExit

	// This script simply changes the boolean in measuresAgainstHittingWall.cs to true if something enters the trigger zone.
	// This is supposed to be activated if the player falls down
	void OnTriggerEnter (Collider other){
	 	if (other.name == "Player" & careAboutEnter){
	 		measuresAgainstHittingWall.leftEnvironment = true;
	 		Debug.Log("The player entered: " + gameObject.name);
	 	}
	}

	void OnTriggerExit (Collider other){
	 	if (other.name == "Player" & !careAboutEnter){
	 		measuresAgainstHittingWall.leftEnvironment = true;
	 		Debug.Log("The player exited: " + gameObject.name);
	 	}
	}
}