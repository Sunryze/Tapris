using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour {

    public Button pauseBtn;
    public Sprite resumeSprite;
    private bool buffer;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (buffer)
                Application.Quit();
            else
            {
                AndroidNativeFunctions.ShowToast("Press again to exit");
                buffer = true;
                StartCoroutine(bufferTime());
            }

    }

    // Reload the game scene to restart level
    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void pause()
    {
        if (!Globals.paused)
        {
            Globals.paused = true;
            pauseBtn.image.overrideSprite = resumeSprite;
        }

        else
        {
            Globals.paused = false;
            pauseBtn.image.overrideSprite = null;
        }
    }

    public void menu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator bufferTime()
    {
        yield return new WaitForSeconds(3);
        buffer = false;
    }
}
