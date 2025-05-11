using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualisationCreator : MonoBehaviour{
    // Public variables to set in the inspector
    public Vector3 cameraPosition = new Vector3(0, 5, -10);
    public Quaternion cameraRotation = Quaternion.Euler(20, 0, 0);
    public List<GameObject> sceneObjectsToInstantiate = new List<GameObject>();
    public List<Vector3> objectPositions = new List<Vector3>();
    public List<Quaternion> objectRotations = new List<Quaternion>();
    public GameObject arrow; 

    // Private variables
    private Camera camera;
    private GameObject[] objectsInTheScene;
    private string screenshotFolderPath;
    private string csvFilePath;

    // Start is called before the first frame update
    void Start(){
        // Create a list of game object names
        List<string> sceneObjects2Delete = new List<string>(){
            "[UXF_Rig]",
            "Experiment",
            "Canvas",
            "FPS_Counter",
            "Player"
        };

        // Deactivate game objects in the scene
        foreach (string name in sceneObjects2Delete){
            GameObject obj = GameObject.Find(name);
            if (obj != null){
                obj.SetActive(false);
            }
        }

        // Find or create a camera and make it a child of this game object
        camera = GetComponentInChildren<Camera>();
        if (camera == null){
            // If no camera is found, create a new one
            GameObject cameraObject = new GameObject("ChildCamera");
            camera = cameraObject.AddComponent<Camera>();

            // Set the camera as a child of this game object
            cameraObject.transform.SetParent(transform);
        }

        // Change the camera's position relative to the parent
        camera.transform.localPosition = cameraPosition;
        camera.transform.localRotation = cameraRotation;

        // Instantiate the objects in the scene
        objectsInTheScene = new GameObject[sceneObjectsToInstantiate.Count];
        for (int i = 0; i < sceneObjectsToInstantiate.Count; i++){
            objectsInTheScene[i] = Instantiate(sceneObjectsToInstantiate[i], objectPositions[i], objectRotations[i]);
            
            // Add arrow to indicate where object is and move over to correct location
            if (i == 0){
                arrow = Instantiate(arrow);
                arrow.transform.position = new Vector3(objectPositions[i].x, arrow.transform.position.y, objectPositions[i].z);
                // Disable the arrowScript attached to the game object
                arrow.GetComponent<arrowScript>().enabled = false;

            }
        }

        // Set the screenshot folder path
        screenshotFolderPath = Application.streamingAssetsPath + "/Screenshots";
        if (!System.IO.Directory.Exists(screenshotFolderPath)){
            System.IO.Directory.CreateDirectory(screenshotFolderPath);
        }

        // Set the CSV file path
        csvFilePath = Application.streamingAssetsPath + "/positions.csv";
        if (!System.IO.File.Exists(csvFilePath)){
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(csvFilePath)){
                writer.WriteLine("ImageName,CameraPosition,CameraRotation,ObjectPositions,ObjectRotations");
            }
        }
    }

    // Update is called once per frame
    void Update(){
        // Update the camera's position if it has changed
        if (camera.transform.localPosition != cameraPosition){
            camera.transform.localPosition = cameraPosition;
        }

        // Update the camera's rotation if it has changed
        if (camera.transform.localRotation != cameraRotation){
            camera.transform.localRotation = cameraRotation;
        }

        // Update the object positions if they have changed
        for (int i = 0; i < sceneObjectsToInstantiate.Count; i++){
            if (objectsInTheScene[i].transform.position != objectPositions[i]){
                objectsInTheScene[i].transform.position = objectPositions[i];

                // Update the arrow position if it is the first object
                if (i == 0){
                    arrow.transform.position = new Vector3(objectPositions[i].x, arrow.transform.position.y, objectPositions[i].z);
                }
            }
        }

        // Update the object rotations if they have changed
        for (int i = 0; i < sceneObjectsToInstantiate.Count; i++){
            if (objectsInTheScene[i].transform.rotation != objectRotations[i]){
                objectsInTheScene[i].transform.rotation = objectRotations[i];
            }
        }

        // Take a screenshot when space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space)){
            string imageName = "screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            string imagePath = System.IO.Path.Combine(screenshotFolderPath, imageName);
            ScreenCapture.CaptureScreenshot(imagePath);

            // Append the image name and positions/rotations to the CSV file
            using (System.IO.StreamWriter writer = System.IO.File.AppendText(csvFilePath)){
                string cameraPositionString = cameraPosition.ToString("F6");
                string cameraRotationString = cameraRotation.eulerAngles.ToString("F6");
                string objectPositionsString = string.Join(";", objectPositions.ConvertAll(v => v.ToString("F6")).ToArray());
                string objectRotationsString = string.Join(";", objectRotations.ConvertAll(q => q.eulerAngles.ToString("F6")).ToArray());

                writer.WriteLine($"{imageName},{cameraPositionString},{cameraRotationString},{objectPositionsString},{objectRotationsString}");
            }

            // Print the screenshot information to the console
            Debug.Log("Screenshot taken: " + imageName);
            Debug.Log("Saved at: " + imagePath);
        }
    }
}
