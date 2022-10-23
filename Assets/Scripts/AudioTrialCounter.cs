// This sctrip is inspired by Benjamin Outram: https://www.benjaminoutram.com/blog/2018/7/13/procedural-audio-in-unity-noise-and-tone
// The purpose of the script is to give an audio signal to the experimenter indicating how many trials are left.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF; 

public class AudioTrialCounter : MonoBehaviour{
    // Play sound
    public bool play;

    // Reference to the UXF Session 
    public Session session; 

    // Sound settings
    public float intervalLength = 0.2f;
    public float toneLength = 0.5f;
    private float timer;
    private float sampling_frequency = 48000;

    // For the tone
    public float frequency = 440f;
    public float gain = 0.9f;
    private float increment;
    private float phase;

    void Awake(){
        sampling_frequency = AudioSettings.outputSampleRate;
    }

    // Control playing in update method
    void Update(){
        // Check for button press
        if(Input.GetKeyDown(KeyCode.Backspace) & ExperimentController.sessionStarted){
            int trialNum =  session.CurrentTrial.numberInBlock; 
            int numTrialsInBlock = session.CurrentBlock.lastTrial.numberInBlock;
            int trialsLeft = numTrialsInBlock - trialNum;
            Debug.Log("AudioTrialCunter requested. Number of trials left: " + trialsLeft);
            StartCoroutine(startSequence(trialsLeft));
        }
    }

    /// <summary>
    /// Coroutine that controls the sequence of tones
    /// </summary>
    IEnumerator startSequence(int numTones){
        for (int i = 1; i <= numTones; i++){
            play = true;
            // Play the sound for toneLength
            yield return new WaitForSeconds(toneLength);
            play = false;
            // Not play the sound for toneLength
            yield return new WaitForSeconds(intervalLength);
        }
    }

    /// <summary>
    /// More info on https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnAudioFilterRead.html
    /// </summary>
    void OnAudioFilterRead(float[] data, int channels){
        if(play){
            // update increment in case frequency has changed
            increment = frequency * 2f * Mathf.PI / sampling_frequency;

            for (int i = 0; i < data.Length; i++){
                phase = phase + increment;
                if (phase > 2 * Mathf.PI) phase = 0;

                // Tone
                data[i] = (float)(gain * Mathf.Sin(phase));

                // if we have stereo, we copy the mono data to each channel
                if (channels == 2){
                    data[i + 1] = data[i];
                    i++;
                }   
            }
        }
    }
}

