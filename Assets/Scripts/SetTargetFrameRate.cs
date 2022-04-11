 using UnityEngine;
 

 // Source https://answers.unity.com/questions/1366716/how-to-liimit-fps.html
 public class SetTargetFrameRate : MonoBehaviour{
     public static int targetFrameRate = 60;
 
     private void Start(){
         QualitySettings.vSyncCount = 0;
         Application.targetFrameRate = targetFrameRate;
     }
 }