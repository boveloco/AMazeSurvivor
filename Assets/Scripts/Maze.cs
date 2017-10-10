using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

	public Transform player;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("end"))
            player.position = new Vector3(19, 3, 313);
		if (Input.GetKeyDown("home"))
            player.position = new Vector3(-1, 1, 6);
	}
}
