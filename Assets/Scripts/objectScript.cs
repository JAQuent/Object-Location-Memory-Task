using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF; 

public class objectScript : MonoBehaviour{
	// Public vars
    public float rotationSpeed = 0.0f;
    public AudioClip collectSound; 

    // Update is called once per frame
    void Update(){
        transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotationSpeed);
    }

    /// <summary>
    /// If the player enters the collider end the trial.
    /// </summary>
    void OnTriggerEnter(Collider other){
    	if (other.name == "Player"){
    		// Logging
    		Debug.Log("Object Picked Up!");

    		// Play sound
    		AudioSource.PlayClipAtPoint(collectSound, gameObject.transform.position, 1.0f);

    		// Get Experiment GameObject
    		GameObject Experiment = GameObject.Find("Experiment");
    		Experiment.GetComponent<ExperimentController>().EndTrial();
    	}
    } 
}
