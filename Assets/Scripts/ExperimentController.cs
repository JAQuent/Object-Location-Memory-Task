using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UXF; 
using System.IO;

// Description of this script:
// Cue (2s) -> delay (4s) -> replace, feedback, collect (7s), ITI (6s). 

public class ExperimentController : MonoBehaviour{
    // Public static vars
    public static float triggerSend_timeStamp; 

	// reference to the UXF Session 
    public Session session; 

    // reference to camera (for SkyBox) & to background to activate & deactivate on control trials
    public Camera mainCam;
    public GameObject background;

	// Public vars
	public List<GameObject> objects;
    public List<Sprite> objectsImages;
	public GameObject arrowPrefab;
	public KeyCode startButton = KeyCode.S;
	public KeyCode confirmButton = KeyCode.E;
    public int target;
    public Image cueImage;
    public Text blockMessage;
    public GameObject panel;
    public GameObject fixationMarker;
    public GameObject player;
    public float distance;
    public List<int> timesObjectPresented; // How often have this object been presented?

	// Private vars
	private int trialNum;
	private int blockNum;
	private GameObject arrow;
	private Trial trial;
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
    private bool sessionStarted = false; // Has the session started yet?
    private string message2draw; // Should a message be drawn after this trial?
    private List<string> blockMessage_eng; // List with the block messages displayed after each block. 
    private List<string> blockMessage_cn; // List with the block messages displayed after each block. 
    private string waitForExperimenter_eng; // String for screen that will be presented for each block message. 
    private string waitForExperimenter_cn; // String for screen that will be presented for each block message.
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
    

    // Private language vars
    private string language;
    private List<string> objectNames_eng; // English object names

    // Excuted at the beginging
    void Start(){
        // Initialise timesObjectPresented
        for(int i = 0; i < objects.Count; i++){
            timesObjectPresented.Add(0);
        }
    }


    // Update is called once per frame
    void Update(){
    	// Start first trial if not in trial already, session started and when the experiment is not started yet
    	// These conditions are important so that the programm doesn't attempt to start the experiment at the wrong time
        if(Input.GetKey(startButton) & !session.InTrial & sessionStarted & !experimentStarted){
            // Log entry
            Debug.Log("Start message send");

            // Start experiment and log time
            triggerSend_timeStamp = Time.time; 

            // Set experiment started to true
            experimentStarted = true;

            // Begin first trial
        	session.BeginNextTrial(); 
        }

        // Only allow this in second block and only during a trial
        // Also only for standard trial
        if(Input.GetKey(confirmButton) & blockNum > 1 & session.InTrial & !buttonPressed & trialType == "retrieval"){
            // Log entry
            Debug.Log("Confirm button pressed: trial" + trialNum);

            // Flip the button so it can only be pressed once
            buttonPressed = !buttonPressed; 

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
        }


        // Wait for space bar press
		if(Input.GetKeyDown(KeyCode.Space) & !closeMessage1 & trialEnded){
        		closeMessage1 = !closeMessage1;
        		Debug.Log("Experimenter pressed space bar.");
        }

        // Close message if it is drawn and the confirmButton is pressed. 
        if(drawMessageNextTrial & messageDrawn){
            if(Input.GetKeyDown(startButton)){
                closeMessage2 = !closeMessage2;
                Debug.Log("Close message!");
            }
        }

        // Stop Experiment
        if(Input.GetKey(KeyCode.Escape)){
            // Log entry
            Debug.Log("The end");

            // Close application
            TheEnd();
        }

        // Log entry for each startButton pressed after the experiment started
        if(Input.GetKeyDown(startButton) & experimentStarted){
        	Debug.Log("trigger was send");

        	// Log time
            triggerSend_timeStamp = Time.time; 
        }
    }

    // Starting the session
    public void sessionStart(){
    	// Set bool to true
        sessionStarted = true;

        // Set frame rate to maximum
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = session.settings.GetInt("targetFrameRate");
    }

