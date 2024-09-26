using UnityEngine;

public class arrowScript : MonoBehaviour{
	// Public vars
    public float rotationSpeed = 30.0f;
	//adjust this to change how high it goes
	public float height = 4.59f;
	//adjust this to change speed
    public float speed = 5f;

    // Update is called once per frame
    void Update(){
    	// Rotate
        transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * rotationSpeed);

        //get the objects current position and put it in a variable so we can access it later with less code
        Vector3 pos = transform.position;
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed) + height;
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(pos.x, newY, pos.z);

        // Source https://forum.unity.com/threads/how-to-make-an-object-move-up-and-down-on-a-loop.380159/
    }
}
