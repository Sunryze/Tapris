using UnityEngine;
using System.Collections;

public class CubeProperties : MonoBehaviour {

    public bool draggable = true;
    public bool fall = true;
    public bool dragLeft, dragRight;
    public bool selected;
    public float fallSpeed = 2;
    public Vector3 prevPos;
    public Color colour;
    public Material mat;
    public Material matSelected;
    private RaycastHit hit;
    private float size;
    private float maxDrag = 0.6f;
    private float distance_to_screen;
    private float offsetX;
    private int scoreInc;

	// Use this for initialization
	void Start () {
        size = transform.localScale.x/2;
	}

    void OnMouseDown() {
        selected = true;
        Globals.clearGroup();
        Globals.group.Add(transform.gameObject);
        prevPos = transform.position;
        if (!fall) {
            selectAdj(Vector3.left);
            selectAdj(Vector3.right);
            selectAdj(Vector3.up);
            selectAdj(Vector3.down);
        }
        distance_to_screen = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 temp_pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
        offsetX = transform.position.x - temp_pos.x;
    }


    void OnMouseDrag() {
        if(!Globals.paused) {
            Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));


            pos_move.x = pos_move.x + offsetX;


            // Check if an object is blocking this cube to the left
            // The +/- 0.01f is necessary to prevent collision detection when a cube is touching from above/below
            Vector3 originU = new Vector3(transform.position.x, transform.position.y + size - 0.01f + offsetX, transform.position.z);
            Vector3 originD = new Vector3(transform.position.x, transform.position.y - size + 0.01f + offsetX, transform.position.z);
            if (Physics.Raycast(originU, Vector3.left, size) || Physics.Raycast(originD, Vector3.left, size))
                dragLeft = false;
            else
                dragLeft = true;

            // Check if an object is blocking this cube to the right
            if (Physics.Raycast(originU, Vector3.right, size) || Physics.Raycast(originD, Vector3.right, size))
                dragRight = false;
            else
                dragRight = true;

            
            // Prevent dragging left or right if an object is blocking in that direction
            if ((!dragLeft && pos_move.x <= transform.position.x) || (!dragRight && pos_move.x >= transform.position.x))
                pos_move.x = transform.position.x;

            // Clamp maximum left drag 
            if (transform.position.x >= pos_move.x)
            if (transform.position.x - pos_move.x > maxDrag)
                pos_move.x = transform.position.x - maxDrag;
            if (pos_move.x < 0)
                pos_move.x = 0;
            
            // Clamp maximum right drag
            if (transform.position.x <= pos_move.x)
                if (pos_move.x - transform.position.x > maxDrag)
                    pos_move.x = transform.position.x + maxDrag;
            if (pos_move.x > 11)
                pos_move.x = 11;

