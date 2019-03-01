using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Globals : MonoBehaviour {

    public const float SCORE_DURATION = 1.0f;

    public static bool gameOver, paused;
    public static int score;
    public static int highScore;
    public static float elapsedTime;    // Game time in seconds, not counting pause
    public static float createTime, decreaseTimer;
    public static float allowWhite, allowGrey;
    public static int warningThreshold;
    public static float cubeSize;
    public static ArrayList group;

    [SerializeField] private Text endText;
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private GameObject retryBtn;
    [SerializeField] private Material warningMat;
    [SerializeField] private Color warningColour = Color.black;
    [SerializeField] private Renderer overlayRend;
    [SerializeField] private float minSwipeLength = 100f;

    private Vector2 pressPosInit;
    private Vector2 pressPosEnd;
    private Vector3 currentSwipe;
    private Ray ray;
    private RaycastHit hit;
    

    // Setup start of level settings
    protected void Awake()
    {
        createTime = 2.0f;
        decreaseTimer = 0;
        elapsedTime = 0;
        warningThreshold = 6;
        allowWhite = 120;   // Time until white and grey blocks begin spawning
        allowGrey = 50;
        cubeSize = 0.5f;
        score = 0;
        gameOver = false;
        paused = false;
        highScore = PlayerPrefs.GetInt("highScore", highScore);
        group = new ArrayList();
        Material mat = pauseOverlay.GetComponent<Renderer>().material;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.5f);
        overlayRend.enabled = false;
        endText.text = null;
        StartCoroutine(ReduceTime());
    }

    // Unselect all cubes in the group and remove them from the arraylist
    public static void ClearGroup()
    {
        foreach (GameObject cube in group)
            cube.GetComponent<CubeProperties>().selected = false;
        group.Clear();
    }

    // Set the previous position of all selected cubes to be their current position
    public static void SetPos()
    {
        foreach (GameObject cube in group)
            cube.GetComponent<CubeProperties>().prevPos = cube.transform.position;
    }

    // Compare if the current position of all cubes in the group are the same as their previous position
    // Return false if a cube has moved
    public static bool CheckPos()
    {
        foreach (GameObject cube in group)
            if (cube.GetComponent<CubeProperties>().prevPos != cube.transform.position)
                return false;
        return true;
    }

    // Check to see if any cube in the selected group is falling
    // Return true if any are falling
    private bool CheckFall()
    {
        foreach (GameObject cube in group)
            if (cube.GetComponent<CubeProperties>().fall)
                return true;
        return false;   
    }

    // Swipe down to increase fall speed
    void Swipe() {
        if (Input.touches.Length > 0) {
            Touch t = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(t.position);
            if (t.phase == TouchPhase.Began) { 
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.transform.tag == "Cube") {
                        pressPosInit = new Vector2(t.position.x, t.position.y);
                    }
                }
            }

            if (t.phase == TouchPhase.Ended) {
                pressPosEnd = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(pressPosEnd.x - pressPosInit.x, pressPosEnd.y - pressPosInit.y);
            }

            // Must be a swipe and not a tap
            if (currentSwipe.magnitude > minSwipeLength) {
                currentSwipe.Normalize();
                if (currentSwipe.y < 0) // && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f
                    hit.transform.GetComponent<CubeProperties>().fallSpeed = 4;
            }
        }
    }

    void FixedUpdate() {
        //swipe();
    }

    // Update is called once per frame
    void Update () {
        // Dragging cubes downwards to speed up game also reduces cooldown between spawns
        if (decreaseTimer >= 3) {
            createTime -= 0.005f;
            decreaseTimer = 0;
        }

        // Clear selected group if a cube in group is falling
        if (CheckFall() && group.Count >= 3)
            ClearGroup();

        if (paused) {
            overlayRend.enabled = true;
            Time.timeScale = 0;
        }
        else {
            overlayRend.enabled = false;
            Time.timeScale = 1;
        }
        elapsedTime += Time.deltaTime;
        if (gameOver) {
            endText.text = "Game Over \n Score: " + score;
            if (score > highScore) {
                highScore = score;
                PlayerPrefs.SetInt("highScore", highScore);
            }
            paused = true;
            retryBtn.SetActive(true);
        }

        // Continiously Lerp the colour of warning frame between black and red
         warningColour = Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 1));
         warningMat.color = warningColour;

	}

    // Detect if app state is in the background
    void OnApplicationFocus(bool status) {
        if (!status)
            paused = true;
    }

    // Reduce time in between each cube spawn
    IEnumerator ReduceTime() {
        yield return new WaitForSeconds(2.0f);
        if (createTime >= 0.6)
            createTime -= 0.005f;
        StartCoroutine(ReduceTime());
    }
}