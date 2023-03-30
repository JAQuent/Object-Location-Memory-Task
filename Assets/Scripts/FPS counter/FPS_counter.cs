using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
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
    private float timer;
    private int index1 = 0;
    private List<int> temporaryFrameRateStorage = new List<int>();
    private List<double> permanentStorage = new List<double>();
    private float FPSCriterium = 30.0f;
    private GameObject UXF_UI;

    void Start(){
        // Find the UXF UI and disable it for the test
        UXF_UI = GameObject.Find("[UXF_Rig]");
        UXF_UI.SetActive(false);

        // Set timer to refresh rate
        timer = refreshRate;

        // Start the FPS measurement
        StartCoroutine(startFPSMeasurement());
    }
 
    /// <summary>
    /// IEnumerator that starts a short FPS measurement to see if the computer is able to handle the task
    /// </summary>
    IEnumerator startFPSMeasurement(){
        // Log the start
        Debug.Log("Start of FPS measurement...");

        // Wait until the measurement time is over
        yield return new WaitForSeconds(20);

        // Save the current average
        measuredFPS = avgFrameRate;

        // Log the end
        Debug.Log("End of FPS measurement...");

        // If FPS is okay close the screen cover so the experiment can be started.
        // If not display message to contact the experimenter.
        if(measuredFPS >= FPSCriterium){
            GameObject screenCover = transform.GetChild(0).gameObject;
            screenCover.SetActive(false);
            GameObject lowFPSMessage = transform.GetChild(3).gameObject;
            lowFPSMessage.SetActive(false);
            // Also activate the UFX menu again
            UXF_UI.SetActive(true);
        } else {
            GameObject lowFPSMessage = transform.GetChild(3).gameObject;
            lowFPSMessage.GetComponent<UnityEngine.UI.Text>().text = "Your average FPS is only " + measuredFPS + ". This is not enough to complete the task. Please contact the experimenter for further instructions.";
        }
    }

    public void Update (){
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
}
