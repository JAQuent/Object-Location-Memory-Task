using UnityEngine;

public class objectScript : MonoBehaviour{
	// Public vars
    public static float rotationSpeed = 0.0f;
    public AudioClip collectSound; 

    // Update is called once per frame
    void Update(){
        transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * 10.0f);
    }

    /// <summary>
    /// If the player enters the collider end the trial.
    /// </summary>
    void OnTriggerEnter(Collider other){
    	if (other.name == "Player"){
    		// Play sound but only if sound mode is set 1. 
            if(ExperimentController.soundMode == 1){
                AudioSource.PlayClipAtPoint(collectSound, gameObject.transform.position, 1.0f);
            }

    		// Get Experiment GameObject
    		GameObject Experiment = GameObject.Find("Experiment");
    		Experiment.GetComponent<ExperimentController>().EndTrial();
    	}
    } 
}
