using UnityEngine;

public class AngleCalculator : MonoBehaviour
{
    // References to the game objects
    public GameObject objectA;
    public GameObject objectB;
    public GameObject referenceObject;

    void Update(){
        // Ensure the objects are set
        if (objectA == null || objectB == null || referenceObject == null){
            Debug.LogWarning("Please assign all game objects.");
            return;
        }

        // Get the vectors relative to the reference object
        Vector3 directionA = objectA.transform.position - referenceObject.transform.position;
        Vector3 directionB = objectB.transform.position - referenceObject.transform.position;

        // Project the vectors onto the reference object's plane
        directionA = Vector3.ProjectOnPlane(directionA, referenceObject.transform.up);
        directionB = Vector3.ProjectOnPlane(directionB, referenceObject.transform.up);

        // Calculate the angle between the two directions
        float angle = Vector3.SignedAngle(directionA, directionB, referenceObject.transform.up);

        // Display the angle in the console
        Debug.Log($"Angle between {objectA.name} and {objectB.name} relative to {referenceObject.name}: {angle}бу");
    }
}
