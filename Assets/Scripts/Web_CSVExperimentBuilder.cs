using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UXF;

public class Web_CSVExperimentBuilder : MonoBehaviour{
    // Inspired by https://stackoverflow.com/questions/43693213/application-streamingassetspath-and-webgl-build
    public static IEnumerator TableFromCSV(string csvPath, Session session, bool copyToResults){
        Debug.Log("Start download...");
        Debug.Log("Path: " + csvPath);
        // download file from StreamingAssets folder
        UnityWebRequest www = UnityWebRequest.Get(csvPath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success){
            Debug.LogError("Error downloading CSV file: " + www.error);
            yield break;
        }

        string csvText = www.downloadHandler.text;
        string[] csvLines = csvText.Split("\n");
        // Loop through the lines
        for (int i = 0; i < csvLines.Length; i++){
            csvLines[i] = csvLines[i].Replace("\r", String.Empty);
        }

        // parse as table
        UXFDataTable table = UXFDataTable.FromCSV(csvLines);

        // build the experiment.
        // this adds a new trial to the session for each row in the table
        // the trial will be created with the settings from the values from the table
        // if "block_num" is specified in the table, the trial will be added to the block with that number
        session.BuildFromTable(table, copyToResults);

        // Log
        Debug.Log("Finished download and created table...");

        // Set experiment start to true
        ExperimentController.startExperimentOnline = true;
    }
}
