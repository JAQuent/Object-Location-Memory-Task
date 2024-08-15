using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UXF; 
using System.IO;
using UnityEngine.SceneManagement;

// Description of this task phases:
// Encoding & control: Cue -> delay -> collect -> ITI. 
// Retrieval: Cue -> delay -> place, feedback, collect -> ITI. 

public class ExperimentController : MonoBehaviour{
    // Public static vars
    public static float runStartTime; 
    public static bool confirm = false;
    public static bool ready2confirm = false;
    public static int soundMode = 1; // Controls which sounds are played. This is parsed from the settings .json file
    // Sound mode explanation:
    // 1 = all (object & message sound) 
    // 2 = message only
    // 3 = none 
    public static bool sessionStarted = false; // Has the session started yet?
    public static string measuredFPS;

	// reference to the UXF Session 
    public Session session; 

    // HTTPpost script
    public UXF.HTTPPost HTTPPostScript; // Add the HTTPPostScript from the [UXF_DataHandling] object. 
    public ResponsePixxInterface responsePixxScript; // Add the responsePixxScript. This is also attached to the Experiment game object.  

    // End screen
    public GameObject endScreen; // The game object the controls the end screen. 

    // reference objects that are needed to implement control trials
    public GameObject background; // All objects that are part of the background so they can be easily disabled. 
    public GameObject mainSun;    // Main sun that changes from an angle used during encoding/retrieval trials.
    public GameObject controlSun; // Sun that is used during control trials shines orthogonal to the plane to avoid spatial cues. 
    public Material[] skyboxes; // First is the main skybox and the second is the skybox for control trials
    public GameObject reflectionProbe; // Since the reflection probes are baked now, they have to be disabled on control trials. 

	// Public vars
	public List<GameObject> objects;
    public List<Sprite> objectsImages;
	public GameObject arrowPrefab;
	public KeyCode startButton = KeyCode.S;
	public KeyCode confirmButton = KeyCode.E;
    public int target;
    public Image cueImage;
    public Image constantCueImage;
    public Text blockMessageText;
    public GameObject panel;
    public GameObject fixationMarker;
    public GameObject player;
    public float distance;
    public List<int> timesObjectPresented; // How often have this object been presented?
    public GameObject warning; // Warning game object
    public AudioClip endSound; // Sound played when a message is played. This can for instance indicate the end of a block/run. 
    public Image feedbackImage;
    public List<Sprite> feedbackIcons;

	// Private vars
	private int trialNum;
	private int blockNum;
	private GameObject arrow; // The arrow that bounces over the object. 
    private Trial trial;
    private Block block;
    private Vector2 endPosition;  // Where did the player end?
    private Vector2 targetPosition; // Where was the target object?
    private bool buttonPressed = false;
    private float navStartTime; // Start of the navigation period
    private float navEndTime; // End of the navigation period
    private float cueTime; // How long is the cue present?
    private float delay; // How long is the delay?
    private float ITI; // How long is the ITI?
    private string trialType; // Type of the current trial (i.e. standard vs. control)
    private bool drawMessageNextTrial = true; // Should a message be shown on the next trial?
    private bool closeMessage1 = false; // Will switch to true if the experimenter presses SPACE
    private bool closeMessage2 = false; // Will switch to true if the letter S is pressed or the scanner sends and S. 
    private bool trialEnded = true;
    private bool messageDrawn = false; // Was the message drawn yet?
    private string message2draw; // Should a message be drawn after this trial?
    private List<string> blockMessages; // List with the block messages displayed after each block. 
    private string waitForExperimenter; // String for screen that will be presented for each block message. 
    private float start_x; // Start position of the player
    private float start_z; // Start position of the player 
    private float start_yRotation; // Start rotation of the player
    private float object_x; // Object position
    private float object_z; // object position
    private GameObject currentObject; // the current object of the trial
    private bool experimentStarted = false; // Bool if the experiment has been started by pressing S or scanner send S
    private int messageToDisplay; // Should a message be displayed after the current trial (-1 = no). If yes, then value
    // is > 0 and can be used as an index for blockMessage
    private bool displayingBackground = true; // Is the background (including skybox) currenly displayed?
    private bool runActive = false; // Only if this is not true, we change runStartTime 
    private float confirmButtonTime = float.NaN; 
    private bool startEndCountDown; // If true it starts the end countdown
    private float endCountDown = 60; // End countdown if zero, application closes. 
    private Text endScreenText; // Text component of the end screen
    private string endMessage; // String for the end message that is used.
    private bool useHTTPPost = false; // Is HTTPPost to be used? If so it needs input from the .json
    private bool continuousMode = false; // To be parsed from the .json file. If true, it enables the mode with
    // no cue, delay and ITI, which was created to shoot a video. 
    private float movedDistance = float.NaN; // Variable to save the distance from the start location to the location where the
    // confirm button was pressed.
    private Vector2 startPosition;  // Position where the player started. 
    private float warningCriterium = 4.0f; // If the participant moved less than this, then a warning is shown.
    private bool warningShown = false;
    private List<string> objectNames; // Object names
    private GameObject FPS_Counter; // Game object for FPS counter
    private bool foundFPS_Counter = false; // Has the FPS counter been found?
    private bool inCueOrDelayPeriod = false; // Is the task curerntly in the cue delay period? This is used to ignore the confirm button press.
    private float objectRotationSpeed = 0; // Speed with which the object should rotated around the y-axis.
    private bool feedbackEnabled = false; // Boolean that decides if feedback should be shown (parsed from .json).
    private float feedback_critVal1; // The criteria used to display feedback (parsed from .csv).
    private float feedback_critVal2; // The criteria used to display feedback (parsed from .csv).
    private bool showConstantCue = false; // Boolen that decied if a constant cue is presented during the trial (parsed from .json).
    private List<int> blocks2shuffle; // Contains the blocks that need to be shuffled
    private bool need2shuffle = false;
    private bool lastTrial_inBlockMessage = false; // Determines the mode how messages are displayed. If true, then the last trial of 
    // a block always displays a message and messageToDisplay in the .csv is ignored. If false, then the functionality is controlled by
    // messageToDisplay in the .csv. 
    private bool showProgressBar = false; // Should the progress bar be shown?
    private float totalNumTrials; // Total number of trials in the session.

