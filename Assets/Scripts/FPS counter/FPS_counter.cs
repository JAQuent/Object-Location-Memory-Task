using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UXF;
using UnityEngine.Networking;
using System.IO;
// Inspired by https://forum.unity.com/threads/fps-counter.505495/ 

public class FPS_counter: MonoBehaviour{
    public static double measuredFPS;
    public int currentFrameRate;
    public double avgFrameRate;
    public int samples;
    public Text displayTextCurrent;
    public Text displayTextAverage;
    public float refreshRate = 0.2f;
    public int temporaryStorageCapacity = 500; 
    public float FPSCriterium = 30.0f;
    private float timer;
    private int index1 = 0;
    private List<int> temporaryFrameRateStorage = new List<int>();
    private List<double> permanentStorage = new List<double>();
    private float measurementLength = 20.0f;
    private string[] lowFPS_message = {"Your average FPS is only", "This is not enough to complete the task. Please contact the experimenter for further instructions."};
    private string[] waitingMessage_string = {"Please wait 20 sec while we measure your frames per second (FPS)."};
    private bool measuring = false;

    void Start(){

        // Set timer to refresh rate
        timer = refreshRate;

#if UNITY_WEBGL
        StartCoroutine(SetUp_FPScounter_WebGL());
#else
        SetUp_FPScounter_standard();
#endif
    }
 
    /// <summary>
    /// IEnumerator that starts a short FPS measurement to see if the computer is able to handle the task
    /// </summary>
    IEnumerator startFPSMeasurement(){
        // Set measuring boolean to true
        measuring = true;

        // Present waiting message
        GameObject waitingMessage = transform.GetChild(4).gameObject;
        waitingMessage.GetComponent<UnityEngine.UI.Text>().text = waitingMessage_string[0];

        // Log the start
        Debug.Log("Start of FPS measurement...");

        // Wait until the measurement time is over
        yield return new WaitForSeconds(measurementLength);

        // Save the current average
        measuredFPS = avgFrameRate;

        // Log the end
        Debug.Log("End of FPS measurement...");

        // Deactivate the waiting message
        waitingMessage.SetActive(false);

        // If FPS is okay close the screen cover so the experiment can be started.
        // If not display message to contact the experimenter.
        if(measuredFPS >= FPSCriterium){
            GameObject screenCover = transform.GetChild(0).gameObject;
            screenCover.SetActive(false);
            GameObject lowFPSMessage = transform.GetChild(3).gameObject;
            lowFPSMessage.SetActive(false);
        } else {
            GameObject lowFPSMessage = transform.GetChild(3).gameObject;
            lowFPSMessage.GetComponent<UnityEngine.UI.Text>().text = lowFPS_message[0] + measuredFPS + lowFPS_message[1];
        }

        // Change measuredFPS in ExperimentController so it can be logged.
        ExperimentController.measuredFPS = measuredFPS.ToString();

        // Set measuring boolean to false
        measuring = false;
    }

    public void Update (){
        // Check if the F1 key is pressed to abort the measurement coroutine
        if(Input.GetKeyDown(KeyCode.F1) & measuring){
            Debug.Log("FPS check aborted by user!");
            StopCoroutine(startFPSMeasurement());

            // Save that test was aborted for later logging
            ExperimentController.measuredFPS = "Test was aborted by user.";

            // Deactivate the message and acitvate the UXF UI
            GameObject screenCover = transform.GetChild(0).gameObject;
            screenCover.SetActive(false);
            GameObject lowFPSMessage = transform.GetChild(3).gameObject;
            lowFPSMessage.SetActive(false);
            GameObject waitingMessage = transform.GetChild(4).gameObject;
            waitingMessage.SetActive(false);

            // Set measuring boolean to false
            measuring = false;
        }

        // Calculate current FPS
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        currentFrameRate = (int)current;

        // Update UI text 
        timer = timer - Time.deltaTime; // Coubt time for refresh rate
        if(timer <= 0){
            displayTextCurrent.text = currentFrameRate.ToString() + " FPS";
            timer = refreshRate;
        }

        // Commit current FPS to temporary storage
        if(index1 <= temporaryStorageCapacity){
            temporaryFrameRateStorage.Add(currentFrameRate);
            index1 = index1 + 1;
        } else {
            // If above index calculate average of the full temporary storage and save in long term storage. 
            permanentStorage.Add(temporaryFrameRateStorage.Average());
            avgFrameRate =  Mathf.Round((float)permanentStorage.Average());
            samples = permanentStorage.Count; // Calculate the number of samples
            displayTextAverage.text = avgFrameRate.ToString() + " FPS (AVG based on " + samples + ")";
            
            // Reset the values
            index1 = 0;
            temporaryFrameRateStorage = new List<int>();
        }
    }

