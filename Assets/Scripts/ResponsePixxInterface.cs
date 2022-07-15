using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using unitypixx;
using UnityEngine.UI;
using System.IO;

// Attach this script to the game object will control movement of the ThreeButtonMovement.cs script as well as
// the confirmation response in ExperimentController.cs using a ResponsePixx button box. This script can easily
// control something else by changing the corresponding methods (e.g. greenMethod).

public class ResponsePixxInterface : MonoBehaviour{
	// public vars
    public bool debugMode = false;
    public string fileName = "responsePixx.json";

    // private vars sthat can be configured.
    private uint dinBuffAddr = 0x800000;
    private uint dinBuffSize = 0x400000;
    private int deviceType = 2; // Default see below. 
    // Legend of codes:
    /* 1   DATAPixx
       2   DATAPixx2
       3   DATAPixx3
       4   VIEWPixx
       5   PROPixx Controller
       6   PROPixx
       7   TRACKPixx */
    // key information. The default below are for MRI button box
    private int yellowCode = -65523;
    private int redCode = -65522;
    private int blueCode = -65529;
    private int greenCode = -65525;

    // private vars hard-coded
    private ushort[] rxBuff;          // response buffer
    private int err = -99;            // error code
    private int deviceDetected = -99; // deviced detected
    private int opened;               // Was a connection opened?

    // You need to set-up all variables that you want to get from the .json file.
    // The variable names have to correspond to the input names in that file. 
    [System.Serializable]
    public class JSONDataClass {
        public int yellowCode;
        public int redCode;
        public int blueCode;
        public int greenCode;
        public int deviceType;
        public uint dinBuffAddr;
        public uint dinBuffSize;
    }

    // private vars
    private JSONDataClass JSONData;

    // Start is called before the first frame update
    void Start(){
        // Configuration
        configure();

        // Open connection
        openConnection();
    }

    /// <summary>
    /// Configuration method
    /// </summary>
    void configure(){
        GetDataFromJSON(fileName);

        // Assign the new values
        dinBuffAddr = JSONData.dinBuffAddr;
        dinBuffSize = JSONData.dinBuffSize;
        deviceType = JSONData.deviceType;
        yellowCode = JSONData.yellowCode;
        redCode = JSONData.redCode;
        blueCode = JSONData.blueCode;
        greenCode = JSONData.greenCode;
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

    /// <summary>
    /// Open connection and set everything up. 
    /// </summary>
    void openConnection(){
        dp.DPxSetDebug(1);  // Enable printing of libdpx API debugging messages
        opened = dp.DPxOpen(); // Open a connection
        err = dp.DPxGetError(); // Error code if an error occured, otherwise 0.
        deviceDetected = dp.DPxDetectDevice(deviceType); 
        Debug.Log("Datapixx Device detected: " + deviceDetected);
        Debug.Log("Datapixx Error code: " + err);

        // Check if error
        if (err != 0) {
            Debug.Log("Datapixx ERROR: Couldn't open a Datapixx!\n");
        }

        // Check if device is ready
        if (dp.DPxIsReady() == 1){
            Debug.Log("DPxIsReady: Success!");
        } else {
            Debug.Log("DPxIsReady: Nope!");
        }

        // The DATAPixx "Digital IN" port can manage two different types of input transients.
        // "DIN Stabilization" eliminates transients in the order of 80 nanoseconds.
        // The purpose of this is to remove transmission line reflections when capturing parallel port data.
        // "DIN Debouncing" removes button debouncing of up to 30 milliseconds.
        // Debouncing reports a button transition immediately, but will then ignore any further transitions during the next 30 ms.
        dp.DPxEnableDinStabilize();
        dp.DPxEnableDinDebounce();

        // The digital input subsystem can use a normal schedule to capture the DIN port at regular intervals;
        // however, for our purposes, we will use a "logging" mode which only records transitions on the DIN port.
        dp.DPxEnableDinLogTimetags();
        dp.DPxEnableDinLogEvents();

        // Set the location of the RAM buffer to which the digital input subsystem should log the button transitions.
        // We'll arbitrarily place the start of the buffer at DATAPixx address 8 MB, and specify a buffer size of 4 MB.
        // DIN logging with timestamps stores the data as a 64-bit nanosecond timetag, followed by a 16-bit DIN datum.
        // One reason that we selected DIN bits 23:16 as the button outputs, is that only bits 15:0 recorded during logging.
        dp.DPxSetDinBuff(dinBuffAddr, dinBuffSize);

        //Let's push these commands to the device register, and record the time this occurred
        dp.DPxUpdateRegCache();
    }

    // Update is called once per frame
    void Update(){
        dp.DPxSetDinBuff(dinBuffAddr, dinBuffSize);
        dp.DPxUpdateRegCache();
        if(dp.DPxGetDinBuffWriteAddr() != dinBuffAddr){
            // Key press check which one
            // Read the keydown from memory
            rxBuff = dp.DPxReadRam(dinBuffAddr, 10);
            var keyDown = ~rxBuff[4];
            if(debugMode){
                Debug.Log(keyDown);
            }

            // Check which key was pressed
            if(keyDown == yellowCode){
            	yellowMethod();
            } else if(keyDown == redCode){
            	redMethod();
            } else if(keyDown == blueCode){
            	blueMethod();
            } else if(keyDown == greenCode){
            	greenMethod();
            } else {
            	Debug.Log("Datapixx: Unknown code.");
            }
        }
    }

    /// <summary>
    /// Yellow method set-up to move foward.
    /// </summary>
    void yellowMethod(){
    	Debug.Log("Datapixx: Yellow!");
        ThreeButtonMovement.turnignLeft = false;
        ThreeButtonMovement.movingForward = !ThreeButtonMovement.movingForward;
        ThreeButtonMovement.turningRight = false;
    }
    /// <summary>
    /// Red method set-up to turn right.
    /// </summary>
    void redMethod(){
    	Debug.Log("Datapixx: Red!");
        ThreeButtonMovement.turnignLeft = false;
        ThreeButtonMovement.movingForward = false;
        ThreeButtonMovement.turningRight = !ThreeButtonMovement.turningRight;
    }
    /// <summary>
    /// Red method to act as confirm button.
    /// </summary>
    void blueMethod(){
    	Debug.Log("Datapixx: Blue!");
        ExperimentController.confirm = true;
    }
    /// <summary>
    /// Red method set-up to turn left.
    /// </summary>
    void greenMethod(){
    	Debug.Log("Datapixx: Green!");
        ThreeButtonMovement.turnignLeft = !ThreeButtonMovement.turnignLeft;
        ThreeButtonMovement.movingForward = false;
        ThreeButtonMovement.turningRight = false;
    }
}