    // Excuted at the beginging
    void Start(){
        // Initialise timesObjectPresented
        for(int i = 0; i < objects.Count; i++){
            timesObjectPresented.Add(0);
        }

        // Activate FPS by default
        activateFPS_Counter(false);
    }

    // Update is called once per frame
    void Update(){
    	// Start first trial if not in trial already, the session is started and when the experiment is not started yet
    	// These conditions are important so that the programm doesn't attempt to start the experiment at the wrong time.
        if(Input.GetKey(startButton) & !session.InTrial & sessionStarted & !experimentStarted){
        	startExperiment();
        }

        // Only run the function if a) the confirm button is pressed or confirm is true (e.g. set by other script), 
        // b) the experiment is in trial and the button hasn't been pressed already and the trial type is "retrieval" & 
        // not during cue and delay.
        ready2confirm = session.InTrial & !buttonPressed & trialType == "retrieval" & !inCueOrDelayPeriod;
        if((Input.GetKey(confirmButton) | confirm) & ready2confirm){
        	locationRetrieved();
        }

        // Wait for space bar press to move to message screen
		if(Input.GetKeyDown(KeyCode.Space) & !closeMessage1 & trialEnded){
        		closeMessage1 = !closeMessage1;
        		Debug.Log("Experimenter pressed space bar.");
        }

        // Close message if it is drawn and the startButton is pressed. 
        if(drawMessageNextTrial & messageDrawn){
            if(Input.GetKeyDown(startButton)){
                closeMessage2 = !closeMessage2;
                Debug.Log("Close message!");
            }
        }

        // Stop Experiment
        if(Input.GetKey(KeyCode.Escape)){
            // Log entry
            Debug.Log("Session end time " + System.DateTime.Now);

            // Close application
            TheEnd();
        }

        // Log entry for each startButton pressed after the experiment started
        if(Input.GetKeyDown(startButton) & experimentStarted){
        	logTrigger();
        }

        // End countdown
        if(startEndCountDown){
            endCountDown -= Time.deltaTime;
            endScreenText.text = endMessage + Mathf.Round(endCountDown);

            // Quit if end count down over
            if(endCountDown <= 0){
                Application.Quit();
            }
        }
    }

