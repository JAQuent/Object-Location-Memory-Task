using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using UXF.UI;
using UnityEngine.Networking;
using System;

//// This script can be attaced to any Game object and will controll the text of the UXF start up panel.
public class UXF_UI_controllScript : MonoBehaviour{
	// Public vars
	public string fileNameForStartUpText = "startupText.json";

    // Get script from other object
    public UIController UIControllerScript; // Add the UIControllerScript object. 

    // You need to set-up all variables that you want to get from the .json file.
    // The variable names have to correspond to the input names in that file. 
    [Serializable]
    public class JSONDataClass {
    	public string chromeBar;
    	public string instructionsPanelContent1;
        public string instructionsPanelContent2;
        public string expSettingsprofile;
        public string localPathElement;
        public string localPathElement_placeHolder;
        public string participantID;
        public string participantID_placeholder;
        public string sessionNumber;
        public string termsAndConditions;
        public string beginButton;
    }

	// Private vars
	private Text chromeBar; //"Startup";
    private Text instructionsPanelContent1; // Heading
	private Text instructionsPanelContent2; // Main text
	private Text expSettingsprofile;  //"Experiment settings profile";
    private Text localPathElement;
	private Text localPathElement_placeHolder;
	private Text participantID;
	private Text participantID_placeholder;
	private Text sessionNumber;
	private Text beginButton;
    private Text termsAndConditions;
	private JSONDataClass JSONData;

    // Start is called before the first frame update
    void Start(){
#if UNITY_WEBGL
        fileNameForStartUpText = WelcomeScript.fileNameForStartUpText;
        if(fileNameForStartUpText == "None"){
            Debug.LogError("Error: fileNameForStartUpText is not set. Please set this in the study_dict.json file.");
        } else{
            StartCoroutine(SetUp_UI_WebGLExperiment());
        }
        UIControllerScript.jsonURL = WelcomeScript.UXF_settings_url;
        UIControllerScript.experimentName = WelcomeScript.studyID;
        Debug.Log("JSON URL set: " + UIControllerScript.jsonURL);
#else
        SetUp_UI_LocalExperiment();
#endif
    }

    /// <summary>
    /// Method to read in the JSON file that is placed in the StreamingAssets folder for the file that is provided
    /// </summary>
    void GetDataFromJSON(string fileContents){
        // Get instruction data profile
        JSONData = JsonUtility.FromJson<JSONDataClass>(fileContents);
    }

