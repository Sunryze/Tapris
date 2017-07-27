using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Globals : MonoBehaviour {

    public static bool gameOver, paused;
    public static int score;
    public static float fallSpeed, createTime;
    public Text endText;
    public static ArrayList group;
    public static ArrayList pos;
    public GameObject pauseOverlay;
    private Renderer overlayRend;
    private Vector2 pressPosInit;
    private Vector2 pressPosEnd;
    private Vector3 currentSwipe;
    private float minSwipeLength = 50f;
    private Ray ray;
    private RaycastHit hit;

    // Setup start of level settings
    protected void Awake()
    {
        createTime = 2.0f;
        score = 0;
        gameOver = false;
        paused = false;
        group = new ArrayList();
        pos = new ArrayList();
        pauseOverlay = GameObject.FindGameObjectWithTag("overlay");
        Material mat = pauseOverlay.GetComponent<Renderer>().material;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.5f);
        overlayRend = pauseOverlay.GetComponent<Renderer>();
        overlayRend.enabled = false;
        endText.text = null;
        StartCoroutine(reduceTime());
    }

	// Use this for initialization
	void Start () {
	
	}

    // Unselect all cubes in the group and remove them from the arraylist
    public static void clearGroup()
    {
        foreach (GameObject cube in group)
            cube.GetComponent<CubeProperties>().selected = false;
        group.Clear();
        pos.Clear();
    }

    // Set the previous position of all selected cubes to be their current position
    public static void setPos()
    {
        foreach (GameObject cube in group)
            cube.GetComponent<CubeProperties>().prevPos = cube.transform.position;
    }

    // Compare if the current position of all cubes in the group are the same as their previous position
    // Return false if a cube has moved
    public static bool checkPos()
    {
        foreach (GameObject cube in group)
            if (cube.GetComponent<CubeProperties>().prevPos != cube.transform.position)
                return false;
        return true;
    }

    // Check to see if any cube in the selected group is falling
    // Return true if any are falling
    private bool checkFall()
    {
        foreach (GameObject cube in group)
            if (cube.GetComponent<CubeProperties>().fall)
                return true;
        return false;   
    }


    void swipe() {
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
        swipe();
    }

    // Update is called once per frame
    void Update () {
        // Clear selected group if a cube in group is falling
        if (checkFall() && group.Count >= 3)
            clearGroup();

        if (paused)
        {
            overlayRend.enabled = true;
            Time.timeScale = 0;
        }
        else
        {
            overlayRend.enabled = false;
            Time.timeScale = 1;
        }

        if (gameOver)
        {
            endText.text = "Game Over \n Score: " + Globals.score;
            paused = true;
        }
	}

    // Reduce time inbetween each cube spawn
    IEnumerator reduceTime()
    {
        yield return new WaitForSeconds(2.0f);
        if (createTime >= 0.5)
            createTime -= 0.005f;
        StartCoroutine(reduceTime());
    }
}