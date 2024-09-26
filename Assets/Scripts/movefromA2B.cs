using UnityEngine;

public class movefromA2B : MonoBehaviour{
	// This script is only use to move the player to record a video while going past the objects
    public Vector3 pos1 = new Vector3(-7.7f, 0, 79.37f);
    public Vector3 pos2 = new Vector3(-7.7f, 0, -24.72f);
    public float speed = 0.1f;
    public bool startCamera = false;
    public float timeElapsed = 0;
 
    void Update() {
    	if(startCamera){
    		transform.position = Vector3.Lerp(pos1, pos2, timeElapsed * speed);
    		timeElapsed += Time.deltaTime;
    	}
    }
}
