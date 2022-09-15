using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class WelcomeScript : MonoBehaviour{
	// public vars
    public string fileName = "welcome.json";
    public GameObject button1;
    public Text button1Text;
    public GameObject button2;
    public Text button2Text;
    public GameObject button3;
    public Text button3Text;
    public Text billboard;
    public Text title;

	// Static
	public static string language;

    // You need to set-up all variables that you want to get from the .json file.
    // The variable names have to correspond to the input names in that file. 
    [System.Serializable]
    public class JSONDataClass {
        public string button1Label;
        public bool button1Show;
        public string button2Label;
        public bool button2Show;
        public string button3Label;
        public bool button3Show;
        public string title;
        public string billboardText;
    }

    // private vars
    private JSONDataClass JSONData;

    // Start is called before the first frame update
    void Start(){
        // Get the information from the JSON file
        GetDataFromJSON(fileName);

        // Change texts
        button1Text.text = JSONData.button1Label;
        button2Text.text = JSONData.button2Label;
        button3Text.text = JSONData.button3Label;
        title.text = JSONData.title;
        billboard.text = JSONData.billboardText;

        // De/activate buttons
        button1.SetActive(JSONData.button1Show);
        button2.SetActive(JSONData.button2Show);
        button3.SetActive(JSONData.button2Show);
    }

    void Update(){
        // Check if secret button is pressed. 
        if(Input.GetKeyDown(KeyCode.LeftBracket)){
            Debug.Log("Super secret mode");
            SceneManager.LoadScene("videoScene");
        }
    }

    /// <summary>
    /// Method to load desert arena
    /// </summary>
    public void LoadDesertArena(){
    	Debug.Log("Load desert arena.");
    	SceneManager.LoadScene("square");
    }

    /// <summary>
    /// Method to load grassy arena
    /// </summary>
    public void LoadGrassyArena(){
    	Debug.Log("Load grassy arena.");
    	SceneManager.LoadScene("arena");
    }

    /// <summary>
    /// Method to load practice arena
    /// </summary>
    public void LoadPracticeArena(){
        Debug.Log("Load practice arena.");
        SceneManager.LoadScene("practiceEnvironment");
    }

    /// <summary>
    /// Method to read in the JSON file that is placed in the StreamingAssets folder for the file that is provided
    /// </summary>
    void GetDataFromJSON(string fileName){
        // Get path
        string path2file = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, fileName));

        // Get JSON input
        var sr = new StreamReader(path2file);
        var fileContents = sr.ReadToEnd();
        sr.Close();

        // Get instruction data profile
        JSONData = JsonUtility.FromJson<JSONDataClass>(fileContents);
    }
}
