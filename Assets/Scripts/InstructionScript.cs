using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class InstructionScript : MonoBehaviour{
	// Input texts
	public Text startUp;
	public Text instructions1;
	public Text instructions2;
	public string fileNameForInstructions = "instructions.json";

	// Get all variables set up that you can to get from the .json file.
	// The variable names have to correspond to the input names in that file. 
	[System.Serializable]
    public class InstructionData {
    	public string language;
    	public string startUp_eng;
        public string instructions1_eng;
        public string instructions2_eng;
        public string startUp_cn;
        public string instructions1_cn;
        public string instructions2_cn;
    }


    // Start is called before the first frame update
    void Start(){
        // Get path
        string path2file = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, fileNameForInstructions));

        // Get JSON input
        var sr = new StreamReader(path2file);
        var fileContents = sr.ReadToEnd();
        sr.Close();

        // Get instruction data profile
        InstructionData Profile = JsonUtility.FromJson<InstructionData>(fileContents);

        // Change the instructions accordingly
        if(Profile.language == "chinese"){
        	startUp.text = Profile.startUp_cn;
        	instructions1.text = Profile.instructions1_cn;
        	instructions2.text = Profile.instructions2_cn;
        } else {
        }
    }
}
