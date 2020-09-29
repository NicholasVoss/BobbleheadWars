using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	
	public int rotationSpeed;
	
	void Update () {
		//rotate pickup
		transform.Rotate(new Vector3(0, rotationSpeed, 0)* Time.deltaTime);
	}
}
