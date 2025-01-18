using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject square;
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (square.transform.position.x + 3, transform.position.y, -10);
	}
}
