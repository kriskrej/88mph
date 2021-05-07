using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

	void Update () {
	    var scenesCount = SceneManager.sceneCountInBuildSettings;
	    var currentScene = SceneManager.GetActiveScene().buildIndex;
	    if (Input.GetKeyUp("[+]")) {
	        if (currentScene + 1 == scenesCount)
	            SceneManager.LoadScene(0);
            else 
	            SceneManager.LoadScene(currentScene + 1);
	    }
	}
}