    // Setting up trial
    public void SetUpTrial(){
        // Initialise the button press so it can be pressed. 
        buttonPressed = false;

    	// Get current trial and block number
    	trialNum =  session.currentTrialNum; 
		blockNum =  session.currentBlockNum;

		// Get current trial
		trial = session.CurrentTrial;

		// Get current trial values
		target = trial.settings.GetInt("targets");
		delay = trial.settings.GetFloat("delay");
		cueTime = trial.settings.GetFloat("cue");
		ITI = trial.settings.GetFloat("ITI");
		trialType = trial.settings.GetString("trialType");
		messageToDisplay = trial.settings.GetInt("messageToDisplay");

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

		// Set up background 
		if(trialType == "control" & displayingBackground){
			// For control trials
			mainCam.GetComponent<Skybox>().enabled = false;
			background.SetActive(false);
			displayingBackground = false;
		} else if (trialType != "control" & !displayingBackground){
			// For normal encoding & retrieval trials
			mainCam.GetComponent<Skybox>().enabled = true;
			background.SetActive(true);
			displayingBackground = true;
		}


		// Instantiate object
        currentObject = Instantiate(objects[target - 1]);
        currentObject.SetActive(false);
        currentObject.name = "Object" + target;
		currentObject.transform.position = new Vector3(object_x, currentObject.transform.position.y, object_z);

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

        // For blocks over 1 that are standard
        if(blockNum > 1 & trialType == "retrieval"){
            arrow.SetActive(false);
        }

		// Set player position and rotation
		player.transform.position = new Vector3(start_x, 1.0f, start_z);
		player.transform.rotation = Quaternion.Euler(0, start_yRotation, 0);

        // Check if message should be drawn after the trial.
        // by checking if messageToDisplay is above -1. If not, it means not message.
        if(messageToDisplay >= 0){
            // Set 2 true
            drawMessageNextTrial = true;

            // Select the correct message
            if(WelcomeScript.language == "chinese"){
                message2draw = blockMessage_cn[messageToDisplay];
            } else {
                message2draw = blockMessage_eng[messageToDisplay];
            } 
        } else {
            drawMessageNextTrial = false;
        }

        // Set trial ended to false
        trialEnded = false;
    }


    ////////////////////////////////
    // Function that handles everything at the end of trial
    public void EndTrial(){
        // Log entry
        Debug.Log("End of trial " + trialNum);
        
        // Log the end time
        navEndTime = Time.time;

        // Set the object inactive
		currentObject.SetActive(false);


        // Calculate distance
        if(blockNum == 1 | trialType == "control"){
            // Create Vector2 for player position
            endPosition = new Vector2(player.transform.position.x, player.transform.position.z);

            // Create vector2 for target position
            targetPosition = new Vector2(currentObject.transform.position.x, currentObject.transform.position.z);

            // Calculate Euclidean distance
            distance = Vector2.Distance(endPosition, targetPosition); 
        }

        // Update timesObjectPresented 
        timesObjectPresented[target - 1] += 1;
		
        // Save that information for this trial
        session.CurrentTrial.result["end_x"] = endPosition.x;
        session.CurrentTrial.result["end_z"] = endPosition.y; // Note it's y because it comes from Vector2
        session.CurrentTrial.result["euclideanDistance"] = distance; 
        session.CurrentTrial.result["objectName"] = objectNames_eng[target - 1];
        session.CurrentTrial.result["objectNumber"] = target;
        session.CurrentTrial.result["navStartTime"] = navStartTime;
        session.CurrentTrial.result["navEndTime"] = navEndTime;
        session.CurrentTrial.result["navTime"] = navEndTime - navStartTime;
        session.CurrentTrial.result["triggerSend_timeStamp"] = triggerSend_timeStamp;
        session.CurrentTrial.result["timesObjectPresented"] = timesObjectPresented[target - 1];

        // Destroy arrow
        Destroy(arrow);

        // Reset movement so that player is stationary
    	ThreeButtonMovement.reset = true;

        // End trial
		session.EndCurrentTrial();

		// Set trial ended to true
		trialEnded = true;
    }

