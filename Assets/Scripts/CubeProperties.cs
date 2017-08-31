using UnityEngine;
using System.Collections;

public class CubeProperties : MonoBehaviour {

    public bool draggable = true;
    public bool fall = true;
    public bool dragLeft, dragRight;
    public bool selected, warning;
    public float fallSpeed = 2;
    public bool menuCube = false;
    public float distance_to_screen;
    public float offsetX;
    public Vector3 currentPos;
    public int scoreInc;
    public bool allowDragX, allowDragY;
    public GameObject wireFrame;
    public Vector3 prevPos;
    public Color colour;
    public Material mat;
    public Material matSelected;
    public Material matWarning;
    private RaycastHit hit;
    private float size;
    private float maxDragX = 0.6f;
    private float maxDragY = 0.08f;
    
	// Use this for initialization
	void Start () {
        size = transform.localScale.x/2;
	}

    /*void OnMouseDown() {
        if (!Globals.paused && !Globals.gameOver) {
            selected = true;
            allowDragX = false;
            allowDragY = false;
            prevPos = transform.position;
            // Clear previous selections and select this cube
            Globals.clearGroup();
            Globals.group.Add(transform.gameObject);
            // Select all connected cubes if this one isn't falling
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
    }


    void OnMouseDrag() {
        // Cubes can't be moved if game is paused or if cube is grey
        if(!Globals.paused && !Globals.gameOver && colour != new Color(0.5f, 0.5f, 0.5f)) {
            Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));

            pos_move.x = pos_move.x + offsetX;
            // Make sure object is dragged and not moved slightly
            if (Mathf.Abs(pos_move.x - prevPos.x) > 0.3f) {
                allowDragX = true;
            }
            if (allowDragX) {
                // Check if an object is blocking this cube to the left
                // The +/- 0.01f is necessary to prevent collision detection when a cube is touching from above/below
                Vector3 originU = new Vector3(transform.position.x, transform.position.y + size - 0.01f, transform.position.z);
                Vector3 originD = new Vector3(transform.position.x, transform.position.y - size + 0.01f, transform.position.z);
                if (Physics.Raycast(originU, Vector3.left, size) || Physics.Raycast(originD, Vector3.left, size))
                    dragLeft = false;
                else
                    dragLeft = true;

                // Check if an object is blocking this cube to the right
                if (Physics.Raycast(originU, Vector3.right, size) || Physics.Raycast(originD, Vector3.right, size))
                    dragRight = false;
                else
                    dragRight = true;

                
                // Raycast to see where objects are to the left and right
                // Prevent fast dragging from ignoring collision
                if (Physics.Raycast(originU, Vector3.left, out hit) || Physics.Raycast(originD, Vector3.left, out hit))
                    if (pos_move.x < hit.transform.position.x)
                        pos_move.x = hit.transform.position.x + 1;
                if (Physics.Raycast(originU, Vector3.right, out hit) || Physics.Raycast(originD, Vector3.right, out hit))
                    if (pos_move.x > hit.transform.position.x)
                        pos_move.x = hit.transform.position.x - 1;
                

                // Prevent dragging left or right if an object is blocking in that direction
                if ((!dragLeft && pos_move.x <= transform.position.x) || (!dragRight && pos_move.x >= transform.position.x))
                    pos_move.x = transform.position.x;

                // Clamp maximum left drag 
                if (transform.position.x >= pos_move.x)
                    if (transform.position.x - pos_move.x > maxDragX)
                        pos_move.x = transform.position.x - maxDragX;
                if (pos_move.x <= 0)
                    pos_move.x = 0;

                // Clamp maximum right drag
                if (transform.position.x <= pos_move.x)
                    if (pos_move.x - transform.position.x > maxDragX)
                        pos_move.x = transform.position.x + maxDragX;
                if (pos_move.x >= 11)
                    pos_move.x = 11;
            }
            else
                pos_move.x = transform.position.x;


            if (Mathf.Abs(pos_move.y - prevPos.y) > 0.5) {
                allowDragY = true;
            }
            prevPos.y = transform.position.y;
            bool checkDown = Physics.Raycast(transform.position, Vector3.down, size*2);
            if (checkDown) {
                //print(allowDragY);
                allowDragY = false;
            }
            
            if (allowDragY) {
                // Calculate y position movement
                if (pos_move.y > transform.position.y)
                    pos_move.y = transform.position.y;
                if (transform.position.y - pos_move.y > maxDragY)
                    pos_move.y = transform.position.y - maxDragY;
                Globals.decreaseTimer += Time.fixedDeltaTime;
            }
            else
                pos_move.y = transform.position.y;

            // Update to new adjusted position
            transform.position = new Vector3(pos_move.x, pos_move.y, pos_move.z);
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
    */
    // Update is called once per frame
    void FixedUpdate() {
        if(!Globals.paused) {
            // Check to see if either side of the cube is blocked by an object downwards
            // Round the y position of the block as it hits an object provided that object isn't falling
            // If the cube is white then call the function to destroy nearby cubes
            Vector3 originL = new Vector3(transform.position.x - size + 0.01f, transform.position.y, transform.position.z);
            Vector3 originR = new Vector3(transform.position.x + size - 0.01f, transform.position.y, transform.position.z);

            if (Physics.Raycast(originL, Vector3.down, out hit, size) || Physics.Raycast(originR, Vector3.down, out hit, size)) {
                fall = false;
                fallSpeed = 2;
                if (hit.transform.tag == "Boundary") {
                    transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), transform.position.z);
                    checkBomb();
                }
                else if (hit.transform.tag == "Cube") { 
                    if (!hit.transform.GetComponent<CubeProperties>().fall) {
                        transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), transform.position.z);
                        checkBomb();  
                    }
                }
            }
            else
                fall = true;


            warning = false;
            // If there is an object below this one that is positioned over the threshold
            // then set the cube with a warning property
            if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
                if (hit.transform.tag == "Cube")
                    if (hit.transform.position.y >= Globals.warningThreshold && !hit.transform.GetComponent<CubeProperties>().fall)
                        warning = true;
            }
            // Check if any cube above this one is positioned over the threshold and set warning
            RaycastHit[] targets = Physics.RaycastAll(transform.position, Vector3.up);
            if(targets.Length > 0) {
                foreach (RaycastHit h in targets) {
                    if (h.transform.tag == "Cube")
                        if (h.transform.position.y > Globals.warningThreshold && !h.transform.GetComponent<CubeProperties>().fall)
                            warning = true;
                }
            }
            

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
            if (transform.position.x < 1)
                transform.position = new Vector3(1, transform.position.y, transform.position.z);
            if (transform.position.x > 10)
                transform.position = new Vector3(10, transform.position.y, transform.position.z);

            // Change this cube's frame colour
            if (warning)
                transform.GetChild(0).GetComponent<Renderer>().material = matWarning;
            else if (selected)
                transform.GetChild(0).GetComponent<Renderer>().material = matSelected;
            else
                transform.GetChild(0).GetComponent<Renderer>().material = mat;
        }

        // Cubes spawned in menu
        if(menuCube) {
            transform.GetChild(0).GetComponent<Renderer>().material = matSelected;
            if (transform.position.y <= -11)
                Destroy(transform.gameObject);
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

    void destroyCube(Vector3 dir) {
        if (Physics.Raycast(transform.position, dir, out hit, size * 2)) {
            if (hit.transform.tag == "Cube") {
                if (hit.transform.GetComponent<CubeProperties>().selected)
                    Globals.clearGroup();
                Destroy(hit.transform.gameObject);
                Globals.score++;
            }
        }
    }

    // If cube is a white bomb, destroy surrounding blocks
    public void checkBomb() {
        if(colour == Color.white) {

            // Find and destroy cubes in all surrounding directions upon landing
            destroyCube(Vector3.left);
            destroyCube(Vector3.right);
            destroyCube(Vector3.down);
            destroyCube(new Vector3(1, 1, 0));
            destroyCube(new Vector3(1, -1, 0));
            destroyCube(new Vector3(-1, -1, 0));
            destroyCube(new Vector3(-1, 1, 0));

            /*
            int numOfRays = 8;
            float angle = 0;
            for(int i = 0; i < numOfRays; i++) {
                float x = Mathf.Sin(angle);
                float y = Mathf.Cos(angle);
                Vector3 dir = new Vector3(x, y, 0);
                angle += 2 * Mathf.PI / numOfRays;
                Debug.DrawLine(transform.position, dir, Color.red, 2);
                destroyCube(dir);
            }*/
            if (Globals.group.Contains(gameObject))
                Globals.clearGroup();
            Globals.score++;

            // Create an expanding frame on destroy
            Instantiate(wireFrame, transform.position, Quaternion.identity);

            Destroy(gameObject);
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
