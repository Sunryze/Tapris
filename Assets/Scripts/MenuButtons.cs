using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}

}
