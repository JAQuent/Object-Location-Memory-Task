using System.IO;
using UnityEngine;
using UnityEditor;

#if ENABLE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
#endif


namespace AmazingAssets.RenderMonster
{
    [CustomEditor(typeof(RenderMonster))]
    public class RenderMonsterEditorWindow : Editor
    {
        static public RenderMonsterEditorWindow get;

        static Texture iconPlayOn, iconPlayOff, icon_screenshot;

        RenderMonster _target;

        SerializedProperty outputPath;
        SerializedProperty filePrefix;
        SerializedProperty superSize;

        SerializedProperty beginRecordingMode;
        SerializedProperty stopRecordingMode;
        SerializedProperty nFrame;
        SerializedProperty nSec;
        SerializedProperty fPS;



        void OnEnable()
        {
            // Setup the SerializedProperties.
            outputPath = serializedObject.FindProperty("outputPath");
            filePrefix = serializedObject.FindProperty("filePrefix");
            superSize = serializedObject.FindProperty("superSize");

            fPS = serializedObject.FindProperty("fPS");
            beginRecordingMode = serializedObject.FindProperty("beginRecordingMode");
            stopRecordingMode = serializedObject.FindProperty("stopRecordingMode");
            nFrame = serializedObject.FindProperty("nFrame");
            nSec = serializedObject.FindProperty("nSec");


            if (iconPlayOn == null)
                iconPlayOn = UnityEditor.EditorGUIUtility.IconContent("PauseButton").image;

            if (iconPlayOff == null)
                iconPlayOff = UnityEditor.EditorGUIUtility.IconContent("PlayButton").image;


            if (icon_screenshot == null)
                icon_screenshot = UnityEditor.EditorGUIUtility.IconContent("RawImage Icon").image;
        }

        public void ApplyModifiedProperties()
        {
            serializedObject.ApplyModifiedProperties();
            Repaint();
        }


        public override void OnInspectorGUI()
        {
            _target = (RenderMonster)target;

            serializedObject.Update();


            DrawSettings();

            DrawImageSequence();

            DrawScreenshot();


            serializedObject.ApplyModifiedProperties();
        }


        void DrawImageSequence()
        {
            GUILayout.Space(5);
            RenderMonsterEditorGUIHelper.Header("Image Sequence", Color.white);

            Rect drawRect;
            drawRect = GUILayoutUtility.GetLastRect();

            using (new AmazingAssets.EditorGUIUtility.EditorGUIIndentLevel(1))
            {
                using (new AmazingAssets.EditorGUIUtility.GUILayoutBeginHorizontal())
                {
                    EditorGUILayout.PropertyField(beginRecordingMode, new GUIContent("Begin Recording"));

                    EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(50));
                }
                using (new AmazingAssets.EditorGUIUtility.GUILayoutBeginHorizontal())
                {
                    EditorGUILayout.PropertyField(stopRecordingMode, new GUIContent("Stop Recording"));

                    EditorGUILayout.LabelField(" ", GUILayout.MaxWidth(50));
                }

                drawRect = GUILayoutUtility.GetLastRect();

                using (new AmazingAssets.EditorGUIUtility.GUIEnabled(Application.isPlaying ? true : false))
                {
                    using (new AmazingAssets.EditorGUIUtility.GUIBackgroundColor(_target.IsRecording() ? Color.red : Color.white))
                    {
                        if (GUI.Button(new Rect(drawRect.xMax - 50, drawRect.yMin - 18, 50, 33), _target.IsRecording() ? iconPlayOn : iconPlayOff))
                        {
                            if (_target.IsRecording())
                                _target.StopRecording();
                            else
                                _target.BeginRecording();
                        }
                    }
                }


                if ((beginRecordingMode.enumValueIndex == (int)RenderMonster.BEGIN_RECORDING.ByHotkey ||
                      stopRecordingMode.enumValueIndex == (int)RenderMonster.STOP_RECORDING.ByHotkey))
                {
                    EditorGUILayout.LabelField("Hotkey");

                    drawRect = GUILayoutUtility.GetLastRect();
                    drawRect.xMin += UnityEditor.EditorGUIUtility.labelWidth;

                    if (GUI.Button(drawRect, _target.recordingHotkey.ToString(), EditorStyles.popup))
                    {
                        RenderMonsterKeyCodeWindow.ShowWindow(GUIUtility.GUIToScreenPoint(new Vector2(drawRect.xMin, drawRect.yMax + 2)), UpdatePauseResumeKeyCode);
                    }
                }

                if (stopRecordingMode.enumValueIndex == (int)RenderMonster.STOP_RECORDING.AfterNFrame)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(nFrame);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (nFrame.intValue < 1)
                            nFrame.intValue = 1;
                    }
                }

