using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// add the UXF namespace
using UXF;

public class ExperimentGenerator : MonoBehaviour{     
    // generate the blocks and trials for the session.
    // the session is passed as an argument by the event call.
    public List<object> test;
    public float bla;
    public Session session;

    public void Test(){
        bla = session.settings.GetFloat("cueTime");
        test = session.settings.GetObjectList("objectNames_eng");
        Debug.Log(test[0]);
    }


    public void Generate(Session session){
    	// // Get settings from .json
     //    int trialsPerBlock = session.settings.GetInt("trialsPerBlock");
     //    int blocks = session.settings.GetInt("blocks");

     //    test = settings.GetObjectList("objectNames_eng");

        /*// Loop through the number of blocks
        for (int i = 0; i < blocks; i++){
        	session.CreateBlock(trialsPerBlock);
		}*/
    }
}
