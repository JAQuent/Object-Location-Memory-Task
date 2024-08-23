using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Collections;
using UnityEngine.Networking;

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
    public Text versionNumber;
    public GameObject backgroundImage;
    public GameObject studyIDInstruction;
    public GameObject studyIDTextField;
    public GameObject studyIDSubmitButton;

    // static vars
    public static string studyID = "None";
    public static string UXF_settings_url = "None";
    public static string fileNameForStartUpText = "None";
    public static bool testFPS = true;

    // JSON Data for UI 
    [Serializable]
    public class UIDataClass {
        public string button1Label;
        public bool button1Show;
        public string button2Label;
        public bool button2Show;
        public string button3Label;
        public bool button3Show;
        public string title;
        public string billboardText;
        public bool testFPS;
    }

    // JSON Data for studyID dictionary for WebGL experiments
    [Serializable]
    public struct Study {
        public string studyID;
        public string UXF_settings_url;
        public string scene;
        public string startupText;
        public bool testFPS;
    }
    [Serializable]
    public class StudyDictClass {
        public List<Study> studies;
    }

    // private vars
    private UIDataClass UIData;
    private StudyDictClass StudyDict;

    // Start is called before the first frame update
    void Start(){
        // Add version number to screen
         versionNumber.text = "Version: " + Application.version;

#if UNITY_WEBGL
        StartCoroutine(SetUp_WebGLExperiment());
        Debug.Log("WebGL build");
#else
        SetUp_LocalExperiment();
#endif
    }

    /// <summary>
    /// Method to load desert arena
    /// </summary>
    public void LoadDesertArena(){
    	Debug.Log("Load desert arena.");
    	SceneManager.LoadScene("desert");
    }

    /// <summary>
    /// Method to load grassy arena
    /// </summary>
    public void LoadGrassyArena(){
    	Debug.Log("Load grassy arena.");
    	SceneManager.LoadScene("grassy");
    }

    /// <summary>
    /// Method to load practice arena
    /// </summary>
    public void LoadPracticeArena(){
        Debug.Log("Load practice arena.");
        SceneManager.LoadScene("practice");
    }

    /// <summary>
    /// Method to submit the study ID, set static variable and load scene.
    /// </summary>
    public void SubmitStudyID(){
        Debug.Log("The Study ID was submitted.");
        studyID = studyIDTextField.GetComponent<InputField>().text;
        Debug.Log("Study ID: " + studyID);

        // Loop through the study dictionary to find the correct study
        foreach (Study study in StudyDict.studies){
            if (study.studyID == studyID){
                UXF_settings_url = study.UXF_settings_url;
                fileNameForStartUpText = study.startupText;
                testFPS = study.testFPS;
                Debug.Log("UXF_settings_url: " + UXF_settings_url);
                Debug.Log("startupText: " + fileNameForStartUpText);
                SceneManager.LoadScene(study.scene);
                return;
            }
        }

        // Throw error if study ID is not found
        string msg = "Study ID not found in study dictionary. Please contact experimenter.";
        Debug.LogError(msg);
        studyIDInstruction.GetComponent<Text>().text = msg;
    }

    /// <summary>
    /// Method to read in the JSON file for UI that is placed in the StreamingAssets folder for the file that is provided
    /// </summary>
    void GetDataFromJSON_UI(string fileName){
        // Get path
        string path2file = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, fileName));

        // Get JSON input
        var sr = new StreamReader(path2file);
        var fileContents = sr.ReadToEnd();
        sr.Close();

        // Get instruction data profile
        UIData = JsonUtility.FromJson<UIDataClass>(fileContents);
    }

    /// <summary>
    /// Method to read in the JSON file contaning the study dictionary for WebGL experiments
    /// </summary>
    void GetDataFromJSON_StudyDict(string fileContents){
        // Get instruction data profile
        StudyDict = JsonUtility.FromJson<StudyDictClass>(fileContents);
    }

    /// <summary>
    /// Method to set up the Welcome UI for local (non-webGL) experiments
    /// </summary>
    void SetUp_LocalExperiment() {
        // Get the information from the JSON file about the UI
        GetDataFromJSON_UI(fileName);

        // Change texts
        button1Text.text = UIData.button1Label;
        button2Text.text = UIData.button2Label;
        button3Text.text = UIData.button3Label;
        title.text = UIData.title;
        billboard.text = UIData.billboardText;
        backgroundImage.SetActive(true);

        // Activate or deactivate buttons
        button1.SetActive(UIData.button1Show);
        button2.SetActive(UIData.button2Show);
        button3.SetActive(UIData.button3Show);

        // Set static variable
        testFPS = UIData.testFPS;
    }

    /// <summary>
    /// Method to set up the Welcome UI for WebGL experiments
    /// </summary>
    private IEnumerator SetUp_WebGLExperiment(){
        // Show loading text
        studyIDInstruction.SetActive(true);
        studyIDInstruction.GetComponent<Text>().text = "Please wait for settings to load...";

        //////////////////// Load URL from StreamingAssets
        // Get the URL for the dictionary of studies
        string StudyDictURLPath = Path.Combine(Application.streamingAssetsPath, "study_dict_url.txt");

        // download file from StreamingAssets folder
        UnityWebRequest www = UnityWebRequest.Get(StudyDictURLPath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error downloading study_dict_url.txt file: " + www.error);
            yield break;
        }
        
        // Get the URL to download from
        string StudyDictURL = www.downloadHandler.text;
        Debug.Log("Downloading Study Dictionary file from: " + StudyDictURL);

        //////////////////// Download the study dictionary
        // download file from the internet
        www = UnityWebRequest.Get(StudyDictURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error downloading Study Dictionary file: " + www.error);
            yield break;
        }

        // Get the URL to download from
        string StudyDictContent = www.downloadHandler.text;

        // Get information from the JOSN about the studies
        GetDataFromJSON_StudyDict(StudyDictContent);

        // Enable relevant UI elements at the end of participant don't submit to early.
        studyIDInstruction.GetComponent<Text>().text = "Please type in the study ID that we gave you earlier. ";
        studyIDTextField.SetActive(true);
        studyIDSubmitButton.SetActive(true);
    }
}