    /// <summary>
    /// Method to start the experiment, which is mainly waiting for the S (i.e. the trigger from the scanner) to arrive. 
    /// </summary>
    void startExperiment(){
        // Log entry
        Debug.Log("Start message send");

        // Start experiment and log time
        runStartTime = Time.time; 
        Debug.Log("Run start " + System.DateTime.Now + " runStartTime: " + runStartTime);

        // Set experiment started to true
        experimentStarted = true;

        // Check if shuffling is necessary
        if (need2shuffle){
            Debug.Log("Session number " + session.blocks.Count);
            ShuffleBlocks(session);
            need2shuffle = false; // Set to false so it is not done again.
        }

        // Begin first trial
        session.BeginNextTrial(); 
    }

    /// <summary>
    /// Method to retrieve the location when the correct button was pressed.  
    /// </summary>
    void locationRetrieved(){
        // Log entry
        Debug.Log("Confirm button pressed: trial " + trialNum);

        // Get time point
        confirmButtonTime = Time.time;

        // Change button to true so this function won't be called again (see conditions above).
        buttonPressed = true; 

    	// Show object as feedback
    	currentObject.SetActive(true);

        // Get position of object
        arrow.SetActive(true);

        // Create Vector2 for player position
        endPosition = new Vector2(player.transform.position.x, player.transform.position.z);

        // Create vector2 for target position
        targetPosition = new Vector2(currentObject.transform.position.x, currentObject.transform.position.z);

        // Calculate Euclidean distance
        distance = Vector2.Distance(endPosition, targetPosition); 

        // Start position as vector 2
        movedDistance = Vector2.Distance(endPosition, startPosition); 

        // Check if warning has to be distplayed
        if(movedDistance <= warningCriterium){
            StartCoroutine(showWarning());
        }

        // Provide feedback if enabled
        if(feedbackEnabled){
            // Get current feedback values
            feedback_critVal1 = trial.settings.GetFloat("feedback_critVal1");
            feedback_critVal2 = trial.settings.GetFloat("feedback_critVal2");

            if(distance <= feedback_critVal1){
                StartCoroutine(showFeedback(2));
            } else if(distance > feedback_critVal1 & distance <= feedback_critVal2){
                StartCoroutine(showFeedback(1));
            } else if(distance > feedback_critVal2){
                StartCoroutine(showFeedback(0));
            }
        }

        // Reset at the end
        confirm = false; // reset confirm if this was set by different script. 
    }

    /// <summary>
    /// Present warning of a short time.
    /// </summary>
    IEnumerator showWarning(){
        // Log entry
        Debug.Log("Movement warning start. Trial " + trialNum);

        // Warning was shown
        warningShown = true;

        // Activate warning
        warning.SetActive(true);

        // Wait until warning time is over 
        yield return new WaitForSeconds(2.0f);   

        // Activate warning
        warning.SetActive(false);

        // Log entry
        Debug.Log("Movement warning end. Trial " + trialNum);
    }


    /// <summary>
    /// Method to show feedback on the centre of the screen.
    /// </summary>
    IEnumerator showFeedback(int feedbackLevel){
        if(feedbackLevel == 0){
            Debug.Log("Negative feedback given.");
        } else if(feedbackLevel == 1){
            Debug.Log("Neutral feedback given.");
        } else if(feedbackLevel == 2){
            Debug.Log("Positive feedback given.");
        } else {
            Debug.Log("Error: unknown feedback");
        }

        // Change sprite and enable
        feedbackImage.sprite = feedbackIcons[feedbackLevel];
        feedbackImage.enabled = true;

        // Wait until the time is over 
        yield return new WaitForSeconds(2.0f);   

        // Disable sprite
        feedbackImage.enabled = false;
    }


    /// <summary>
    /// Method to log everytime an S arrives (i.e. a trigger from the scanner).
    /// If approbriate it will also reset the run start time.   
    /// </summary>
    void logTrigger(){
        // Log every trigger 
    	Debug.Log("A trigger was send " + System.DateTime.Now + " Run time: " + Time.time);

    	// Change runStartTime only when run is not active
        if(!runActive){
            // Record new time
            runStartTime = Time.time; 
            Debug.Log("Run start " + System.DateTime.Now + " runStartTime: " + runStartTime);

            // Set run active to true
            runActive = true;
        }
    }

