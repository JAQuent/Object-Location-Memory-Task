using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class create_Screenshots : MonoBehaviour
{
    public Camera cameraToUse; // Public variable to assign the camera
    public Vector3[] cameraPositions;
    public Vector3[] targetPoint;
    public float moveDuration = 2.0f; // Duration to move between positions
    public float screenshotInterval = 0.01666667f; // Interval between screenshots (60 FPS)
    public GameObject[] objectsToDisable; // List of game objects to disable
    public string externalPath = "C:/Users/alex/Desktop/screenshots"; // Path to save screenshots outside the Unity project

    // New variables for spawning game objects
    public GameObject[] objectsToSpawn; // List of game objects to spawn
    public Vector3[] spawnPositions; // List of positions to spawn the game objects

    // Private vars
    private int screenshotCount = 0;

    void Start()
    {
        // Check if the camera is assigned
        if (cameraToUse == null)
        {
            Debug.LogError("Camera not assigned. Please assign a camera in the Inspector.");
            return;
        }

        // Set the camera to the first position immediately
        if (cameraPositions.Length > 0)
        {
            cameraToUse.transform.position = cameraPositions[0];
            cameraToUse.transform.LookAt(targetPoint[0]);
        }

        // Spawn the specified game objects at the beginning
        SpawnGameObjects(objectsToSpawn, spawnPositions);

        // Disable the specified game objects
        DisableGameObjects(objectsToDisable);

        // Start the coroutine to move the camera and capture screenshots
        StartCoroutine(MoveCameraAndCaptureScreenshots());
    }

    void SpawnGameObjects(GameObject[] gameObjects, Vector3[] positions)
    {
        if (gameObjects.Length != positions.Length)
        {
            Debug.LogError("The number of game objects and positions must be the same.");
            return;
        }

        for (int i = 0; i < gameObjects.Length; i++)
        {
            Vector3 spawnPosition = new Vector3(positions[i].x, gameObjects[i].transform.position.y, positions[i].z);
            Instantiate(gameObjects[i], spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator MoveCameraAndCaptureScreenshots()
    {
        for (int i = 1; i < cameraPositions.Length; i++)
        {
            Vector3 startPosition = cameraToUse.transform.position;
            Vector3 endPosition = cameraPositions[i];
            float elapsedTime = 0;

            while (elapsedTime < moveDuration)
            {
                cameraToUse.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
                cameraToUse.transform.LookAt(targetPoint[i]);
                elapsedTime += Time.deltaTime;

                // Capture screenshot at the specified interval and update screenshotCount
                yield return StartCoroutine(CaptureScreenshot($"screenshot_{screenshotCount}.png"));
                screenshotCount++;
                yield return new WaitForSeconds(screenshotInterval);
            }

            // Ensure the camera reaches the final position
            cameraToUse.transform.position = endPosition;
            cameraToUse.transform.LookAt(targetPoint[i]);
        }
    }

    IEnumerator CaptureScreenshot(string fileName)
    {
        yield return new WaitForEndOfFrame();

        // Create a texture to hold the screenshot
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // Save the screenshot to a file
        byte[] bytes = screenshot.EncodeToPNG();
        string directoryPath = externalPath;
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        File.WriteAllBytes(Path.Combine(directoryPath, fileName), bytes);

        // Clean up
        Destroy(screenshot);
    }

    void DisableGameObjects(GameObject[] gameObjects)
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
