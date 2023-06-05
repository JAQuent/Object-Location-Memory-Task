using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveChildrenPositions : MonoBehaviour
{
    private List<Transform> childrenList;

    private void Start()
    {
        childrenList = new List<Transform>();
        GetAllChildren(transform);

        SavePositionsToCSV();
    }

    private void GetAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            childrenList.Add(child);
            GetAllChildren(child);
        }
    }

    private void SavePositionsToCSV()
    {
        string csvFilePath = Application.dataPath + "/Positions.csv";
        Debug.Log("Information on children position saved: " + csvFilePath);

        using (StreamWriter writer = new StreamWriter(csvFilePath))
        {
            foreach (Transform child in childrenList)
            {
                string row = child.name + "," + child.position.x + "," + child.position.y + "," + child.position.z;
                writer.WriteLine(row);
            }
        }

        Debug.Log("Positions saved to: " + csvFilePath);
    }
}
