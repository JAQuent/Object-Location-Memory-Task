using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move2script : MonoBehaviour{
	// Public var
	public bool move = false;
	public float speed = 3.0f;
	public Vector3 targetPosition;
    
    // Update is called once per frame
    void Update(){
        if(move){
        	transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