    /// <summary>
    /// Method to start the session. This needs to be attached to the On Session Begin Event of the UXF Rig.
    // This a) logs the screen resoltion & b) locks the target frame rate to that is provided by the .json file. 
    /// </summary>
    public void sessionStart(){
        // Log entry
        Debug.Log("Session start time " + System.DateTime.Now);
        // Screen resolution
        Debug.Log(Screen.currentResolution);
        // Scene name
        Debug.Log("The scene used: " + SceneManager.GetActiveScene().name);
        // Version of the task
        Debug.Log("Application Version : " + Application.version);
        // measuredFPS
        Debug.Log("The measured FPS was: " + measuredFPS);
        // studyID
        Debug.Log("The studyID was: " + WelcomeScript.studyID);
        // UXF_settings_url
        Debug.Log("The UXF_settings_url was: " + WelcomeScript.UXF_settings_url);

        // Set bool to true
        sessionStarted = true;

        // Set frame rate to maximum
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = session.settings.GetInt("targetFrameRate");

        // Check if it is continuous mode
        continuousMode = session.settings.GetBool("continuousMode");;

        // Get endCountDown
        endCountDown = session.settings.GetFloat("endCountDown");

        // Get warningMessage and the criterium
        string warningMessage = session.settings.GetString("warningMessage");
        warning.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().text = warningMessage;
        warningCriterium = session.settings.GetFloat("warningCriterium");

        // Check if the keys have to be changed
        bool changeKeys = session.settings.GetBool("changeKeys");
        if(changeKeys){
            changeKeyboardKeys();
        }

        // Check if HTTPPost needs to be set.
        useHTTPPost = session.settings.GetBool("useHTTPPost");
        if(useHTTPPost){
            configureHTTPPost();
        }

        // Check if responsePixx should be used
        if(session.settings.GetBool("useResponsePixx")){
            responsePixxScript.enabled = true;
        }

        // Log which platform
        whichPlatform();

        // Detect which input devices are presented
        detectInputDevices();

        // Get which sound mode should be used.
        soundMode = session.settings.GetInt("soundMode");

        // Activate FPS counter if configured so.
        activateFPS_Counter(session.settings.GetBool("showFPS"));

        ////////////////////////////////////////////////////////////////////////////////////
        // Newly added features that if not specified in the .json file get a default value.
        // Check for objectRotationSpeed
        string tempKey = "objectRotationSpeed";
        if(containsThisKeyInSessionSettings(tempKey)){
            objectRotationSpeed = session.settings.GetFloat(tempKey);
        } else {
            objectRotationSpeed = 0.0f;
        }

        // Check if feedbackvalues are provided
        tempKey = "showFeedback";
        if(containsThisKeyInSessionSettings(tempKey)){
            feedbackEnabled = session.settings.GetBool(tempKey);
        } else {
            feedbackEnabled = false;
        }

        // Check if feedbackvalues are provided
        tempKey = "showConstantCue";
        if(containsThisKeyInSessionSettings(tempKey)){
            showConstantCue = session.settings.GetBool(tempKey);
        } else {
            showConstantCue = false;
        }

        // Check whether any trials in blocks need to be shuffled.
        tempKey = "shuffleBlocks";
        if(containsThisKeyInSessionSettings(tempKey)){
            blocks2shuffle = session.settings.GetIntList(tempKey);
            Debug.Log("Number of blocks that will be shuffled: " + blocks2shuffle.Count);
            need2shuffle = true;
        } 

        // Check whether actionNeedToBeEnded should be controlled.
        tempKey = "actionNeedToBeEnded";
        if(containsThisKeyInSessionSettings(tempKey)){
            ThreeButtonMovement.actionNeedToBeEnded = session.settings.GetBool(tempKey); 
        }

        // Check whether the last trial in the block (true) or the .csv (false) drives the messages.
        tempKey = "lastTrial_inBlockMessage";
        if(containsThisKeyInSessionSettings(tempKey)){
            lastTrial_inBlockMessage = session.settings.GetBool(tempKey); 
        }

        // Check if a progress bar should be shown. 
        tempKey = "showProgressBar";
        if (containsThisKeyInSessionSettings(tempKey)){
            showProgressBar = session.settings.GetBool(tempKey);
            ProgressBar.ActivateProgressBar(showProgressBar);
        }
    }

    /// <summary>
    /// Method to check if a certain key can be found in the settings heirarchy
    /// </summary>  
    bool containsThisKeyInSessionSettings(string targetKey){
        // Initialise result
        bool contained = false;

        // Loop through all keys in settings
        foreach(var key in session.settings.Keys){
            if(key == targetKey){
                contained = true;
                break;
            }
        }

        // Retun the value
        return(contained);
    }

