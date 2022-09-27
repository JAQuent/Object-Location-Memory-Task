    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using System.Linq;
    // Inspired by https://forum.unity.com/threads/fps-counter.505495/ 

    public class FPS_counter: MonoBehaviour{
        public int currentFrameRate;
        public double avgFrameRate;
        public int samples;
        public Text displayTextCurrent;
        public Text displayTextAverage;
        public int index1 = 0;
        public float refreshRate = 0.2f;
        private float timer;
        public int temporaryStorageCapacity = 500; 
        private List<int> temporaryFrameRateStorage = new List<int>();
        private List<double> permanentStorage = new List<double>();

        void Start(){
            // Set timer to refresh rate
            timer = refreshRate;
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
                displayTextAverage.text =avgFrameRate.ToString() + " FPS (AVG based on " + samples + ")";
                
                // Reset the values
                index1 = 0;
                temporaryFrameRateStorage = new List<int>();
            }
        }
    }
