    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using System.Linq;
    // Inspired by https://forum.unity.com/threads/fps-counter.505495/ 

    public class FPS_counter : MonoBehaviour{
        public int currentFrameRate;
        public double avgFrameRate;
        public int samples;
        public Text displayTextCurrent;
        public Text displayTextAverage;
        public int index = 0;
        public int temporaryStorageCapacity = 500; 
        private List<int> temporaryFrameRateStorage = new List<int>();
        private List<double> permanentStorage = new List<double>();
     
        public void Update (){
            float current = 0;
            current = (int)(1f / Time.unscaledDeltaTime);
            currentFrameRate = (int)current;
            displayTextCurrent.text = currentFrameRate.ToString() + " FPS";

            // Commit to temporary storage
            if(index <= temporaryStorageCapacity){
                temporaryFrameRateStorage.Add(currentFrameRate);
                index = index + 1;
            } else {
                permanentStorage.Add(temporaryFrameRateStorage.Average());
                avgFrameRate =  Mathf.Round((float)permanentStorage.Average());
                samples = permanentStorage.Count; // Calculate the number of samples
                displayTextAverage.text =avgFrameRate.ToString() + " FPS (AVG based on " +  samples+ ")";
                // Reset
                index = 0;
                temporaryFrameRateStorage = new List<int>();
            }
        }
    }
