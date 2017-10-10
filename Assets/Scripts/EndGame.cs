using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	
	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other)
	{
		var loader = gameObject.GetComponent<SceneLoader>();
		loader.loadScene("End");
	}

	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag);
		if(other.tag == "Player") {
			var loader = gameObject.GetComponent<SceneLoader>();
			loader.loadScene("End");
		}
	}
}