    /// <summary>
    /// Method to set-up UI for non-WebGL experiments
    /// </summary>
    void SetUp_UI_LocalExperiment(){
        // Get path
        string path2file = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, fileNameForStartUpText));

        // Get JSON input
        var sr = new StreamReader(path2file);
        string fileContents = sr.ReadToEnd();
        sr.Close();

        // Get the Startup Text from the JSON file
        GetDataFromJSON(fileNameForStartUpText);
    
        // Change the UI elements
        ChangeUI();
    }
    /// <summary>
    /// Method to set-up UI for WebGL experiments
    /// </summary>
    private void ChangeUI(){
        // Go through all elements and update the text if it is not empty
        //"Startup";
        if (JSONData.chromeBar != ""){
            try{
                chromeBar = GameObject.Find("Chrome Bar/Text").GetComponent<Text>();
                chromeBar.text = JSONData.chromeBar;
            }
            catch (Exception){
                print("Error: The option for chromeBar is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }

        }

        //"Welcome to UXF! ";
        if (JSONData.instructionsPanelContent1 != ""){
            try{
                instructionsPanelContent1 = GameObject.Find("Instructions Panel Content/Text (1)").GetComponent<Text>();
                instructionsPanelContent1.text = JSONData.instructionsPanelContent1;
            }
            catch (Exception){
                print("Error: The option for instructionsPanelContent1 is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }

        //"You could use this space to display some instructions to the researcher or the participant.";
        if (JSONData.instructionsPanelContent2 != ""){
            try{
                instructionsPanelContent2 = GameObject.Find("Instructions Panel Content/Text (2)").GetComponent<Text>();
                instructionsPanelContent2.text = JSONData.instructionsPanelContent2;
            }
            catch (Exception){
                print("Error: The option for instructionsPanelContent2 is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }

        //"Experiment settings profile";
        if (JSONData.expSettingsprofile != ""){
            try{
                expSettingsprofile = GameObject.Find("SettingsElement/Title Box/Title").GetComponent<Text>();
                expSettingsprofile.text = JSONData.expSettingsprofile;
            }
            catch (Exception){
                print("Error: The option for expSettingsprofile is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }

        //"Local data save directory";
        if (JSONData.localPathElement != ""){
            try{
                localPathElement = GameObject.Find("LocalPathElement/Title Box/Title").GetComponent<Text>();
                localPathElement.text = JSONData.localPathElement;
            }
            catch (Exception){
                print("Error: The option for localPathElement is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }

        //"Press browse button to select...";
        if (JSONData.localPathElement_placeHolder != ""){
            try{
                localPathElement_placeHolder = GameObject.Find("LocalPathElement/InputField/Placeholder").GetComponent<Text>();
                localPathElement_placeHolder.text = JSONData.localPathElement_placeHolder;
            }
            catch (Exception){
                print("Error: The option for localPathElement_placeHolder is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }

        //"Participant ID";
        if (JSONData.participantID != ""){
            try{
                participantID = GameObject.Find("PPIDElement/Title Box/Title").GetComponent<Text>();
                participantID.text = JSONData.participantID;
            }
            catch (Exception){
                print("Error: The option for participantID is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }

        //"Enter text...";
        if (JSONData.participantID_placeholder != ""){
            try{
                participantID_placeholder = GameObject.Find("PPIDElement/InputField/Placeholder").GetComponent<Text>();
                participantID_placeholder.text = JSONData.participantID_placeholder;
            }
            catch (Exception){
                print("Error: The option for participantID_placeholder is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }

        //"Session number";
        if (JSONData.sessionNumber != ""){
            try{
                sessionNumber = GameObject.Find("SessionNumDropdown/Title Box/Title").GetComponent<Text>();
                sessionNumber.text = JSONData.sessionNumber;
            }
            catch (Exception){
                print("Error: The option for sessionNumber is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }

        //"Please tick if you understand the instructions and agree for your data to be collected and used for research purposes.<color=red>*</color>";
        if (JSONData.termsAndConditions != ""){
            try{
                // This has to be set-up in the UIController script because it would be overwritten otherwise.
                var termsAndConditionsVar = GameObject.Find("[UXF_UI]").GetComponent<UIController>();
                termsAndConditionsVar.termsAndConditions = JSONData.termsAndConditions;
                termsAndConditions = GameObject.Find("Terms And Conditions Text").GetComponent<Text>();
                termsAndConditions.text = JSONData.termsAndConditions;
            }
            catch (Exception){
                print("Error: The option for termsAndConditions is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }

        //"Begin session";
        if (JSONData.beginButton != ""){
            try{
                beginButton = GameObject.Find("Begin Button/Text").GetComponent<Text>();
                beginButton.text = JSONData.beginButton;
            }
            catch (Exception){
                print("Error: The option for beginButton is not enabled. Therefore, we can't change the text for this UI element. Set to empty string.");
            }
        }
    }

    /// <summary>
    /// Method to set-up UI for WebGL experiments
    /// </summary>
    private IEnumerator SetUp_UI_WebGLExperiment(){
        //////////////////// Load URL from StreamingAssets
        // Get the URL for the dictionary of studies
        string StartUpTextURL = fileNameForStartUpText;

        // download file from StreamingAssets folder
        UnityWebRequest www = UnityWebRequest.Get(StartUpTextURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error downloading StartUpText file: " + www.error);
            yield break;
        }

        // Get the URL to download from
        string fileContents = www.downloadHandler.text;

        // Get instruction data profile
        JSONData = JsonUtility.FromJson<JSONDataClass>(fileContents);

        // Change the UI elements
        ChangeUI();
    }
}