    ////////////////////////////////
    // The function should be added to the UXF rig event 
    ///at the beginning of the session. 
    public void LanguageInformation(){
        objectNames_eng = session.settings.GetStringList("objectNames_eng");
        blockMessage_eng = session.settings.GetStringList("blockMessage_eng");
        blockMessage_cn = session.settings.GetStringList("blockMessage_cn");
        waitForExperimenter_eng = session.settings.GetString("waitForExperimenter_eng");
        waitForExperimenter_cn = session.settings.GetString("waitForExperimenter_cn");
    }

    ////////////////////////////////
    // IEnumerator that handels the time the cue is presented
    IEnumerator ShowCue(float cueTime){
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

        // Reset cue
    	cueImage.enabled = false;

    	// Start delay
		StartCoroutine(Delay());
    }

    ////////////////////////////////
    // IEnumerator that handels the delay between cue and movement start
    IEnumerator Delay(){
        // Log entry
        Debug.Log("Start of delay period of trial " + trialNum);

    	// Show fixationMarker
    	fixationMarker.SetActive(true);

    	// Wait delay then start new trial
    	yield return new WaitForSeconds(delay);

    	// Hide the background panel again
    	panel.SetActive(false);

        // Enable movement
        ThreeButtonMovement.movementAllowed = true;

        // Start of navigation period
        navStartTime = Time.time;

        // Log entry
        Debug.Log("End of delay period of trial " + trialNum);
    }


    ////////////////////////////////
    // IEnumerator that handels the ITI countdown and the message draw
    IEnumerator countdownITI(){
        // Log entry
        Debug.Log("Start of ITI period of trial" + trialNum);

    	// Wait ITI then start new trial
    	yield return new WaitForSeconds(ITI);

    	// Disable movement
    	ThreeButtonMovement.movementAllowed = false;

        // Log entry
        Debug.Log("End of ITI period of trial" + trialNum);

        // If statement to handle if a message should be drawn at the end
        if(drawMessageNextTrial){
            // Present message first
            // Log entry
            Debug.Log("Waiting for experimenter to press space.");

            // Activate panel and change text to message
            panel.SetActive(true);

            // Select the correct message and display
            if(WelcomeScript.language == "chinese"){
                blockMessage.text = waitForExperimenter_cn;
            } else {
                blockMessage.text = waitForExperimenter_eng;
            } 

            // Wait until S key is pressed or send
            yield return new WaitUntil(() => closeMessage1);

            // Display message
            blockMessage.text = message2draw;

            // Hide fixationMarker
            fixationMarker.SetActive(false);

            // Flip messageDrawn so that only then pressing foward
            // will close the message
            messageDrawn = ! messageDrawn;

            // Wait until S key is pressed or send
            yield return new WaitUntil(() => closeMessage2);

            // Deactivate panel and change text to message back to empty
            panel.SetActive(false);
            blockMessage.text = "";

            // Flip closeMessage
            closeMessage1 = !closeMessage1;
            closeMessage2 = !closeMessage2;

            // Flip messageDrawn back
            messageDrawn = ! messageDrawn;

            // Log entry
            Debug.Log("Stopped presenting message");

            // Start new trial
            session.BeginNextTrial(); 
        } else {
            // Start new trial
            session.BeginNextTrial(); 
        }
    }

    ////////////////////////////////
    // Function to strat the ITI countdown
    public void BeingCountdownITI(){
    	StartCoroutine(countdownITI());
    }

    ////////////////////////////////
    // Function to end application
    public void TheEnd(){
        Application.Quit();
    }

    ////////////////////////////////
    // Function to deactivate cursor
    public void HideCursor(){
        Cursor.visible = false;
    }
}