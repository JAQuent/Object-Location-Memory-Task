using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour{
    // This script handles the progress bar for the experiment
    
    // Static variables
    public static Slider slider;
    private static GameObject background;
    private static GameObject fill;

    // Excuted at the beginging
    private void Start(){
        // Assign the slider and the children of the progress bar
        slider = GetComponent<Slider>();
        background = this.gameObject.transform.GetChild(0).gameObject;
        fill = this.gameObject.transform.GetChild(1).gameObject;

        // Set the progress bar to inactive as a default
        background.SetActive(false);
        fill.SetActive(false);
    }

    /// <summary>
    /// Set progress bar components active/inactive.
    /// </summary>
    public static void ActivateProgressBar(bool active){
        // Log the status
        Debug.Log("Progress bar status changed to " + active);

        // Set to active/inactive
        background.SetActive(active);
        fill.SetActive(active);
    }

    /// <summary>
    /// Method update the progress bar to the current trial.
    /// </summary>
    public static void UpdateProgressBar(float currentTrial, float totalNumTrials){
        // Log
        Debug.Log("Completed " + currentTrial + " / " + totalNumTrials + " trials");
        // set the value of the slider to the current trial divided by the total number of trials
        slider.value = currentTrial / totalNumTrials; 
    }
}
