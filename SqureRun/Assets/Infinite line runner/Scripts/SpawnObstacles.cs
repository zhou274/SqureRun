using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour {

	public GameObject square;
	private List <GameObject> obstacles = new List<GameObject> ();
	public float obstacleLevel = 1;
	public float lastObstaclePosition = 20.48f;
	public int deletedObstacleLevel = 0;
	void Update () {
		if (square.transform.position.x + 20.48f >= lastObstaclePosition) {
			int randObstacle = UnityEngine.Random.Range (1, 11);
			obstacles.Add (Instantiate (Resources.Load ("obstacle"+ randObstacle), new Vector2 (lastObstaclePosition, 0), Quaternion.identity) as GameObject);
			obstacleLevel++;
			lastObstaclePosition = obstacleLevel * 20.48f;
			if (obstacles.Count > 2) {
				Destroy (obstacles [deletedObstacleLevel]);
				deletedObstacleLevel++;
			}
		}
	}
}
