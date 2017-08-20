using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour {

    public Button pauseBtn;
    public Sprite resumeSprite;
    public Text helpText;
    public GameObject admob;
    private bool buffer;

    // Use this for initialization
    void Start () {

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
        if(Input.touches.Length > 1) {
            Globals.paused = false;
            pauseBtn.image.overrideSprite = null;
        }
        if(Globals.paused)
            pauseBtn.image.overrideSprite = resumeSprite;
        else
            pauseBtn.image.overrideSprite = null;
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
            
            //admob.GetComponent<AdManager>().playAd();
        }

        else
        {
            Globals.paused = false;
            
            helpText.GetComponent<Text>().enabled = false;
        }
    }

    public void help() {
        if (!Globals.paused) {
            Globals.paused = true;
            helpText.GetComponent<Text>().enabled = true;
        }
        else {
            Globals.paused = false;
            helpText.GetComponent<Text>().enabled = false;
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
