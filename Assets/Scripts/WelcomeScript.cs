using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class WelcomeScript : MonoBehaviour{
	// public vars
	public Toggle m_Toggle;
    public string fileName = "welcome.json";
    public GameObject button1;
    public Text button1Text;
    public GameObject button2;
    public Text button2Text;
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
        public string title;
        public string billboardText;
    }

    // private vars
    private JSONDataClass JSONData;

    // Start is called before the first frame update
    void Start(){
        // Get the information from the JSON file
        GetDataFromJSON(fileName);
        Debug.Log(JSONData.button1Label);
        Debug.Log(JSONData.button1Show);
        Debug.Log(JSONData.button2Label);
        Debug.Log(JSONData.button2Show);
        Debug.Log(JSONData.title);
        Debug.Log(JSONData.billboardText);

        // Change texts
        button1Text.text = JSONData.button1Label;
        button2Text.text = JSONData.button2Label;
        title.text = JSONData.title;
        billboard.text = JSONData.billboardText;

        // De/activate buttons
        button1.SetActive(JSONData.button1Show);
        button2.SetActive(JSONData.button2Show);

        //Add listener for when the state of the Toggle changes, and output the state
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });

        //Initialize the Text to say whether the Toggle is in a positive or negative state
        language = "chinese";
    }

    void Update(){
        // Check if secret button is pressed. 
        if(Input.GetKeyDown(KeyCode.LeftBracket)){
            Debug.Log("Super secret mode");
            SceneManager.LoadScene("videoScene");
        }
    }

    /// <summary>
    /// Output the new state of the Toggle into Text when the user uses the Toggle
    /// </summary>
    void ToggleValueChanged(Toggle change){
    	if(m_Toggle.isOn){
    		Debug.Log("Language set to Chinese.");
    		language = "chinese";
    	} else {
    		Debug.Log("Language set to English.");
    		language = "english";
    	}
    }

    /// <summary>
    /// Method to load practice scene
    /// </summary>
    public void LoadPracticeVersion(){
    	Debug.Log("Load square scene.");
    	SceneManager.LoadScene("square");
    }

    /// <summary>
    /// Method to load main task scene
    /// </summary>
    public void LoadfMRIVersion(){
    	Debug.Log("Load arena scene.");
    	SceneManager.LoadScene("arena");
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
