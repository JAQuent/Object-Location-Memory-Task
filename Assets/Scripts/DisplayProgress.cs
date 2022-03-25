using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UXF; 

public class DisplayProgress : MonoBehaviour{
	// Text var
	public Text progress;
	public Session session;

    // Update is called once per frame
    void Update(){
        // Get current values
    	int trialNum =  session.currentTrialNum; 
		int blockNum =  session.currentBlockNum;

		progress.text = "Trail: " + trialNum + "\n" + "Block: " + blockNum;
    }
}