                if (stopRecordingMode.enumValueIndex == (int)RenderMonster.STOP_RECORDING.AfterNSec)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(nSec);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (nSec.intValue < 1)
                            nSec.intValue = 1;
                    }
                }


                EditorGUILayout.LabelField("FPS");
                drawRect = GUILayoutUtility.GetLastRect();
                drawRect.xMax -= 54;
                EditorGUI.BeginChangeCheck();
                fPS.intValue = EditorGUI.IntField(drawRect, " ", fPS.intValue);
                if (EditorGUI.EndChangeCheck())
                {
                    if (fPS.intValue < 1)
                        fPS.intValue = 1;
                }


                drawRect.xMin = drawRect.xMax + 4;
                drawRect.width = 50;
                if (GUI.Button(drawRect, "...", EditorStyles.popup))
                {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("1"), false, MenuCallback_FrameRate, 1);
                    menu.AddItem(new GUIContent("5"), false, MenuCallback_FrameRate, 5);
                    menu.AddItem(new GUIContent("10"), false, MenuCallback_FrameRate, 10);
                    menu.AddItem(new GUIContent("15"), false, MenuCallback_FrameRate, 15);
                    menu.AddItem(new GUIContent("25"), false, MenuCallback_FrameRate, 25);
                    menu.AddItem(new GUIContent("30"), false, MenuCallback_FrameRate, 30);
                    menu.AddItem(new GUIContent("60"), false, MenuCallback_FrameRate, 60);
                    menu.AddItem(new GUIContent("120"), false, MenuCallback_FrameRate, 120);

                    menu.ShowAsContext();
                }
            }
        }

        void DrawScreenshot()
        {
            GUILayout.Space(5);
            RenderMonsterEditorGUIHelper.Header("Screenshot", Color.white);


            using (new AmazingAssets.EditorGUIUtility.EditorGUIIndentLevel(1))
            {
                EditorGUILayout.LabelField("Hotkey");

                Rect drawRect = GUILayoutUtility.GetLastRect();
                drawRect.xMin += UnityEditor.EditorGUIUtility.labelWidth;

                if (GUI.Button(new Rect(drawRect.xMin, drawRect.yMin, drawRect.width - 54, drawRect.height), _target.screenshotHotkey.ToString(), EditorStyles.popup))
                {
                    RenderMonsterKeyCodeWindow.ShowWindow(GUIUtility.GUIToScreenPoint(new Vector2(drawRect.xMin, drawRect.yMax + 2)), UpdateScreenshotKeyCode);
                }


                drawRect = EditorGUILayout.GetControlRect();
                using (new AmazingAssets.EditorGUIUtility.GUIEnabled(IsOutputFolderValid(outputPath.stringValue)))
                {
                    if (GUI.Button(new Rect(drawRect.xMax - 50, drawRect.yMin - 18, 50, 33), new GUIContent(icon_screenshot, "Screenshot")))
                    {
                        _target.CaptureScreenshot();

                        PlayShutterSound();
                    }
                }
            }
        }

        void DrawSettings()
        {
            GUILayout.Space(5);
            RenderMonsterEditorGUIHelper.Header("Output", Color.white);

            using (new AmazingAssets.EditorGUIUtility.EditorGUIIndentLevel(1))
            {
                using (new AmazingAssets.EditorGUIUtility.EditorGUILayoutBeginHorizontal())
                {
                    using (new AmazingAssets.EditorGUIUtility.GUIBackgroundColor(IsOutputFolderValid(outputPath.stringValue) ? Color.white : Color.red))
                    {
                        EditorGUILayout.PropertyField(outputPath, new GUIContent("Path"));
                    }

                    if (GUILayout.Button("Select", GUILayout.MaxWidth(50)))
                    {
                        GUIUtility.keyboardControl = 0;


                        string newPathName = outputPath.stringValue;
                        newPathName = UnityEditor.EditorUtility.OpenFolderPanel("Save textures to directory", Directory.Exists(newPathName) ? newPathName : Application.dataPath, string.Empty);
                        if (IsOutputFolderValid(newPathName))
                        {
                            outputPath.stringValue = newPathName;
                            
                        }
                    }
                }


                using (new AmazingAssets.EditorGUIUtility.EditorGUILayoutBeginHorizontal())
                {
                    EditorGUILayout.PropertyField(filePrefix);

                    if (GUILayout.Button("...", EditorStyles.popup, GUILayout.MaxWidth(50)))
                    {
                        GenericMenu menu = new GenericMenu();

                        menu.AddItem(new GUIContent("None"), false, MenuCallback_FilePrefix, string.Empty);
                        menu.AddSeparator(string.Empty);

                        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                        if (string.IsNullOrEmpty(sceneName))
                            sceneName = "Untitled";
                        menu.AddItem(new GUIContent("Scene Name (" + sceneName + ")"), false, MenuCallback_FilePrefix, sceneName);

                        string objectName = Selection.activeGameObject == null ? string.Empty : Selection.activeGameObject.name;
                        if (string.IsNullOrEmpty(objectName))
                            menu.AddDisabledItem(new GUIContent("Object Name"));
                        else
                            menu.AddItem(new GUIContent("Object Name (" + objectName + ")"), false, MenuCallback_FilePrefix, objectName);

                        menu.ShowAsContext();
                    }
                }

                superSize.intValue = EditorGUILayout.IntSlider("Super Size", superSize.intValue, 1, 32);
            }
        }


        void MenuCallback_FrameRate(object obj)
        {
            if (obj == null)
                return;

            fPS.intValue = (int)obj;


            ApplyModifiedProperties();
        }

        void MenuCallback_FilePrefix(object obj)
        {
            if (obj == null)
                return;

            filePrefix.stringValue = CorrectName(obj.ToString());

            ApplyModifiedProperties();
        }