    /// <summary>
    /// Method to change keys 
    /// </summary>  
    void changeKeyboardKeys(){
        // Get List of string from .json
        List<string> newKeys = session.settings.GetStringList("keys");
        Debug.Log("Number of keys: " + newKeys.Count);
        ThreeButtonMovement.leftTurn = (KeyCode) System.Enum.Parse(typeof(KeyCode), newKeys[0]);
        ThreeButtonMovement.forwardKey = (KeyCode) System.Enum.Parse(typeof(KeyCode), newKeys[1]);
        ThreeButtonMovement.rightTurn = (KeyCode) System.Enum.Parse(typeof(KeyCode), newKeys[2]);
        confirmButton = (KeyCode) System.Enum.Parse(typeof(KeyCode), newKeys[3]);
    }

    /// <summary>
    /// Method to configure the HTTPPost script. Needs public UXF.HTTPPost HTTPPostScript;
    /// </summary>
    void configureHTTPPost(){
        string url = session.settings.GetString("url");
        string username = session.settings.GetString("username");
        string password = session.settings.GetString("password");

        // Set the variables
        HTTPPostScript.url = url;
        HTTPPostScript.username = username;
        HTTPPostScript.password = password;
        HTTPPostScript.active = true;
    }

    /// <summary>
    /// Method to activate FPS counter
    /// </summary>
    void activateFPS_Counter(bool activateFPS_Recording){
        // Find the game object
        if(!foundFPS_Counter){
            FPS_Counter = GameObject.Find("FPS_Counter");
            foundFPS_Counter = true; // Found the game object
        }

        // Set in/active
        FPS_Counter.SetActive(activateFPS_Recording);
    }


    void getSettingsForCurrentTrial(){
        // Get current trial and block number
        trialNum =  session.currentTrialNum; 
		blockNum =  session.currentBlockNum;

		// Get current trial
		trial = session.CurrentTrial;
        block = session.CurrentBlock;

		// Get current trial values
		target = trial.settings.GetInt("targets");
		delay = trial.settings.GetFloat("delay");
		cueTime = trial.settings.GetFloat("cue");
		ITI = trial.settings.GetFloat("ITI");
		trialType = trial.settings.GetString("trialType");

        // Ccheck if a message needs to be displayed
        // See above for details on lastTrial_inBlockMessage
        if(lastTrial_inBlockMessage){
            // Check if the current trial is the last trial of the block
            if(trial == block.lastTrial){
                Debug.Log("This is the last trial of block " + blockNum);
                messageToDisplay = blockNum - 1;
            } else {
                messageToDisplay = -1; // If it is not the last trial, don't display message. 
            }
        } else {
            messageToDisplay = trial.settings.GetInt("messageToDisplay");
        }

		// Set movement param
		ThreeButtonMovement.forwardSpeed = trial.settings.GetInt("speedForward");
		ThreeButtonMovement.rotationSpeed = trial.settings.GetInt("rotationSpeed");

		// Get position and rotation for this trial
		start_x = trial.settings.GetFloat("start_x");
		start_z = trial.settings.GetFloat("start_z");
		start_yRotation = trial.settings.GetFloat("start_yRotation");

		// Get object positions
		object_x = trial.settings.GetFloat("object_x");
		object_z = trial.settings.GetFloat("object_z");
    }

    /// <summary>
    /// Method to spawn the object. It is set inactive immediately and later set active.
    /// </summary>
    void spawnObject(){
		// Instantiate object
        currentObject = Instantiate(objects[target - 1]);
        currentObject.SetActive(false);
        currentObject.name = "Object" + target;
		currentObject.transform.position = new Vector3(object_x, currentObject.transform.position.y, object_z);

        // Set rotation value
        //currentObject.GetComponent<objectScript>().rotationSpeed = objectRotationSpeed;
        objectScript.rotationSpeed = objectRotationSpeed;
    }

    /// <summary>
    /// Method to check if a message has to be displayed.
    /// </summary>
    void checkIfMessageNeedsToBeDisplayed(){
        // Check if message should be drawn after the trial.
        // by checking if messageToDisplay is above -1. If not, it means not message.
        if(messageToDisplay >= 0){
            // Set 2 true
            drawMessageNextTrial = true;

            // Select the correct message
            message2draw = blockMessages[messageToDisplay];

        } else {
            drawMessageNextTrial = false;
        }
    }

