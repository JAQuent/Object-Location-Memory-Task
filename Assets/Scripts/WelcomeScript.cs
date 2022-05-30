using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WelcomeScript : MonoBehaviour{
	// public vars
	public Toggle m_Toggle;

	// Static
	public static string language;

    // Start is called before the first frame update
    void Start(){
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
}
