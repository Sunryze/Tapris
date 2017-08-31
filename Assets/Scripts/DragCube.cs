using UnityEngine;
using System.Collections;

/*  Controls touch and drag input behaviour 
 * 
 */

public class DragCube : MonoBehaviour {

    private float distance;
    private bool dragging;
    private Vector3 offset;
    private Transform target;
    private RaycastHit hit;
    private CubeProperties cube;
    private float maxDragX = 0.5f;
    private float maxDragY = 0.08f;
    private int scoreInc;


    void Start() {

    }

    // Update is called once per frame
    void Update() {   
        
        if (Input.touchCount < 1) {
            dragging = false;
            return;
        }
        //Touch[] touch = Input.touches;
        Touch touch = Input.GetTouch(0);

        if (!Globals.paused && !Globals.gameOver) {
            if (touch.phase == TouchPhase.Began) {
                Debug.Log("touch");
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Cube") {
                    target = hit.transform;
                    dragging = true;
                    cube = hit.transform.GetComponent<CubeProperties>();
                    cube.selected = true;
                    cube.allowDragX = false;
                    cube.allowDragY = false;
                    cube.prevPos = target.position;
                    // Clear previous selections and select this cube
                    Globals.clearGroup();
                    Globals.group.Add(target.gameObject);
                    // Select all connected cubes if this one isn't falling
                    if (!cube.fall) {
                        cube.selectAdj(Vector3.left);
                        cube.selectAdj(Vector3.right);
                        cube.selectAdj(Vector3.up);
                        cube.selectAdj(Vector3.down);
                    }
                    cube.distance_to_screen = Camera.main.WorldToScreenPoint(target.position).z;
                    Vector3 temp_pos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cube.distance_to_screen));

                    cube.offsetX = target.position.x - temp_pos.x;
                }
            }

            if (cube.colour != new Color(0.5f, 0.5f, 0.5f)) {
                if (dragging && touch.phase == TouchPhase.Moved) {
                    Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, cube.distance_to_screen));
                    Vector3 cube_pos = target.position;
                    pos_move.x = pos_move.x + cube.offsetX;
                    // Make sure object is dragged and not moved slightly
                    if (Mathf.Abs(pos_move.x - cube.prevPos.x) > 0.3f) {
                        cube.allowDragX = true;
                    }
                    if (cube.allowDragX) {
                        // Check if an object is blocking this cube to the left
                        // The +/- 0.01f is necessary to prevent collision detection when a cube is touching from above/below
                        Vector3 originU = new Vector3(cube_pos.x, cube_pos.y + Globals.cubeSize - 0.01f, cube_pos.z);
                        Vector3 originD = new Vector3(cube_pos.x, cube_pos.y - Globals.cubeSize + 0.01f, cube_pos.z);
                        if (Physics.Raycast(originU, Vector3.left, Globals.cubeSize) || Physics.Raycast(originD, Vector3.left, Globals.cubeSize))
                            cube.dragLeft = false;
                        else
                            cube.dragLeft = true;

                        // Check if an object is blocking this cube to the right
                        if (Physics.Raycast(originU, Vector3.right, Globals.cubeSize) || Physics.Raycast(originD, Vector3.right, Globals.cubeSize))
                            cube.dragRight = false;
                        else
                            cube.dragRight = true;

                        
                        // Raycast to see where objects are to the left and right
                        // Prevent fast dragging from ignoring collision
                        if (Physics.Raycast(originU, Vector3.left, out hit) || Physics.Raycast(originD, Vector3.left, out hit))
                            if (pos_move.x < hit.transform.position.x)
                                pos_move.x = hit.transform.position.x + 1;
                        if (Physics.Raycast(originU, Vector3.right, out hit) || Physics.Raycast(originD, Vector3.right, out hit))
                            if (pos_move.x > hit.transform.position.x)
                                pos_move.x = hit.transform.position.x - 1;
                        

                        // Prevent dragging left or right if an object is blocking in that direction
                        if ((!cube.dragLeft && pos_move.x <= cube_pos.x) || (!cube.dragRight && pos_move.x >= cube_pos.x))
                            pos_move.x = cube_pos.x;

                        // Clamp maximum left drag 
                        if (cube_pos.x >= pos_move.x)
                            if (cube_pos.x - pos_move.x > maxDragX)
                                pos_move.x = cube_pos.x - maxDragX;
                        if (pos_move.x <= 0)
                            pos_move.x = 0;

                        // Clamp maximum right drag
                        if (cube_pos.x <= pos_move.x)
                            if (pos_move.x - cube_pos.x > maxDragX)
                                pos_move.x = cube_pos.x + maxDragX;
                        if (pos_move.x >= 11)
                            pos_move.x = 11;
                    }
                    else
                        pos_move.x = cube_pos.x;


                    if (Mathf.Abs(pos_move.y - cube.prevPos.y) > 0.5) {
                        cube.allowDragY = true;
                    }
                    cube.prevPos.y = cube_pos.y;
                    bool checkDown = Physics.Raycast(cube_pos, Vector3.down, Globals.cubeSize * 2);
                    if (checkDown) {
                        //print(allowDragY);
                        cube.allowDragY = false;
                    }

                    if (cube.allowDragY) {
                        // Calculate y position movement
                        if (pos_move.y > cube_pos.y)
                            pos_move.y = cube_pos.y;
                        if (cube_pos.y - pos_move.y > maxDragY)
                            pos_move.y = cube_pos.y - maxDragY;
                        Globals.decreaseTimer += Time.fixedDeltaTime;
                    }
                    else
                        pos_move.y = cube_pos.y;

                    // Update to new adjusted position
                    cube_pos = new Vector3(pos_move.x, pos_move.y, pos_move.z);
                    target.transform.position = cube_pos;
                }
            }

            if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) {
                Debug.Log("release");
                if (target.position != cube.prevPos)
                    Globals.clearGroup();
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit, 10)) {
                    if (hit.transform.tag == "Cube") {
                        if (hit.transform.GetComponent<CubeProperties>().selected) {
                            if (Globals.group.Count >= 3) {
                                scoreInc = -2;  // Begin at -2 so once 3 are counted the points start at 1
                                foreach (GameObject o in Globals.group) {
                                    Destroy(o);
                                    scoreInc++;
                                }
                                // 3 points for 3 cubes and 3 additional points per extra cube
                                Globals.score += (scoreInc * 3);
                            }
                            cube.selected = false;
                        }
                    }
                }
                Globals.clearGroup();
                target.position = new Vector3(Mathf.Round(target.position.x), target.position.y, target.position.z);
                dragging = false;
               
            }
        }

        /*
        // Detect touch and assign target to object
        if (touch.phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if(Physics.Raycast(ray, out hit) && hit.transform.tag == "Cube") {
                if (hit.collider.gameObject.GetComponent<CubeProperties>().draggable == true) {
                    Debug.Log("here");
                    target = hit.transform;
                    prop = hit.transform.GetComponent<CubeProperties>();
                    distance = target.position.z - Camera.main.transform.position.z;
                    update = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, target.position.y, distance));
                    offset = target.position - update;
                    dragging = true;
                }
            }
        }

        // Control drag movement
        if(dragging && touch.phase == TouchPhase.Moved)
        {
            // Set update position to cursor position
            update = new Vector3(Input.mousePosition.x, target.position.y, distance);
            update = Camera.main.ScreenToWorldPoint(update);

            // Clamp maximum left drag
            if (update.x < transform.position.x)
                if (transform.position.x - update.x > maxDistance)
                    update.x = transform.position.x - maxDistance;

            // Clamp maximum right drag
            if (update.x > transform.position.x)
                if (update.x - transform.position.x > maxDistance)
                    update.x = transform.position.x + maxDistance;

            // Clamp the x position if an object is adjacent to it
            if ((!prop.dragLeft && update.x <= target.position.x) || (!prop.dragRight && update.x >= target.position.x))
                update.x = target.position.x;

            // Update y value
            if (prop.fall)
                update.y = target.position.y - prop.fallSpeed * Time.fixedDeltaTime;

            // Update to new position
            target.position = update + offset;

        }

        // End dragging
        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            dragging = false;
            transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, transform.position.z);
        }*/
    }
}