    /// <summary>
    /// Method to set up a trial. This needs to be attached to the On Trial Begin Event of the UXF Rig.
    /// </summary>
    public void SetUpTrial(){
        // Set run active
        runActive = true;

        // Reset values
        confirmButtonTime = float.NaN;
        movedDistance = float.NaN;
        warningShown = false;
        confirm = false; 

        // Initialise the button press so it can be pressed. 
        buttonPressed = false;

        // Get all necessary information for setting up current trial
        getSettingsForCurrentTrial();

		// Set up background 
		if(trialType == "control" & displayingBackground){
			// For control trials
			RenderSettings.skybox = skyboxes[1];
			background.SetActive(false);
			displayingBackground = false;
            mainSun.SetActive(false);
            controlSun.SetActive(true);
            reflectionProbe.SetActive(false);
            RenderSettings.sun = controlSun.GetComponent<UnityEngine.Light>();
		} else if (trialType != "control" & !displayingBackground){
			// For normal encoding & retrieval trials
			RenderSettings.skybox = skyboxes[0];
			background.SetActive(true);
			displayingBackground = true;
            mainSun.SetActive(true);
            RenderSettings.sun = mainSun.GetComponent<UnityEngine.Light>();
            controlSun.SetActive(false);
            reflectionProbe.SetActive(true);
		}

		// Spawn the object & set inactive at first
		spawnObject();

		// Reset movement so that player is stationary at the beginning
        ThreeButtonMovement.reset = true;
		
		// Show cue
		StartCoroutine(ShowCue(cueTime));

		// During encoding & on control trials show the object immediately
		if(trialType == "encoding" | trialType == "control"){
			currentObject.SetActive(true);
		} 

        // Get position of current object 
        Vector3 objPos = currentObject.transform.position;

        // Add arrow to indicate where object is and move over to correcy location
        arrow = Instantiate(arrowPrefab);
        arrow.transform.position = new Vector3(objPos.x, arrow.transform.position.y, objPos.z);

        // For trials that retrieval
        if(trialType == "retrieval"){
            arrow.SetActive(false);
        }

        // Set player position/rotation only if it is not the continuous mode or trial 1 where even if it is this mode
        // the player position/rotation has to be set.
        if(!continuousMode | trialNum == 1){
        		// Set player position and rotation
				player.transform.position = new Vector3(start_x, 1.0f, start_z);
				player.transform.rotation = Quaternion.Euler(0, start_yRotation, 0);
        }

        // Get start position for the trial
        startPosition = new Vector2(player.transform.position.x, player.transform.position.z);

		// Check if a message needs to be displayed at the end of the trial
		checkIfMessageNeedsToBeDisplayed();

        // Set trial ended to false
        trialEnded = false;
    }

    /// <summary>
    /// Method that saves the results at the end of the trial.
    /// </summary>
    void saveResults(){
        // Save that information for this trial
        session.CurrentTrial.result["end_x"] = endPosition.x;
        session.CurrentTrial.result["end_z"] = endPosition.y; // Note it's y because it comes from Vector2
        session.CurrentTrial.result["euclideanDistance"] = distance; 
        session.CurrentTrial.result["objectName"] = objectNames[target - 1];
        session.CurrentTrial.result["objectNumber"] = target;
        session.CurrentTrial.result["navStartTime"] = navStartTime;
        session.CurrentTrial.result["navEndTime"] = navEndTime;
        session.CurrentTrial.result["navTime"] = navEndTime - navStartTime;
        session.CurrentTrial.result["runStartTime"] = runStartTime;
        session.CurrentTrial.result["timesObjectPresented"] = timesObjectPresented[target - 1];
        session.CurrentTrial.result["confirmButtonTime"] = confirmButtonTime;
        session.CurrentTrial.result["movedDistance"] = movedDistance;
        session.CurrentTrial.result["warningShown"] = warningShown;
    }

