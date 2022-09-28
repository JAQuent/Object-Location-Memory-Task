using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeSkyboxTest : MonoBehaviour{
	// Variables
	public Material[] secondSkybox;
	public bool changeSkybox = false;
	public int i = 0;

    // Update is called once per frame
    void Update(){
    	if(changeSkybox){
    		RenderSettings.skybox = secondSkybox[i];
    		changeSkybox = false;
    	}   
    }
}
