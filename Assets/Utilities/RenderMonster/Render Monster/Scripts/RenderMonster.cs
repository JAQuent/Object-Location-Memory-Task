using System.IO;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
#endif


namespace AmazingAssets 
{ 
    namespace RenderMonster 
    {
        [RequireComponent(typeof(Camera))]
        [AddComponentMenu("Amazing Assets/Render Monster")]
        public class RenderMonster : MonoBehaviour 
        {
            public enum BEGIN_RECORDING { OnStart, ByHotkey, Manually }
            public enum STOP_RECORDING { ByHotkey, AfterNFrame, AfterNSec, Manually } 


            public string outputPath; 
            public string filePrefix;
            public int superSize = 1;  

            public BEGIN_RECORDING beginRecordingMode = BEGIN_RECORDING.ByHotkey;
            public STOP_RECORDING stopRecordingMode = STOP_RECORDING.ByHotkey;

#if ENABLE_INPUT_SYSTEM
            public Key recordingHotkey = Key.F12;
#else
            public KeyCode recordingHotkey = KeyCode.F12;
#endif

            public int nFrame = 300;
            public int nSec = 10;
            public int fPS = 30;

#if ENABLE_INPUT_SYSTEM
            public Key screenshotHotkey = Key.F5;
#else
            public KeyCode screenshotHotkey = KeyCode.F5;
#endif

            bool isRecording;
            int oldFPS;
            int nFrameCounter;

            string lastSavedFileName;



            void Start()
            {
                if (beginRecordingMode == BEGIN_RECORDING.OnStart)
                    BeginRecording();
            }
            
            void OnDestroy()
            {

            }
             
            void Update()
            {
                CaptureImageSequence();

                if (IsScreenShotHotKeyDown())
                    CaptureScreenshot();
            }

            public void BeginRecording()
            {
                if(string.IsNullOrEmpty(outputPath))
                {
                    Debug.LogError("Render Monster: Can not capture image sequence. Output directory is not defined.\n");
                    return;
                }


                if (isRecording == false)
                    isRecording = true;
                else
                    return;


                if (Directory.Exists(outputPath) == false)
                    Directory.CreateDirectory(outputPath);

                if(Directory.Exists(outputPath) == false)
                {
                    Debug.Log("Render Monster: Can not capture image sequence. Directory '" + outputPath + "' does not exist.\n");

                    isRecording = false;
                    return;
                }

                Debug.Log("Render Monster: Begin Recording.\n");
#if UNITY_EDITOR
                //Repaint editor to highlight buttons
                UnityEditor.EditorUtility.SetDirty(UnityEditor.Selection.activeGameObject);
#endif


                superSize = Mathf.Clamp(superSize, 1, 32);

                nFrameCounter = 0;


                //Set playback framerate
                oldFPS = Time.captureFramerate;
                Time.captureFramerate = fPS;
            }

            public void StopRecording()
            {
                if (isRecording == true)
                    isRecording = false;
                else
                    return;


                Debug.Log("Render Monster: Stop Recording. (" + nFrameCounter + ") frames captured.\n");
#if UNITY_EDITOR
                //Repaint editor to highlight buttons
                UnityEditor.EditorUtility.SetDirty(UnityEditor.Selection.activeGameObject);
#endif


                nFrameCounter = 0;

                //Restore playback framerate
                Time.captureFramerate = oldFPS;
            }

            public bool IsRecording()
            {
                return isRecording;
            }

             

            void CaptureImageSequence()
            {
                if (isRecording)
                {
                    if ((stopRecordingMode == STOP_RECORDING.ByHotkey && IsRecordingHotKeyDown()) ||
                        (stopRecordingMode == STOP_RECORDING.AfterNFrame && nFrameCounter > nFrame) ||
                        (stopRecordingMode == STOP_RECORDING.AfterNSec && nFrameCounter > nSec * fPS))
                    {
                        StopRecording();                        
                    }
                }
                else 
                {
                    if (beginRecordingMode == BEGIN_RECORDING.ByHotkey && IsRecordingHotKeyDown())
                        BeginRecording();
                }


                if (isRecording == false)
                    return;

                ++nFrameCounter;
                ScreenCapture.CaptureScreenshot(GetSaveFileName(outputPath), superSize);                
            }

            public void CaptureScreenshot()
            { 
                if(string.IsNullOrEmpty(outputPath))
                {
                    Debug.LogError("Render Monster: Can not capture screenshot. Output directory is not defined.\n");
                    return;
                }


                string saveFolder = Path.Combine(outputPath, "Screenshot");
                if (Directory.Exists(saveFolder) == false)
                    Directory.CreateDirectory(saveFolder);

                if (Directory.Exists(saveFolder))
                {
                    string fileName = GetSaveFileName(saveFolder);
                    ScreenCapture.CaptureScreenshot(fileName, superSize);

                    Debug.Log("Render Monster: Screenshot saved at path.\n" + fileName + "\n");
                }
                else
                {
                    Debug.LogError("Render Monster: Can not capture screenshot. Directory '" + outputPath + "' does not exist.\n");
                }
            }

            bool IsRecordingHotKeyDown()
            {
#if ENABLE_INPUT_SYSTEM
                return Keyboard.current[recordingHotkey].wasPressedThisFrame;
#else
                return Input.GetKeyDown(recordingHotkey);
#endif
            }

            bool IsScreenShotHotKeyDown()
            {
#if ENABLE_INPUT_SYSTEM
                return Keyboard.current[screenshotHotkey].wasPressedThisFrame;
#else
                return Input.GetKeyDown(screenshotHotkey);
#endif
            }

            string GetSaveFileName(string path)
            {
                lastSavedFileName = Path.Combine(path, (string.IsNullOrEmpty(filePrefix) ? string.Empty : (filePrefix + "_")) + Time.frameCount + ".png");

                return lastSavedFileName;
            }


#if UNITY_EDITOR
            [ContextMenu("Open Save Folder")]
            public void OpenSaveFolder()
            {
                if (string.IsNullOrEmpty(outputPath) == false && Directory.Exists(outputPath))
                {
                    System.Diagnostics.Process[] localByName = System.Diagnostics.Process.GetProcessesByName(outputPath);

                    if (localByName == null || localByName.Length == 0)
                        System.Diagnostics.Process.Start(outputPath);
                }
                else
                {
                    Debug.LogError("Render Monster: Directory " + (string.IsNullOrEmpty(outputPath) ? string.Empty : ("'" + outputPath + "' ")) + "does not exist.\n");
                }
            }
#endif
        }
    }
}