    /// <summary>
    /// Method that handles everything that need to happen at the end of trial.
    /// </summary>
    public void EndTrial(){
        // Update progress bar if necessary
        if (showProgressBar){
            ProgressBar.UpdateProgressBar(trialNum, session.LastTrial.number);
        }

        // Log entry
        Debug.Log("End of trial " + trialNum);
        
        // Log the end time
        navEndTime = Time.time;

        // Set the object inactive
		currentObject.SetActive(false);

        // Calculate distance
        if(trialType == "encoding" | trialType == "control"){
            // Create Vector2 for player position
            endPosition = new Vector2(player.transform.position.x, player.transform.position.z);

            // Create vector2 for target position
            targetPosition = new Vector2(currentObject.transform.position.x, currentObject.transform.position.z);

            // Calculate Euclidean distance
            distance = Vector2.Distance(endPosition, targetPosition); 
        }

        // Update timesObjectPresented 
        timesObjectPresented[target - 1] += 1;

        // Save results
        saveResults();

        // Destroy arrow
        Destroy(arrow);
        Destroy(currentObject);

        // If enabled, disable constant cue image
        if(showConstantCue){
            constantCueImage.enabled = false;
        }

        // Reset movement so that player is stationary
    	ThreeButtonMovement.reset = true;

        // End trial
		session.EndCurrentTrial();

		// Set trial ended to true
		trialEnded = true;
    }

    /// <summary>
    /// Method that handles everything related to text input.
    // This needs to be attached to the On Session Begin Event of the UXF Rig.
    /// </summary>
    public void TextInformation(){
        objectNames = session.settings.GetStringList("objectNames");
        blockMessages = session.settings.GetStringList("blockMessages");
        waitForExperimenter = session.settings.GetString("waitForExperimenter");
        endMessage = session.settings.GetString("endMessage");
    }

    /// <summary>
    /// IEnumerator that handels the time the cue is presented
    /// </summary>
    IEnumerator ShowCue(float cueTime){
    	// Set inCueOrDelayPeriod to true so that the confirm button press is ignored
    	inCueOrDelayPeriod = true;

    	// Only if not continuous mode or during retrieval or on trial 1
        // which also serves as a cue to cut the video so it match with the trial
    	if(!continuousMode | trialType == "retrieval" | trialNum == 1){
	    	// Log entry
	        Debug.Log("Cue start of trial " + trialNum);

	        // Hide the fixation marker so the image can be displayed
	        fixationMarker.SetActive(false);

	    	// Show cueImage
	        panel.SetActive(true);

	        cueImage.sprite = objectsImages[target - 1];
	    	cueImage.enabled = true;

            // Disable movement
            ThreeButtonMovement.movementAllowed = false;

	    	// Wait until cue time is over 
	        yield return new WaitForSeconds(cueTime);	

	        // Log entry
        	Debug.Log("Cue end of trial " + trialNum);
    	}

        // Reset cue
    	cueImage.enabled = false;

    	// Start delay
		StartCoroutine(Delay());
    }

    /// <summary>
    /// IEnumerator that handels the delay between cue and movement start
    /// </summary>
    IEnumerator Delay(){
    	// Only if not continuous mode or during retrieval or on trial 1
        // which also serves as a cue to cut the video so it match with the trial
    	if(!continuousMode | trialType == "retrieval" | trialNum == 1){
	        // Log entry
	        Debug.Log("Start of delay period of trial " + trialNum);

	    	// Show fixationMarker
	    	fixationMarker.SetActive(true);

	    	// Wait delay then start new trial
	    	yield return new WaitForSeconds(delay);

	    	// Hide the background panel again
	    	panel.SetActive(false);

            // If constant cue is enabled display that
            if(showConstantCue){
                constantCueImage.sprite = objectsImages[target - 1];
                constantCueImage.enabled = true;
            }

	    	// Log entry
	        Debug.Log("End of delay period of trial " + trialNum);
    	}

        // Enable & reset movement & reset inCueOrDelayPeriod
        ThreeButtonMovement.reset = true;
        ThreeButtonMovement.movementAllowed = true;
        inCueOrDelayPeriod = false;

        // Start of navigation period
        navStartTime = Time.time;
    }

    /// <summary>
    /// IEnumerator that handels drawing the first message that has to be ended by space bar and the second message
    /// that is ended by trigger/S press. 
    /// </summary>
    IEnumerator drawMessage(){
        // Present message first
        // Log entry
        Debug.Log("Waiting for someone to press space.");

        // Play end sound at player position
        if(soundMode < 3){
            AudioSource.PlayClipAtPoint(endSound, player.transform.position, 1.0f);
        }

        // Activate panel and change text to message
        panel.SetActive(true);

        // Select the correct message and display
        blockMessageText.text = waitForExperimenter;

        // Wait until Space key is pressed or send
        yield return new WaitUntil(() => closeMessage1);

        // Set run inactive
        runActive = false;

        // Display message
        blockMessageText.text = message2draw;

        // Hide fixationMarker
        fixationMarker.SetActive(false);

        // Flip messageDrawn so that only then pressing foward
        // will close the message
        messageDrawn = ! messageDrawn;

        // Wait until S key is pressed or send
        yield return new WaitUntil(() => closeMessage2);

        // Deactivate panel and change text to message back to empty
        panel.SetActive(false);
        blockMessageText.text = "";

        // Flip closeMessage
        closeMessage1 = !closeMessage1;
        closeMessage2 = !closeMessage2;

        // Flip messageDrawn back
        messageDrawn = ! messageDrawn;

        // Log entry
        Debug.Log("Stopped presenting message");

        // Start new trial
        session.EndIfLastTrial(trial);
        if(session.hasInitialised){
            session.BeginNextTrialSafe();
        }
    }