    // Method to read in .txt file
    string[] readText(string fileName){
        // Loads .txt file and split by \t and \n
        string inputText = System.IO.File.ReadAllText(fileName);
        string[] stringList = inputText.Split('\t', '\n'); //splits by tabs and lines
        return stringList;
    }
#if UNITY_WEBGL
    private IEnumerator SetUp_FPScounter_WebGL(){
        ////////////// FPSCriterium
        // Set file names
        string fileName = "FPSCriterium.txt";

        // Create path/url to the file
        string fileLocation = Path.Combine(Application.streamingAssetsPath, fileName);

        // download file from StreamingAssets folder
        UnityWebRequest www = UnityWebRequest.Get(fileLocation);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error downloading " + fileName + " file: " + www.error);
            yield break;
        }

        // Get the text of the file
        string loaded_text = www.downloadHandler.text;

        // Parse to variable
        FPSCriterium = float.Parse(loaded_text);

        ////////////// lowFPS_message
        // Set file names
        fileName = "lowFPS_message.txt";

        // Create path/url to the file
        fileLocation = Path.Combine(Application.streamingAssetsPath, fileName);

        // download file from StreamingAssets folder
        www = UnityWebRequest.Get(fileLocation);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error downloading " + fileName + " file: " + www.error);
            yield break;
        }

        // Get the text of the file
        loaded_text = www.downloadHandler.text;

        // Parse to variable
        lowFPS_message = loaded_text.Split("\n");

        ////////////// waitingMessage
        // Set file names
        fileName = "waitingMessage.txt";

        // Create path/url to the file
        fileLocation = Path.Combine(Application.streamingAssetsPath, fileName);

        // download file from StreamingAssets folder
        www = UnityWebRequest.Get(fileLocation);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error downloading " + fileName + " file: " + www.error);
            yield break;
        }

        // Get the text of the file
        loaded_text = www.downloadHandler.text;

        // Parse to variable
        waitingMessage_string = loaded_text.Split("\n");

        // Start the FPS measurement
        StartCoroutine(startFPSMeasurement());
    }
#else
    private void SetUp_FPScounter_standard(){
        // Look for FPS criterium
        string path2file = Application.streamingAssetsPath + "/FPSCriterium.txt";
        bool fileExists = System.IO.File.Exists(path2file);
        if (fileExists){
            Debug.Log("File exists at path: " + path2file + ". Provided value will be choosen. ");
            string[] input_FPSCriterium = readText(path2file); 
            FPSCriterium = float.Parse(input_FPSCriterium[0]);
        } else {
            Debug.Log("File does not exist at path: " + path2file + ". Default value will be choosen. This is " + FPSCriterium + " FPS.");
        }

        // Look for the FPS message
        path2file = Application.streamingAssetsPath + "/lowFPS_message.txt";
        fileExists = System.IO.File.Exists(path2file);
        if (fileExists){
            Debug.Log("File exists at path: " + path2file + ". Provided values will be choosen. ");
            lowFPS_message = readText(path2file); 
        } else {
            Debug.Log("File does not exist at path: " + path2file + ". Default values will be choosen.");
        }

        // Look for waiting message
        path2file = Application.streamingAssetsPath + "/waitingMessage.txt";
        fileExists = System.IO.File.Exists(path2file);
        if (fileExists){
            Debug.Log("File exists at path: " + path2file + ". Provided values will be choosen. ");
            waitingMessage_string = readText(path2file); 
        } else {
            Debug.Log("File does not exist at path: " + path2file + ". Default value will be choosen.");
        }
        
        // Start the FPS measurement
        StartCoroutine(startFPSMeasurement());
    }

#endif
}