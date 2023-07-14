using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Wirting the script was assisted by ChatGPT and bing.

public class DrawLines : MonoBehaviour{
	// Public vars
	public GameObject sphere;
	public GameObject correctObject;
	public Vector3 correctLocation;
	public Vector3 arrowLocation;
	public GameObject arrowPrefab;
    public Vector3[] startPoints;
    public Vector3[] endPoints;
    public Color[] colors;

    // Private variables
    private GameObject currentSphere;

    private void Start(){
		// Get path to .csv files
        string path2file1 = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, "startPoints.csv"));
        string path2file2 = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, "endPoints.csv"));
        string path2file3 = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, "lineColours.csv"));
    	
    	// Read the start and end points
        startPoints = csv2Vector3(path2file1);
        endPoints = csv2Vector3(path2file2);
        colors = csv2Color(path2file3);

    	// Create the lines and add shere to the start of the line and
        for (int i = 0; i < startPoints.Length; i++){
        	// Create the lines
            GameObject line = new GameObject("Line" + i);
            line.transform.SetParent(transform);

            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = colors[i];
            lineRenderer.endColor = colors[i];
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;

            lineRenderer.SetPosition(0, startPoints[i]);
            lineRenderer.SetPosition(1, endPoints[i]);

            // Add the spheres
            currentSphere = Instantiate(sphere, startPoints[i], Quaternion.identity);
            Renderer renderer = currentSphere.GetComponent<Renderer>();
            renderer.material.color = colors[i];
        }

        // Spawn the correct object
        Instantiate(correctObject, correctLocation, Quaternion.identity);
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = new Vector3(correctLocation.x, arrow.transform.position.y, correctLocation.z);
    }

	// Function that reads a .csv file and converts to Vector3 array
	Vector3[] csv2Vector3(string filePath){
		// Read the CSV file
	    string[] lines = File.ReadAllLines(filePath);

		// Create a list to store Vector3 values
	    List<Vector3> vectorList = new List<Vector3>();

        // Iterate through each line in the CSV file
        for (int i = 0; i < lines.Length; i++){
            string[] values = lines[i].Split(',');

            if (values.Length >= 3){
                float x = float.Parse(values[0]);
                float y = float.Parse(values[1]);
                float z = float.Parse(values[2]);

                Vector3 vector = new Vector3(x, y, z);
                vectorList.Add(vector);
            }
            else{
                Debug.LogError("Invalid row format in CSV file at line " + (i + 1));
            }
        }

        // Convert the list to an array
        Vector3[] vectorArray = vectorList.ToArray();

        // Output the vectorArray to the console for testing
        for (int i = 0; i < vectorArray.Length; i++){
            Debug.Log("Vector[" + i + "]: " + vectorArray[i]);
        }

	    // Return
	    return vectorArray;
	}

	// The ConvertHexColorsToColors function takes a list of hex colors as strings 
	// and converts them into a Color array. It iterates through each hex color in the list, 
	// uses ColorUtility.TryParseHtmlString to parse the hex color string into a Color object, 
	// and stores it in the colorArray.
	Color[] ConvertHexColorsToColors(string[] hexColors){
        Color[] colorArray = new Color[hexColors.Length];

        for (int i = 0; i < hexColors.Length; i++){
            Color color;
            ColorUtility.TryParseHtmlString(hexColors[i], out color);
            colorArray[i] = color;
        }

        return colorArray;
    }

    // Function reading in hex colors as strings and converting to colour array
    Color[] csv2Color(string filePath){
    	// Read the CSV file to get the color values
	    string[] hexColors = File.ReadAllLines(filePath);

	    // Conver to array
	    Color[] colorArray = ConvertHexColorsToColors(hexColors);

	    // Return
	    return colorArray;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Return)){
            CaptureScreenshot();
        }
    }

    void CaptureScreenshot(){
        string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = Application.persistentDataPath + "/" + fileName;
        ScreenCapture.CaptureScreenshot(filePath);

        Debug.Log("Screenshot captured and saved: " + filePath);
    }
}


// How the shot was created:
// Deactivate everything between UXF_RIG and Player (including those two) 