#if ENABLE_INPUT_SYSTEM
        public void UpdatePauseResumeKeyCode(Key keyCode)
#else
        public void UpdatePauseResumeKeyCode(KeyCode keyCode)
#endif
        {
            Undo.RecordObject(_target, "Change KeyCode");
            _target.recordingHotkey = keyCode;
        }

#if ENABLE_INPUT_SYSTEM
        public void UpdateScreenshotKeyCode(Key keyCode)
#else
        public void UpdateScreenshotKeyCode(KeyCode keyCode)
#endif
        {
            Undo.RecordObject(_target, "Change KeyCode");
            _target.screenshotHotkey = keyCode;
        }


        bool HasDirectorySavePermition(string path)
        {
            byte[] bytes = new byte[] { 0, 1, 2, 3 };

            string fullPath = Path.Combine(path, "__TestFile__.txt");

            try
            {
                File.WriteAllBytes(fullPath, bytes);
            }
            catch (System.Exception)
            {

                return false;
            }

            File.Delete(fullPath);


            return true;
        }

        bool IsOutputFolderValid(string path)
        {
            if (string.IsNullOrEmpty(path) || Directory.Exists(path) == false)
                return false;
            else
                return true;
        }


        void PlayShutterSound()
        {
            //System.Media.SystemSounds.Beep.Play();            
        }

        public string CorrectName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            char[] invalidFileNameCharachters = Path.GetInvalidFileNameChars();
            foreach (var c in invalidFileNameCharachters)
            {
                name = name.Replace(c.ToString(), string.Empty);
            }

            return name;
        }
    }
}