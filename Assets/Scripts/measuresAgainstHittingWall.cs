using UnityEngine;

// This script will automatically righten the player after falling down. 
public class measuresAgainstHittingWall : MonoBehaviour{
	// public static
	public static float tolerance = 3.0f;
    public static float respawnDistance = 15; // Distance toward the centre to which the player is moved if leaving the environment 
    public static bool leftEnvironment = false;

	// private vars
	private float x_pos;
	private float y_pos;
	private float z_pos;
	private float x_rot;
	private float y_rot;
	private float z_rot;
	private float diff_x;
	private float diff_z;
    private Vector2 centre = new Vector2(0, 0);

    // Update is called once per frame
    void Update(){
        // Check if left environment
        if(leftEnvironment){
            Debug.Log("Player left the environment.");
            respawnTowardsCentre();
        }

    	// Access rotation
    	x_rot = Mathf.Round(gameObject.transform.eulerAngles.x);
    	z_rot = Mathf.Round(gameObject.transform.eulerAngles.z);

    	// Calculate difference
    	diff_x = Mathf.Abs(Mathf.DeltaAngle(x_rot, 0.0f));
    	diff_z = Mathf.Abs(Mathf.DeltaAngle(z_rot, 0.0f));

    	// Check the current x_rot & z_rot
    	if(diff_x > tolerance | diff_z > tolerance){
    		// Log Entry
    		Debug.Log("Attempting to righten the player.");

    		// Get y rotation
    		y_rot = gameObject.transform.eulerAngles.y;

    		// Reset movement
    		ThreeButtonMovement.reset = true;

    		// Set player rotation
			gameObject.transform.rotation = Quaternion.Euler(0, y_rot, 0);
    	}

        // Check if the R button is clicked for manual reset
        if(Input.GetKeyDown(KeyCode.R) & ExperimentController.sessionStarted){
            // Log
            Debug.Log("The experimenter pressed R to manually rotated the participant by 180 degrees.");

            // Reset movement
            ThreeButtonMovement.reset = true;

            // Rotate by 180 degrees
            gameObject.transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);
        }       
    }

    // Respawn inside the arena again
    void respawnTowardsCentre(){
        // Get the current position & calculate the distance from the centre
        Vector2 currentPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        float currentDistance = Vector2.Distance(currentPosition, centre);

        // Calculate target distance from centre
        float targetDistance = currentDistance - respawnDistance;

        // Based on this target distance calculate the scalar with which the player position has to be multiplied
        float positionScalar = targetDistance/currentDistance;

        // Use scalar to scale position
        Vector2 scaledPosition = currentPosition*positionScalar;

        // Reset movement
        ThreeButtonMovement.reset = true;

        // Reposition the player
        gameObject.transform.position = new Vector3(scaledPosition.x, 1.0f, scaledPosition.y);

        // Reset boolean & report new player position
        leftEnvironment = false;
        Debug.Log("Player position has been reset to: " + gameObject.transform.position);
    }
}