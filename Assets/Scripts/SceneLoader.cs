using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public void loadScene(string scene){
		SceneManager.LoadScene(scene);
	}

	public void exitGame(){
		Application.Quit();
	}
	
}
