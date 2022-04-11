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
    public static float experimentStarted; 

	// reference to the UXF Session 
    public Session session; 

	// Public vars
	public List<GameObject> objects;
    public List<Sprite> objectsImages;
	public GameObject arrowPrefab;
	public KeyCode startButton = KeyCode.S;
	public KeyCode confirmButton = KeyCode.E;
    public KeyCode forwardKey = KeyCode.W;
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
    private bool closeMessage = false; // Will switch to true if forward key is pressed during presentation of mesage
    private bool messageDrawn = false;
    private bool sessionStarted = false;
    private string message2draw;
    private int blockMessage1_trial;
    private int blockMessage2_trial;
    private string blockMessage1_eng;
    private string blockMessage2_eng;
    private string blockMessage1_cn;
    private string blockMessage2_cn;
    

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
    	// Start first trial
        if(Input.GetKey(startButton) & !session.InTrial & sessionStarted){
            // Log entry
            Debug.Log("Start message send");

            // Start experiment and log time
            experimentStarted = Time.time; 

            // Begin first trial
        	session.BeginNextTrial(); 
        }

        // Only allow this in second block and only during a trial
        // Also only for standard trial
        if(Input.GetKey(confirmButton) & blockNum > 1 & session.InTrial & !buttonPressed & trialType == "standard"){
            // Log entry
            Debug.Log("Confirm button pressed: trial" + trialNum);

            // Flip the button so it can only be pressed once
            buttonPressed = !buttonPressed; 

        	// Show object as feedback
        	objects[target - 1].SetActive(true);

            // Get position of object
            arrow.SetActive(true);

            // Create Vector2 for player position
            endPosition = new Vector2(player.transform.position.x, player.transform.position.z);

            // Create vector2 for target position
            targetPosition = new Vector2(objects[target - 1].transform.position.x, objects[target - 1].transform.position.z);

            // Calculate Euclidean distance
            distance = Vector2.Distance(endPosition, targetPosition); 
        }

        // Close message if drawn
        if(drawMessageNextTrial & messageDrawn){
            if(Input.GetKeyDown(forwardKey)){
                closeMessage = !closeMessage;
            }
        }

        // Stop Experiment
        if(Input.GetKey(KeyCode.Escape)){
            // Log entry
            Debug.Log("The end");

            // Close application
            TheEnd();
        }
    }
    public void sessionStart(){
        sessionStarted = true;
    }

    // Setting up trial
    public void SetUpTrial(){
        // Initialise the button press so it can be pressed. 
        buttonPressed = false;

    	// Get current trial and block
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

		// Set movement param
		ThreeButtonMovement.forwardSpeed = trial.settings.GetInt("speedForward");
		ThreeButtonMovement.rotationSpeed = trial.settings.GetInt("rotationSpeed");

		// Reset movement so that player is stationary at the beginning
        ThreeButtonMovement.reset = true;
		
		// Show cue
		StartCoroutine(ShowCue(cueTime));

		// In first block & on control trials show the object
		if(blockNum == 1 | trialType == "control"){
			objects[target - 1].SetActive(true);
		} 

        // Get position of object
        Vector3 objPos = objects[target - 1].transform.position;

        // Add arrow to indicate where object is and move over to correc
        arrow = Instantiate(arrowPrefab);
        arrow.transform.position = new Vector3(objPos.x, arrow.transform.position.y, objPos.z);

        // For blocks over 1 that are standard
        if(blockNum > 1 & trialType == "standard"){
            arrow.SetActive(false);
        }

		// Get spawn radius
		int spawnRadius = session.settings.GetInt("spawnRadius");

		// Random position of player
		Vector2 randomPosition  = Random.insideUnitCircle * spawnRadius;
		player.transform.position = new Vector3(randomPosition.x, 2.0f, randomPosition.y);

		// Save that information for analysis
		session.CurrentTrial.result["start_x"] = randomPosition.x;
		session.CurrentTrial.result["start_z"] = randomPosition.y; // Note it's because it comes from Vector2

        // Check if message should be drawn and the end
        if(trialNum == blockMessage1_trial){
            // Set 2 true
            drawMessageNextTrial = true;

            // Selct the corret message
            if(language == "chinese"){
                message2draw = blockMessage1_cn;
            } else {
                message2draw = blockMessage1_eng;
            } 
        } else if (trialNum == blockMessage2_trial){
            // Set 2 true
            drawMessageNextTrial = true;

            // Selct the corret message
            if(language == "chinese"){
                message2draw = blockMessage2_cn;
            } else {
                message2draw = blockMessage2_eng;
            } 
        } else {
            drawMessageNextTrial = false;
        }
    }

    ////////////////////////////////
    // Function that handles everything to the set up the objects
    public void SetUpObjectLocations(){
    	// Object index
    	int j;

    	// Instatiate objects and set in-active
		for (int i = 0; i < objects.Count; i++){
        	objects[i] = Instantiate(objects[i]);
        	objects[i].SetActive(false);
        	j = i + 1;
        	objects[i].name = "Object" + j;

        	// Get spawn radius
			int spawnRadius_object = session.settings.GetInt("spawnRadius_object");

			// Random position of object
			Vector2 randomPosition  = Random.insideUnitCircle * spawnRadius_object;
			objects[i].transform.position = new Vector3(randomPosition.x, objects[i].transform.position.y, randomPosition.y);
		}
    }

    ////////////////////////////////
    // Function that handles everything at the end of trial
    public void EndTrial(){
        // Log entry
        Debug.Log("End of trial " + trialNum);
        
        // Log the end time
        navEndTime = Time.time;

        // Set the object inactive
		objects[target - 1].SetActive(false);


        // Calculate distance
        if(blockNum == 1 | trialType == "control"){
            // Create Vector2 for player position
            endPosition = new Vector2(player.transform.position.x, player.transform.position.z);

            // Create vector2 for target position
            targetPosition = new Vector2(objects[target - 1].transform.position.x, objects[target - 1].transform.position.z);

            // Calculate Euclidean distance
            distance = Vector2.Distance(endPosition, targetPosition); 
        }

        // Update timesObjectPresented 
        timesObjectPresented[target - 1] += 1;
		
        // Save that information for this trial
        session.CurrentTrial.result["end_x"] = endPosition.x;
        session.CurrentTrial.result["end_z"] = endPosition.y; // Note it's y because it comes from Vector2
        session.CurrentTrial.result["target_x"] = targetPosition.x;
        session.CurrentTrial.result["target_z"] = targetPosition.y; // Note it's y because it comes from Vector2
        session.CurrentTrial.result["euclideanDistance"] = distance; 
        session.CurrentTrial.result["objectName"] = objectNames_eng[target - 1];
        session.CurrentTrial.result["objectNumber"] = target;
        session.CurrentTrial.result["navStartTime"] = navStartTime;
        session.CurrentTrial.result["navEndTime"] = navEndTime;
        session.CurrentTrial.result["navTime"] = navEndTime - navStartTime;
        session.CurrentTrial.result["experimentStarted"] = experimentStarted;
        session.CurrentTrial.result["timesObjectPresented"] = timesObjectPresented[target - 1];

        // Destroy arrow
        Destroy(arrow);

        // Reset movement so that player is stationary
    	ThreeButtonMovement.reset = true;


        // End trial
		session.EndCurrentTrial();
    }

    ////////////////////////////////
    // The function should be added to the UXF rig event 
    ///at the beginning of the session. 
    public void LanguageInformation(){
        language = session.settings.GetString("language");
        objectNames_eng = session.settings.GetStringList("objectNames_eng");
        blockMessage1_eng = session.settings.GetString("blockMessage1_eng");
        blockMessage2_eng = session.settings.GetString("blockMessage2_eng");
        blockMessage1_cn = session.settings.GetString("blockMessage1_cn");
        blockMessage2_cn = session.settings.GetString("blockMessage2_cn");
        blockMessage1_trial = session.settings.GetInt("blockMessage1_trial");
        blockMessage2_trial = session.settings.GetInt("blockMessage2_trial");
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
    // IEnumerator that handels the ITI countdown and message draw
    IEnumerator countdownITI(){
        // Log entry
        Debug.Log("Start of ITI period of trial" + trialNum);

    	// Wait ITI then start new trial
    	yield return new WaitForSeconds(ITI);

        // Log entry
        Debug.Log("End of ITI period of trial" + trialNum);

        // If statement to handle if a message should be drawn at the end
        if(drawMessageNextTrial){
            // Present message first
            // Log entry
            Debug.Log("Started presenting message");

            // Activate panel and change text to message
            panel.SetActive(true);
            blockMessage.text = message2draw;

            // Hide fixationMarker
            fixationMarker.SetActive(false);

            // Flip messageDrawn so that only then pressing foward
            // will close the message
            messageDrawn = ! messageDrawn;

            // Wait until forward key is pressed
            yield return new WaitUntil(() => closeMessage);

            // Deactivate panel and change text to message back to empty
            panel.SetActive(false);
            blockMessage.text = "";

            // Flip closeMessage
            closeMessage = !closeMessage;

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

// Post processing
// recursive player position etc.
// https://www.youtube.com/watch?v=GtwaN-bONvA&ab_channel=TheChase-racinggame
// https://github.com/samoliverdev/ShaderGraphStochastic
// http://boundingboxsoftware.com/materialize/getkey.php
// add grass https://www.youtube.com/watch?v=L_Bzcw9tqTc&ab_channel=Brackeys
// add random orientation
// for shadows go setting/ and URP seetings
// https://gamedev.stackexchange.com/questions/84401/locking-frame-rate-to-60-in-the-editor
// https://www.youtube.com/watch?v=v5_RN8o1b3g&ab_channel=YagmanX
// check aidan's retrieval phase
// Four block: 6 x 4 -> 24 trials 
// Add another object for a controll task (like a gift)
// check if pressing S at beginning is an issue


// add set up reflection probe