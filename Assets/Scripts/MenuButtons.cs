﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButtons : MonoBehaviour {

    private bool buffer;
    public int highScore;
    public GameObject scoreText;

	// Use this for initialization
	void Start () {
        highScore = PlayerPrefs.GetInt("highScore", highScore);
        scoreText.GetComponent<Text>().text = "Highscore: " + highScore;
	}

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void exitGame() {
        Application.Quit();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (buffer)
                Application.Quit();
            else {
                AndroidNativeFunctions.ShowToast("Press again to exit");
                buffer = true;
                StartCoroutine(bufferTime());
            }
        }
    }

    IEnumerator bufferTime() {
        yield return new WaitForSeconds(3);
        buffer = false;
    }

}