    /// <summary>
    /// IEnumerator that handels the ITI countdown and the message draw
    /// </summary>
    IEnumerator countdownITI(){
        if(!continuousMode | trialType == "retrieval" | trialNum == 1){
            // Log entry
            Debug.Log("Start of ITI period of trial " + trialNum);

            // Disable movement
            ThreeButtonMovement.reset = true;
            ThreeButtonMovement.movementAllowed = false;


            // Wait ITI then start new trial
            yield return new WaitForSeconds(ITI);

            // Log entry
            Debug.Log("End of ITI period of trial " + trialNum);
        }
        
        // If statement to handle if a message should be drawn at the end
        if(drawMessageNextTrial){
            StartCoroutine(drawMessage());
        } else {
            // Start new trial
            session.EndIfLastTrial(trial);
            if(session.hasInitialised){
                session.BeginNextTrialSafe();
            }
        }
    }

    /// <summary>
    /// Function to strat the ITI countdown. This needs to be attached to the On Trial End Event of the UXF Rig.
    /// </summary>
    public void BeingCountdownITI(){
    	StartCoroutine(countdownITI());
    }

    /// <summary>
    /// Function to end application. This needs to be attached to the On Session End Event of the UXF Rig.
    /// </summary>
    public void TheEnd(){
        // In case the constant cue is active when ending, disable
        if(showConstantCue){
            constantCueImage.enabled = false;
        }

        // If useHTTPPost not used than quit immediately
        if(!useHTTPPost){
            Debug.Log("Application closed now.");
            Application.Quit();
        }

        // End session/trial if necessary
        if(session.InTrial){
            // End the trial
            session.EndCurrentTrial();  
        }
        if(!session.isEnding){
            // End the session
            session.End();
        }

        // Disable progress bar
        ProgressBar.ActivateProgressBar(false);
        
        // Set end screen active
        endScreen.SetActive(true);

        // Start end countdown
        startEndCountDown = true;

        // Get text
        endScreenText = endScreen.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>();
    }

    /// <summary>
    /// Function to deactivate the cursor. This needs to be attached to the On Session Begin Event of the UXF Rig.
    /// </summary>
    public void HideCursor(){
        Cursor.visible = false;
    }

    /// <summary>
    /// Method to log which platform is used. # More info here https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
    /// </summary>
    void whichPlatform(){
#if UNITY_EDITOR
        Debug.Log("Platform used is UNITY_EDITOR");
#elif UNITY_STANDALONE_OSX
            Debug.Log("Platform used is UNITY_STANDALONE_OSX");
#elif UNITY_STANDALONE_WIN
            Debug.Log("Platform used is UNITY_STANDALONE_WIN");
#elif UNITY_WEBGL
        Debug.Log("Platform used is UNITY_WEBGL");
#endif
    }

    /// <summary>
    /// Method to shuffle trials within a block as specified by .json file. 
    /// </summary>  
    public void ShuffleBlocks(Session session){
        int num_blocks2shuffle = blocks2shuffle.Count;
        for(int i = 1; i <= num_blocks2shuffle; i++){
            var currentBlock_index =  blocks2shuffle[i - 1];
            Debug.Log("Shuffle block: " + currentBlock_index);
            Block currentBlock = session.GetBlock(1);
            currentBlock.trials.Shuffle();
        }
    }

    /// <summary>
    /// Check which input devices are available
    /// </summary>
    void detectInputDevices(){
        //Debug.Log("Mouse connected to computer: " + Input.mousePresent);
        Debug.Log("Computer supports touchscreen: " + Input.touchSupported);
        Debug.Log("Device type: " + SystemInfo.deviceType);
    }
}