            // Update to new adjusted position
            transform.position = new Vector3(pos_move.x, transform.position.y, pos_move.z);

        }
        
    }

    void OnMouseUp() {
        if (transform.position != prevPos)
            Globals.clearGroup();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 10)) {
            if (hit.transform.tag == "Cube") {
                if (hit.transform.GetComponent<CubeProperties>().selected) {
                    if (Globals.group.Count >= 3) {
                        scoreInc = -2;  // Begin at -2 so once 3 are counted the points start at 1
                        foreach (GameObject i in Globals.group)
                        {
                            Destroy(i);
                            scoreInc++;
                        }
                        // 3 points for 3 cubes and 3 additional points per extra cube
                        Globals.score += (scoreInc * 3);
                    }
                    selected = false;
                }
            }
        }
        Globals.clearGroup();

        transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, transform.position.z);

    }

    // Update is called once per frame
    void FixedUpdate() {
        if(!Globals.paused) {
            // Check to see if either side of the cube is blocked by an object downwards
            Vector3 originL = new Vector3(transform.position.x - size + 0.01f, transform.position.y, transform.position.z);
            Vector3 originR = new Vector3(transform.position.x + size - 0.01f, transform.position.y, transform.position.z);
            if (Physics.Raycast(originL, Vector3.down, size) || Physics.Raycast(originR, Vector3.down, size)) {
                fall = false;
                fallSpeed = 2;
            }
            else
                fall = true;

            Vector3 originU = new Vector3(transform.position.x, transform.position.y + size - 0.01f, transform.position.z);
            Vector3 originD = new Vector3(transform.position.x, transform.position.y - size + 0.01f, transform.position.z);

            // Check if this cube is stuck in another cube, force move it away if so
            if(Physics.Raycast(originU, Vector3.left, size - 0.01f) || Physics.Raycast(originD, Vector3.left, size - 0.01f))
                transform.position = new Vector3(Mathf.Ceil(transform.position.x), transform.position.y, transform.position.z);

            if (Physics.Raycast(originU, Vector3.right, size - 0.01f) || Physics.Raycast(originD, Vector3.right, size - 0.01f))
                transform.position = new Vector3(Mathf.Floor(transform.position.x), transform.position.y, transform.position.z);

            // Only needed for touch input DragCube.cs
            /*
            if (!dragRight && !dragLeft && !fall)
                draggable = false;
            else
                draggable = true;
            */


            // Translate object downwards if nothing is blocking it below
            if (fall)
                transform.Translate(Vector3.down * Time.fixedDeltaTime * fallSpeed);

            // End the game if an object has stopped falling above the field
            if (!fall && transform.position.y >= 9)
                Globals.gameOver = true;

            // Prevent cube from going out of bounds
            if (transform.position.x < 0)
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
            if (transform.position.x > 11)
                transform.position = new Vector3(11, transform.position.y, transform.position.z);

            // Change this cube's frame colour
            if (selected)
                transform.GetChild(0).GetComponent<Renderer>().material = matSelected;
            else
                transform.GetChild(0).GetComponent<Renderer>().material = mat;
        }
        
	}

    // Select any cubes that are adjacent to the current one in 3 directions (excluding the previous one)
    // Uses recursion to add each cube to the global group
    public void selectAdj(Vector3 dir) {
        if (Physics.Raycast(transform.position, dir, out hit, size)) {
            if (hit.transform.tag == "Cube") { 
                CubeProperties prop = hit.transform.GetComponent<CubeProperties>();
                if (hit.transform.GetComponent<CubeProperties>().colour == colour && !prop.fall) {
                    if (!hit.transform.GetComponent<CubeProperties>().selected) {
                        hit.transform.GetComponent<CubeProperties>().selected = true;
                        Globals.group.Add(hit.transform.gameObject);
                
                        if (dir != Vector3.left)
                            prop.selectAdj(Vector3.right);
                        if (dir != Vector3.right)
                            prop.selectAdj(Vector3.left);
                        if (dir != Vector3.up)
                            prop.selectAdj(Vector3.down);
                        if (dir != Vector3.down)
                            prop.selectAdj(Vector3.up);
                    }
                }
            }
        }
    }

}


/*
// Mouse click or touch input
if (Input.GetMouseButtonDown(0))
{
    if (Physics.Raycast(ray, out hit, 10))
    {
        if (hit.transform == transform)
        {
            // If not selected, select this and any of similar colour that are adjacent to each other
            // only selects if any of them are not falling
            if (!selected)
            {
                selected = true;
                Globals.clearGroup();
                Globals.group.Add(transform.gameObject);
                Globals.setPos();
                if (!fall)
                {
                    selectAdj(Vector3.left);
                    selectAdj(Vector3.right);
                    selectAdj(Vector3.up);
                    selectAdj(Vector3.down);
                }
            }
            else
            {

            }
        }
        else
        {
            // Unselect any that aren't the one targeted
            if (selected && Globals.group.Contains(transform))
            {
                selected = false;
                Globals.clearGroup();
            }
        }

    }
    // Unselect all when a cube is not pressed
    else
    {
        selected = false;
        Globals.clearGroup();
    }
}*/
